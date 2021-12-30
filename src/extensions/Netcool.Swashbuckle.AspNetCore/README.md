# Netcool.Swashbuckle.AspNetCore
[![GitHub](https://img.shields.io/github/license/neilq/Netcool)](https://github.com/NeilQ/Netcool/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Netcool.Swashbuckle.AspNetCore)](https://www.nuget.org/packages/Netcool.Swashbuckle.AspNetCore/)
![Nuget](https://img.shields.io/nuget/dt/Netcool.Swashbuckle.AspNetCore)

Extensions library for Swashbuckle.AspNetCore.

## What's included

### EnumDescriptionDocumentFilter
Display enum descriptions for enum member decorated by DescriptionAttribute.

### FileUploadOperationFilter
Put a file upload button for uploading file with content-type:multipart/form-data.

### Extension methods
```
void IncludeAllXmlComments(this SwaggerGenOptions options, bool includeControllerXmlComments = false)

void IncludeXmlComments(this SwaggerGenOptions options, Assembly assembly, bool includeControllerXmlComments = false)

void AddJwtBearerSecurity(this SwaggerGenOptions options)

void InjectHeadContent(this SwaggerUIOptions options, string headContent)
```

## Examples
```c#
services.AddSwaggerGen(c =>
{
    c.DocumentFilter<EnumDescriptionDocumentFilter>();
    c.OperationFilter<FileUploadOperationFilter>();

    c.IncludeXmlComments(Assembly.GetAssembly(typeof(NetcoolDbContext)));
    c.IncludeAllXmlComments();
    
    c.AddJwtBearerSecurity();
});
```

```c#
app.UseSwaggerUI(c =>
 {
     if (env.IsDevelopment())
     {
         c.InjectHeadContent(
             @"<script async id=""mini-profiler"" src=""/profiler/includes.min.js?v=4.2.22+4563a9e1ab""
               data-version=""4.2.22+4563a9e1ab"" data-path=""/profiler/"" 
               data-current-id=""7a3d98bb-3968-41fb-8836-65f9923ee6eb""
               data-ids=""7a3d98bb-3968-41fb-8836-65f9923ee6eb""
               data-position=""Left"" data-scheme=""Light"" data-authorized=""true"" data-max-traces=""15""
               data-toggle-shortcut=""Alt+P"" data-trivial-milliseconds=""2.0"" 
               data-ignored-duplicate-execute-types=""Open,OpenAsync,Close,CloseAsync""></script>");
     }
 });
```

```c#
[HttpPost("upload")]
[ProducesResponseType(typeof(FileDto), 200)]
[FileUploadOperationFilter.FileContentType]
public async Task<IActionResult> Upload()
{
    var fileDto = await UploadMultipart();
    return Ok(fileDto);
}
```

