在局域网下，你不是win11无法开启镜像网络模式，<br/>
或者你手机牛逼可以运行手机版ib... <br/>
如需要开启stun，此时可以尝试arm64的容器版stun...

1. 安卓无root的容器app，可以安装这个[Podroid](https://github.com/ExTV/Podroid)
2. 安装app后设置，记得[打开3478端口](./Podroid打开端口.jpg)
3. 启动并进入终端运行命令启动容器
```shell
# 启动容器，其中1.1是版本号，如有新版要替换成新版
docker run -d --name exvs2_local_stun -p 3478:3478/udp vl00at111/exvs2_local_stun:1.1-arm64v8
```
4. 下载并安装[uu安卓版手机app](https://uu.163.com/), 官网点手游加速会看到安卓版下载...
5. 进入uu房间，同一个账号是可以跟pc端一起进入同一间房的
6. 参考之前方法，获取你手机的uu的ip，及修改刷卡server.json里相应的配置...
7. [运行情况参考](./运行情况参考.jpg)
