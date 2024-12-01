# Netcool
# 介绍
`Netcool`是一个基于.net core的Web应用脚手架，可以用于快速搭建后台管理系统，或者一个简单Web Api。

`Netcool`采用前后端分离的方式，包含Netcool.Api，[Netcool.Admin](https://github.com/NeilQ/Netcool.Admin)两个主要项目，
包含用户、菜单、权限等基础功能。

同时，Netcool系列还包含一些便捷的开发库以满足日常使用：
 - [Netcool.Caching](https://github.com/NeilQ/Netcool/tree/master/src/extensions/Netcool.Caching)
 - [Netcool.Excel](https://github.com/NeilQ/Netcool/tree/master/src/extensions/Netcool.Excel)
 - [Netcool.HttpProxy](https://github.com/NeilQ/Netcool/tree/master/src/extensions/Netcool.HttpProxy)
- [Netcool.Swashbuckle.AspNetCore](https://github.com/NeilQ/Netcool/tree/master/src/extensions/Netcool.Swashbuckle.AspNetCore)

[![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)](https://jb.gg/OpenSourceSupport)

# 项目依赖
在集成一些基础设施时，Netcool尽量使用Microsoft的官方推荐方案，或者使用比较主流、Star数最多并且轻量的第三方Package。 
过多造轮子会增加使用者的学习精力，过多的封装会让人使用起来摸不着头脑，
而主流的第三方库大家可能都比较熟悉了，有完善的文档，使用起来更加顺畅。

目前使用到的基础设施Package：
- ORM: Entity Framework core
- Logger: Serilog
- Swagger: Swashbuckle.AspNetCore
- Authentication: JwtBearer
- Mapper: Mapster

其他帮助类均根据 [官方文档](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0) 中推荐的解决方案实现。

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

为了方便本地调式与局域网调用，Netcool准备了Ip白名单授权，任何符合白名单Ip的请求，在没有JwtBearer的情况下，也可以通过授权验证。

```c#
 services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddIpWhitelist(o =>
    {
        o.Enable = true;
        o.Ips = new {"::1", "127.0.0.1"};
    })
    .AddJwtBearer(options =>{});
 
 services.AddAuthorization(options =>
 {
     var defaultAuthorizationPolicyBuilder =
         new AuthorizationPolicyBuilder(IpWhitelistAuthenticationDefaults.AuthenticationScheme,
             JwtBearerDefaults.AuthenticationScheme);
     defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
     options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
 })
```

如果仅仅需要在开发环境下允许不授权调用，也可以通过配置给所有Controller加上`[AllowAnonymous]`。
```c#
app.UseEndpoints(endpoints =>
{
    // Add AllowAnonymousAttribute to all actions for dev env
    if (env.IsDevelopment())
        endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
    else
        endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});
```

### 公告
基于富文本的系统公告。

### 应用配置
Netcool将会检索运行目录下的conf文件夹，将所有.json文件添加到配置中，方便使用Docker部署时映射外部文件以覆盖默认配置，你也可以自定义你的配置文件夹：
```c#
 public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().UseIISIntegration(); })
                .ConfigureAppConfiguration(configBuilder =>
                {
                    var connectionString = configBuilder.Build().GetConnectionString("Database");
                    configBuilder.AddEfConfiguration(options => { options.UseNpgsql(connectionString); }, true);
                    configBuilder.AddJsonFileFromDirectory(Common.IsWindows ? "conf" : "/conf");
                });
```

此外，Netcool提供了`EFConfigurationProvider`，将数据库中的配置信息适配到内置的`Configuration`中，可以通过注入`IConfiguration`或者`IOptions`获取数据库中的配置信息，如UserService:
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
{
  "File": {
    "UseHttps": false,
    "HttpHost": "",
    "SubWebPath": "file",
    "PhysicalPath": "D:\\netcool-resources"
  }
}
```
- UseHttps: 资源schema是否使用`https`。
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

# 如何自定义一个CRUD Api
假设我们要添加一个User Api，我们需要创建哪些对象呢？
### 添加Entity
创一个`Entity`对象，并实现`IEntity<TPrimaryKey>`接口，在`Netcool.Core.Entities`命名空间下，
有一些常用的Entity基类，包含一些常用字段，比如 `FullAuditEntity`就包含了`CreateTime`, `CreateUserId`, `UpdateTime`,`UpdateUserId`, `IsDelete`等常用字段,
这些字段在`DbContext`基类中持久化到数据库时将自动赋值。
```c#
public class User : FullAuditEntity
{
    public string Name { get; set; }

    public string DisplayName { get; set; }
    
    public Gender Gender { get; set; }
    
    public Organization Organization { get; set; }
}
```

### 添加Repository
Netcool已经准备了通用的`CommonRepository<TEntity>`，包含了大部分的数据库操作，一般情况下我们不需要自己创建，直接使用`IRepository<User, int>`类型的依赖就可以了，
但如果有自定义的需求，仍然可以实现自己的`IRepository`。
```c#
public interface IUserRepository : IRepository<User>
{
    void DoSomething();
}
    
public class UserRepository : CommonRepository<User>, IUserRepository
{
    public override IQueryable<User> GetAll()
    {
        return GetAllIncluding(t => t.Organization);
    }

    public UserRepository(NetcoolDbContext dbContext) : base(dbContext)
    {
    }
    
    public void DoSometing() 
    {
    }
}
```

### 添加Dto
`EntityDto`对象用于从Controller到Service传递用户输入信息，理论上它不应该传递到Repository层
```c#
public class UserDto : UserSaveInput
{
    public IList<RoleDto> Roles { get; set; }

    public string GenderDescription => Reflection.GetEnumDescription(Gender);

    public OrganizationDto Organization { get; set; }
}

public class UserSaveInput : EntityDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(64)]
    public string DisplayName { get; set; }

    public Gender Gender { get; set; }

    public int? OrganizationId { get; set; }
}

public class UserRequest : PageRequest
{
    public string Name { get; set; }

    public Gender? Gender { get; set; }

    public int? OrganizationId { get; set; }
}
```

其中`UserDto`用于返回用户信息到客户端，`UserSaveInput`用于添加及更新`User`时传递客户端输入，
`UserRequest`用于查询用户信息时通过QueryString检索数据。

我们使用`Mapster`库来动态映射Dto与Entity，对于自定义字段映射可以参考`Mapster`文档。


### 添加Service
默认的`ICrudService`、`CrudService`已经包含了常用的操作以及重载方法，我们只需要继承它就可以了。
```c#
public interface IUserService : ICrudService<UserDto, int, UserRequest, UserSaveInput>
{

}

public sealed class UserService : CrudService<User, UserDto, int, UserRequest, UserSaveInput>, IUserService
{
    private readonly string _defaultPassword;

    private new readonly IUserRepository Repository;
    private readonly IRepository<Organization> _orgRepository;

    public UserService(IUserRepository userRepository,
        IServiceAggregator serviceAggregator,
        IConfiguration config,
        IRepository<Organization> orgRepository) : base(
        userRepository,
        serviceAggregator)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _orgRepository = orgRepository;

        GetPermissionName = "user.view";
        UpdatePermissionName = "user.update";
        CreatePermissionName = "user.create";
        DeletePermissionName = "user.delete";
    }

    protected override IQueryable<User> CreateFilteredQuery(UserRequest input)
    {
        var query = base.CreateFilteredQuery(input)
          .WhereIf(!string.IsNullOrEmpty(input.Name), t => t.Name == input.Name);

        return query;
    }

    public override async Task BeforeCreate(User entity)
    {
        base.BeforeCreate(entity);
        entity.Name = entity.Name.SafeString();

        if (entity.OrganizationId > 0)
        {
            var org = await _orgRepository.GetAsync(entity.OrganizationId.Value);
            if (org == null) throw new EntityNotFoundException(typeof(Organization), entity.Id);
        }
        else entity.OrganizationId = null;
    }

    public override async Task BeforeUpdate(UserSaveInput dto, User originEntity)
    {
        base.BeforeUpdate(dto, originEntity);
        dto.Name = dto.Name.SafeString();
        if (dto.OrganizationId > 0)
        {
            var org = await _orgRepository.GetAsync(dto.OrganizationId.Value);
            if (org == null) throw new EntityNotFoundException(typeof(Organization), dto.Id);
        }
        else dto.OrganizationId = null;
    }
}
```

### 添加依赖注入
Netcool可以将所有同一个Assembly下，名字以`Repository`或`Service`结尾， 并且实现了`IRepository`或`IService`的类全部添加到IoC容器，
因此我们不需要一个个得添加。
```csharp
   services.AddScoped(typeof(IRepository<>), typeof(CommonRepository<>));
   services.AddScoped(typeof(IRepository<,>), typeof(CommonRepository<,>));
   services.AddTransient<IServiceAggregator, ServiceAggregator>();
   services.AddDomainRepositoryTypes(Assembly.GetAssembly(typeof(NetcoolDbContext)), ServiceLifetime.Scoped);
   services.AddDomainServiceTypes(Assembly.GetAssembly(typeof(NetcoolDbContext)), ServiceLifetime.Scoped);
```

### 添加Controller
最后，与`CrudService`类似，创建`UserController`并继承`CrudControllerBase`或者`QueryControllerBase`。
```c#
    [Route("users")]
    [Authorize]
    public class UsersController : CrudControllerBase<UserDto, int, UserRequest, UserSaveInput>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userUserService) :
            base(userUserService)
        {
            _userService = userUserService;
        }
    }
```

完成这一步后，即可在swagger页面看到CRUD Api

# Docker部署
根目录下已经准备了`Dockerfile`，编译成镜像后即可直接运行
```
docker build -t netcool-api .

docker run -dit --restart always --log-opt max-size=50m -p 8000:8080 \
-v /mnt/logs:/logs -v /mnt/conf:/conf  \
--name netcool-api netcool-api 
```

注意：.net8容器默认开放端口号为`8080`, .net8以前的容器默认端口号为`80`

# 前端开发
采用 Angular + ng-alain框架开发，位于`/src/fontend`目录下，使用.net web项目做为host，`ClientApp`下的前端项目文件可独立运行。


