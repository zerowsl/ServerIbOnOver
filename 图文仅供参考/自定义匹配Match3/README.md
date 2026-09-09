此功能操作过多，上手难度或许过高，需要有一定的计算机知识和动手能力。所以要根据自身情况量力而行...

1. 安装docker，请自行搜索查阅 windows下的wsl2如何安装docker ... <br/>
   安装docker-compose，请自行搜索查阅 ...

2. 可选修改‘exvs2_match3’文件夹里面的配置文件 <br/>
   并把‘exvs2_match3’文件夹放到 wsl2 里去

3. 在wsl2里面输入以下命令
``` shell
# 定位到具体‘exvs2_match3’文件夹位置
cd ~/exvs2_match3

# 运行容器
docker compose up -d
```

4. 配置并运行stun服务,详情参考[stun说明](../本地店内外-启动器配置/docker容器版stun/)

5. 打开联机程序和配置启动器, 例如 UU , 获取你自己(或者开服务的那个人)的ip ... <br/>
   [UU房获取ip](./UU房获取ip.png)

6. 修改刷卡服务的‘server.json’文件里面的‘LocalMatchingConfigs’项配置, 看说明操作... <br/>
   填写上一步获取到的ip ... <br/>
  
7. 开放端口, 请自行搜索查阅 ...  <br/>
   默认情况, match3使用tcp端口28083，stun使用udp端口3478 

8. 如想要游玩双人匹配，另一个人如果用自身的本地刷卡，那么需要操作第‘5’步和第‘6’步来修改配置(前提是他的刷卡要支持修改)... <br/>
   当然别人也可以使用你的刷卡... <br/>

# 参考
- [单人店外ClassMatchG下的1(+cpu)vs1(+cpu)](./单人店外ClassMatchG下的1(+cpu)vs1(+cpu).png)
- [双人店外ClassMatchG下的2v2cpu](./双人店外ClassMatchG下的2v2cpu.png)