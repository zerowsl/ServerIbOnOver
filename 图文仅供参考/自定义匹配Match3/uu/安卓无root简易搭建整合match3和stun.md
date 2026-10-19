(手机都用来玩模拟器和ib了，装个docker又怎么了)... <br>

# 安卓无root简易搭建整合match3和stun

1. 安卓无root的容器app，可以安装这个[Podroid](https://github.com/ExTV/Podroid)最新版 <br>
   安装后自带docker和docker-compose ..

2. 安装app后设置，记得[打开端口和打开下载文件夹共享](../../stun/适配安卓arm64架构下的docker容器版/Podroid打开端口.jpg)

3. 可选修改‘exvs2-match3’文件夹里面的配置文件 <br>
   并把‘exvs2-match3’文件夹放到 手机的Download目录(一般是/sdcard/Download) 里去

4. 启动并进入终端, 按提示输入用户和密码后，运行命令
```shell
# 定位到具体‘exvs2-match3’文件夹位置
# 其中 /mnt/downloads/ 是共享你手机的Download目录
cd /mnt/downloads/exvs2-match3

# 启动前注意yaml文件里镜像的版本号，如有新版要替换成新版
docker compose up -f docker-compose-arm64v8-with-match3-stun.yaml -d
```

5. 以上是第一次安装，后续启动就是直接启动app和app里面启动环境就是了... <br>
   如要关闭app，可以在app内点停止，或者直接划走app就可以了...

6. 下载并安装[uu安卓版手机app](https://uu.163.com/), 官网点手游加速会看到安卓版下载...

7. 开启或进入uu房间，同一个账号是可以跟pc端一起进入同一间房的 <br>
   [UU房获取ip](../media/UU房获取ip.png)

8. pc和手机浏览器都可以访问以下地址测试是否安装成功, match3默认端口是28083 <br>
   http://172.19.163.2:28083/test (ip是上面获得你开服务的uu的ip) <br>
   http://10.95.96.4:28083/test (ip是你app的ip)

9. 后续操作依旧...

10. [运行状况参考](../media/安卓下match3运行状况.jpg)
