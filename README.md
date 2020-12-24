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
Netcool默认使用Postgresql数据库，使用其他数据库只需通过EF更换数据库适配器，并修改`NetcoolDbContext`类中部分不兼容的代码。
首次同步数据库时，建议将`Netcool.Api.Domain`中的`Migrations`文件夹删除，重新生成同步文件。

# 内置模块
### 菜单与权限
菜单与相应权限目前仅支持直接在`InitialEntities`类中初始化并同步到数据库中。
在实际开发中发现，在UI编辑菜单或权限预定于内容并不是一个好的选择，经常会造成在不同环境中数据不一致，
因此由开发人员预定义并提供数据库同步脚本更加适合，统一职责范围，避免不必要的冲突。

### 用户与授权
Netcool使用JwtBearer进行用户授权，访问Api时，需要添加请求头： `Authorization: Bearer {token}`。
通过`[Authorize]`与`[AllowAnonymous]`属性控制Action是否需要访问授权。

### 应用配置
Netcool将会检索运行目录下的conf文件夹，将所有.json文件添加到配置中，方便使用Docker部署时映射外部文件以覆盖默认配置。

此外，Netcool提供了`EFConfigurationProvider`，将数据库中的配置信息适配到内置的`Configuration`中，可以通过注入`IConfiguration`或者`IOptions`获取数据库中配置的权限，如UserService:
```c#
 public UserService(IUserRepository userRepository,
            IServiceAggregator serviceAggregator,
            IRepository<Role> roleRepository,
            IConfiguration config,
            IRepository<UserRole> userRoleRepository) : base(
            userRepository,
            serviceAggregator)
        {
            ......            
            
            _defaultPassword = config.GetValue<string>("User.DefaultPassword");
            
            ......
        }
```

### 文件上传
启用文件上传需要在`appsettings.json`中加入配置
```json
"File" : {
    "HttpSchema":""    
    "HttpHost": "",
    "SubWebPath": "file",
    "PhysicalPath": "D:\\netcool-resources"
  }
```
- HttpSchema: http或者https。当该值为空时将会从`HttpContext.Request.Schema`中读取。
- Host: 访问文件资源时使用的域名。当该值为空时将会从`HttpContext.Request.Host`中读取，
如果使用了多层代理，需要注意配置`X-Forwarded-Host`请求头。为了方便，可以直接为该值配置域名
- SubWebPath: 访问文件资源跟在域名后的二级路径，注意不能与`ApiController`中定于的路由相同。
- PhysicalPath: 物理文件路径

当Host="www.domain.com" SubWebPath="file"时，文件上传后返回的Dto中将包含URL:https://www.domain.com/file/20201212/xxx.png。
URL拼接操作在AutoMapper中自动处理。

### 权限验证
Netcool使用 [基于资源的授权](https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/resourcebased?view=aspnetcore-5.0) 的方式校验权限，
并且兼容`Role-based`与`Claim-based`授权方式。

#### 添加权限定义
将自定义的权限名称添加到`InitialEntities`中，并通过EF工具同步到数据库。当然，也可以随时更改`PermissionPolicyProvider`或者实现`IAuthorizationPolicyProvider`来自定义你的权限定义获取方式。

#### 在Service中校验权限
基础的CRUD操作可以直接通过给`GetPermissionName`、`UpdatePermissionName`、`CreatePermissionName`、`DeletePermissionName`属性赋值。
其他操作权限可以调用Service中的`CheckPermission`或者`AuthorizationService.AuthorizeAsync`方法

#### 在Controller中校验权限
为Action添加属性`[Authorize("permission")]`

# 如何自定义一个模块
TODO

# Docker部署

# 前端开发
TODO


