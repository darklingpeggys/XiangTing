# 箱庭使用指南
## 前言
本工具是一个蓝鸟插件，作用是简化操作，多号操作

批量养殖，批量洗版等等

有做插件开发接口，之后会写开发手册，可以自行尝试增加功能

当前正在做更加合适的养号插件，相关养号策略收集中，欢迎讨论

## 第一步，获取登录信息
本工具不采用多号登录的方式进行操作，而是使用身份信息来完成相关行为，因此添加账号到工具中采用的是输入Cookie文件的方式

刚买的号自动获取时可能遇到邮箱验证，需要手动解邮箱

获取Cookie有两种方法，一种可以使用提供的自动登录工具，一种可以直接在浏览器上手动提取

### 自动获取工具
本工具的作用是批量登录账号，获取Cookie
![工具界面](https://github.com/MengLuwa/XiangTing/blob/master/Image/自动登录工具界面.png)

点击打开账号文件按钮，选择账号文件

账号文件中的格式为：

用户名1:密码1

用户名2:密码2

……

后续可以添加东西，会被无视，如毛子的格式：

用户名1:密码1:邮箱1:邮箱密码1

用户名2:密码2:邮箱2:邮箱密码2

……

这种格式也可以：

KatieGurule4:xxxxx:naumxxxxxxxx9@outlook.com:xxxxxx

KaylaWa93548703:xxxxx:inoxxxxx@outlook.com:xxxxxx

KimberlyRud12:xxxxx:BR:mxxxxxban@hotmail.com:xxxxxx

JoannaH80951422:xxxxx:rozxxxxxeenidge1993@hotmail.com:xxxxxx

HollyRo80583081:xxxxx:intxxxxxcom@outlook.com:xxxxxx

AnaClar12810614:xxxxx:milxxxxxia00@outlook.com:xxxxxx

登陆之后，可以看到类似这样的提示：

![获取Cookie成功](https://github.com/MengLuwa/XiangTing/blob/master/Image/获取Cookie成功.png)

账号文件同目录下可以看到一个Cookie文件，这个就是我们的身份信息了

### 手动获取方法
打开浏览器匿名模式页面，登录推特，按下F12（不同浏览器快捷键可能不同）打开开发者工具

选择Application(应用)这一栏，找到Cookie：

![获取Cookie](https://github.com/MengLuwa/XiangTing/blob/master/Image/获取Cookie.png)

这里有三个值，ct0，auth_token，_twitter_sess，这三个是我们需要的

这三个值缺了的话重新登录一遍就有了

将这三个的Value复制下来，按照以下格式记录到文本：

auth_token;ct0;_twitter_sess;用户名

每行一个，保存到文本文件，改为Cookie名称，不要后缀即可

## 第二步，载入箱庭
打开箱庭工具，选项，添加角色，找到之前的Cookie文件，载入：

![载入角色](https://github.com/MengLuwa/XiangTing/blob/master/Image/载入角色.png)

载入角色之后可以看到角色出现在列表中了，接下来就是选择任务和计划功能了

**任务**指的是一次动作，比如点个赞，转个推这种，短暂一次性的动作

**计划**指的是长期的计划，计划会根据设置不断发布任务交给角色去执行，比如自动活跃，比如攻击Tag

由于采用了插件的方式，需要加入新的任务和计划类型可以自行开发插件，之后会编写开发接口文档

记住，武器开发出来了，但做什么就是取决与执掌武器的人

冲蝗从来都是破坏行动，不要妄想我们能保护什么，拿上武器的人做的只有摧毁敌人

祝各位武运昌隆！

## 相关链接
[箱庭下载](https://github.com/MengLuwa/XiangTing/releases/tag/箱庭v1.0.0)
[毛子军火库](https://accsmarket.com)
[接码平台](https://smspva.com)https://accsmarket.com
