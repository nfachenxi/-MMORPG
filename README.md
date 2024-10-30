# 《极世界》MMORPG

### 项目介绍：

本项目为Unity全栈工程师系列课程P2阶段的《极世界》MMORPG 教学项目

### 可能遇到的问题及修复方案：

1.数据库实体无法正常删除

出现原因：在本地通过模型创建数据库后，在本地删除了实体但未在数据库端删除指定实体就新建查询

解决方法：手动在数据库删除指定实体再新建查询

2.Beyond Compare 4在使用一段时间后显示注册被吊销

出现原因：破解失效

解决方法：删除C:\Users\用户名\AppData\Roaming\Scooter Software\Beyond Compare 4，删除之后重新输入破解凭证即可

3.客户端报错定位到 NetClient.Instance.OnConnect -= OnGameServerConnect;语句

出现原因：NetClient未初始化

解决方法：在场景中某一物体上挂载NetClient脚本即可

4.服务端无法连接到数据库、

出现原因1：app.config文件中并未设置远端服务器的登录密码

解决方法1：在username语句后加上password语句，并填写密码
