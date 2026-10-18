using Microsoft.EntityFrameworkCore;
using ServerOver.Persistence;

namespace ServerOver.BackgroundServices;

/// <summary>
/// 原ob对战历史页面只显示前100条，并且不分页，也没定时清理功能.. <br/>
/// 开启这个功能会定时清理每个用户的对战历史时间按倒序排列多于120条的记录.
/// </summary>
internal class ClearUpTooMuchBattleHistoryBgService(IServiceScopeFactory serviceScopeFactory, 
    ILogger<ClearUpTooMuchBattleHistoryBgService> log)
    : BackgroundService
{
    const int _loopMs = 1000 * 60 * 60 * 1;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        log.LogInformation("ClearUpTooMuchBattleHistory bg-service is running in background ...");
        
        for (var b = true; !stoppingToken.IsCancellationRequested; b = false)
        {
            try
            {
                if (!b) await Task.Delay(_loopMs, stoppingToken);
                await OnLoopEach(stoppingToken);
            }
            catch when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "error on ClearUpTooMuchBattleHistory bg-service");
            }
        }
    }

    internal async Task OnLoopEach(CancellationToken cancellation)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
        
        using var transaction = await context.Database.BeginTransactionAsync(cancellation);

        var agoTime = DateTimeOffset.UtcNow.AddMonths(-1).ToUnixTimeSeconds();

        var idsToDelete = await context.Database.SqlQueryRaw<int>("""
            SELECT Id FROM (
                SELECT Id, PlayedAt, 
                       ROW_NUMBER() OVER (PARTITION BY CardId ORDER BY PlayedAt DESC) AS rn
                FROM exvs2ob_battle_history
            ) T
            WHERE rn > {0} -- AND PlayedAt < {1} 
            ;
            """, 120, agoTime)
            .ToListAsync();

        if (idsToDelete.Count == 0) return;

        // 分批删除，避免 IN 子句参数过多（SQLite 默认 SQLITE_MAX_VARIABLE_NUMBER 为 999 或 32766）
        foreach (var batch in idsToDelete.Chunk(200))
        {
            var batchList = batch.ToList();

            // 先删子表, 这些实体都有 BattleHistoryId 外键属性
            await context.BattleTargetDbSet
                .Where(x => batchList.Contains(x.BattleHistoryId))
                .ExecuteDeleteAsync();

            await context.BattleActionLogsDbSet
                .Where(x => batchList.Contains(x.BattleHistoryId))
                .ExecuteDeleteAsync();

            await context.BattleAllyDbSet
                .Where(x => batchList.Contains(x.BattleHistoryId))
                .ExecuteDeleteAsync();

            await context.BattleSelfDbSet
                .Where(x => batchList.Contains(x.BattleHistoryId))
                .ExecuteDeleteAsync();

            // 最后删主表
            await context.BattleHistoryDbSet
                .Where(x => batchList.Contains(x.Id))
                .ExecuteDeleteAsync();
        }

        await transaction.CommitAsync(cancellation);
    }
}