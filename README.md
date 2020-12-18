# Netcool.Api

# 前言
Netcool是一个基于.Net 5的Web应用脚手架，可以用于快速搭建后台管理系统，或者一个简单Web Api。

Netcool采用前后端分离的方式，包含Netcool.Api，Netcool.Admin两个项目，
包含用户、菜单、权限等基础功能。

# 项目依赖
在集成一些基础设施时，Netcool尽量使用Microsoft的官方推荐方案，或者使用比较主流、Star数最多并且轻量的第三方Package。 
过多造轮子会增加使用者的学习精力，过多的封装会让人使用起来摸不着头脑，
而主流的第三方库大家可能都比较熟悉了，有完善的文档，使用起来更加顺畅。

目前使用到的基础设施Package：
- ORM: Entity Framework core
- Logger: Serilog
- Swagger: Swashbuckle.AspNetCore
- Authentication: JwtBearer
- Mapper: AutoMapper

其他帮助类均根据 [官方文档](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0) 中推荐的解决方案实现。

如有流量控制的需求，这里推荐一个Pakcage：[AspNetCoreRateLimit](https://github.com/stefanprodan/AspNetCoreRateLimit)

# 项目分层
根据多年的开发经验，传统的三层框架（DAL，BLL，Application）在业务比较多的情况下，
会导致开发时定位某个模块文件时比较困难，经常要在三个文件夹下大量文件中翻找，
加上有些文件命名并不是十分规范，模块之间的业务边界不是很明确，会给开发者造成困扰。
因此Netcool在组织项目文件层次时，更加偏向与领域模型的方式，将同一模块的文件都放到一个文件夹下。

但要注意的是，项目文件的组织方式并不等同于领域模型的严格应用，对于大型项目或者大流量项目，现在很多时候都会采用微服务的方式进行拆分，其实这种方式就属于领域的拆分，
每个微服务就是一个业务领域，需要在实践中灵活应用。

# 数据库适配

# 内置模块
### 应用配置
### 菜单与权限
### 用户与授权
### 文件上传
### 权限验证
TODO

# 如何自定义一个模块
TODO


# 前端开发
TODO

