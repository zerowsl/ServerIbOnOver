1. 安装docker，可自行搜索查阅 windows下的wsl2如何安装docker ....

2. 参考命令 
``` shell
# 运行容器
docker run -d --name exvs2_local_stun -p 3478:3478/udp vl00at111/exvs2_local_stun

# 如果单机情况下,主机用127.0.0.1访问不了时,需要WSL2的ip访问,Powershell或cmd里运行获取该ip..
wsl hostname -I
```

[容器版可能需要wsl2的ip及其sever.json的配置](./容器版可能需要wsl2的ip及其sever.json的配置.png)

3.参考图[启动器配置](./(双)启动器配置.png) ...
