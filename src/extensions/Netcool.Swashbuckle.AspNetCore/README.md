# Netcool.Swashbuckle.AspNetCore

Extensions library for Swashbuckle.AspNetCore.

## What's included

### EnumDescriptionDocumentFilter
Display enum descriptions for enum member decorated by DescriptionAttribute.

### FileUploadOperationFilter
Put a file upload button for uploading file with content-type:multipart/form-data.

### Extension methods
void IncludeAllXmlComments(this SwaggerGenOptions options, bool includeControllerXmlComments = false)

void IncludeXmlComments(this SwaggerGenOptions options, Assembly assembly, bool includeControllerXmlComments = false)

void AddJwtBearerSecurity(this SwaggerGenOptions options)

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
[HttpPost("upload")]
[ProducesResponseType(typeof(FileDto), 200)]
[FileUploadOperationFilter.FileContentType]
public async Task<IActionResult> Upload()
{
    var fileDto = await UploadMultipart();
    return Ok(fileDto);
}
```

