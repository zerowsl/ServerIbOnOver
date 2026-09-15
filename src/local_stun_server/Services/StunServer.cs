using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StunService.Models;
using System.Net;
using System.Net.Sockets;

namespace StunService.Services;

public class StunServer(ILogger<StunServer> logger, StunProtocol _protocol) 
    : IHostedService, IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private UdpClient? _udpServer;
    private Task? _listenTask;
    
    private int _primaryPort => GlobalVars.PrimaryPort;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting STUN server...");

        try
        {
            // 启动主 STUN 服务器
            var primaryEndPoint = new IPEndPoint(GlobalVars.IPAddress, _primaryPort);
            _udpServer = new UdpClient(primaryEndPoint);
            logger.LogInformation("STUN server listening on port {Port}", _primaryPort);

            // 启动监听任务
            _listenTask = ListenForStunMessagesAsync(_cts.Token);
            
            logger.LogInformation("STUN server started successfully");
        }
        catch (SocketException ex)
        {
            logger.LogError(ex, "Failed to start STUN server on port {Port}", _primaryPort);
            throw;
        }

        await Task.CompletedTask;
    }

    private async Task ListenForStunMessagesAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("STUN message listener started");

        try
        {
            while (!cancellationToken.IsCancellationRequested && _udpServer != null)
            {
                try
                {
                    // 接收 UDP 数据
                    var result = await _udpServer.ReceiveAsync(cancellationToken);
                    
                    logger.LogDebug(
                        "Received {Bytes} bytes from {RemoteEndPoint}",
                        result.Buffer.Length,
                        result.RemoteEndPoint
                    );

                    // 处理 STUN 消息（不阻塞接收循环）
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await ProcessStunMessageAsync(result.Buffer, result.RemoteEndPoint);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error processing STUN message from {RemoteEndPoint}", 
                                result.RemoteEndPoint);
                        }
                    }, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (SocketException ex)
                {
                    logger.LogError(ex, "Socket error while receiving STUN message");
                    await Task.Delay(100, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error in STUN message listener");
        }
        finally
        {
            logger.LogInformation("STUN message listener stopped");
        }
    }

    private async Task ProcessStunMessageAsync(byte[] data, IPEndPoint remoteEndPoint)
    {
        try
        {
            // 解码 STUN 消息
            var request = _protocol.Decode(data);

            logger.LogInformation(
                "Processing {MessageType} from {RemoteEndPoint}, TransactionId: {TransactionId}",
                request.Type,
                remoteEndPoint,
                Convert.ToHexString(request.TransactionId)
            );

            // 根据消息类型处理
            switch (request.Type)
            {
                case StunMessageType.BindingRequest:
                    await HandleBindingRequest(request, remoteEndPoint);
                    break;
                
                default:
                    logger.LogWarning("Unsupported STUN message type: {MessageType}", request.Type);
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid STUN message from {RemoteEndPoint}", remoteEndPoint);
            
            // 发送错误响应
            await SendErrorResponse(
                new byte[12], // 使用空 Transaction ID
                remoteEndPoint,
                400,
                "Bad Request"
            );
        }
    }

    private async Task HandleBindingRequest(StunMessage request, IPEndPoint remoteEndPoint)
    {
        try
        {
            // 创建绑定响应
            var response = new StunMessage
            {
                Type = StunMessageType.BindingResponse,
                TransactionId = request.TransactionId
            };

            // 添加 XOR-MAPPED-ADDRESS 属性（推荐使用）
            var xorMappedAddress = _protocol.CreateXorMappedAddress(
                remoteEndPoint,
                request.TransactionId
            );
            response.Attributes.Add(xorMappedAddress);

            // 添加 MAPPED-ADDRESS 属性（为了兼容性）
            var mappedAddress = _protocol.CreateMappedAddress(remoteEndPoint);
            response.Attributes.Add(mappedAddress);

            // 添加 SOFTWARE 属性
            var software = _protocol.CreateSoftwareAttribute(".NET STUN Server v1.0");
            response.Attributes.Add(software);

            // 编码并发送响应
            var responseData = _protocol.Encode(response);
            
            if (_udpServer != null)
            {
                await _udpServer.SendAsync(responseData, responseData.Length, remoteEndPoint);
                
                logger.LogInformation(
                    "Sent binding response to {RemoteEndPoint} -> Mapped: {Address}:{Port}",
                    remoteEndPoint,
                    remoteEndPoint.Address,
                    remoteEndPoint.Port
                );
                
                logger.LogWarning(GlobalVars.LogFangDaoMai);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling binding request from {RemoteEndPoint}", remoteEndPoint);
            
            await SendErrorResponse(
                request.TransactionId,
                remoteEndPoint,
                500,
                "Server Error"
            );
        }
    }

    private async Task SendErrorResponse(byte[] transactionId, IPEndPoint remoteEndPoint, int errorCode, string reason)
    {
        try
        {
            var errorResponse = new StunMessage
            {
                Type = StunMessageType.BindingErrorResponse,
                TransactionId = transactionId
            };

            var errorCodeAttr = _protocol.CreateErrorCode(errorCode, reason);
            errorResponse.Attributes.Add(errorCodeAttr);

            var responseData = _protocol.Encode(errorResponse);
            
            if (_udpServer != null)
            {
                await _udpServer.SendAsync(responseData, responseData.Length, remoteEndPoint);
                
                logger.LogInformation(
                    "Sent error response to {RemoteEndPoint}: {ErrorCode} {Reason}",
                    remoteEndPoint, errorCode, reason
                );
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending error response to {RemoteEndPoint}", remoteEndPoint);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping STUN server...");
        
        _cts.Cancel();
        
        if (_listenTask != null)
        {
            await _listenTask;
        }
        
        _udpServer?.Close();
        _udpServer?.Dispose();
        
        logger.LogInformation("STUN server stopped");
    }

    public void Dispose()
    {
        _cts.Dispose();
        _udpServer?.Dispose();
    }
}