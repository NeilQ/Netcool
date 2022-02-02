# Netcool.Excel
[![GitHub](https://img.shields.io/github/license/neilq/Netcool)](https://github.com/NeilQ/Netcool/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/Netcool.Excel)](https://www.nuget.org/packages/Netcool.Excel/)
![Nuget](https://img.shields.io/nuget/dt/Netcool.Excel)

Excel import and export extensions base on [ClosedXml](https://github.com/ClosedXML/ClosedXML).

## Usage

### Dynamic export
```c#
var wb = new ExcelExporter()
          .WithTitle("MyTitle")
          .WithSheetName("MySheet")
          .WithHeaders(new[] { "col1", "col2", "col3" })
          .WithRows(new List<List<object>>
          {
              new List<object>() { "r1c1", "1", DateTime.Now },
              new List<object>() { "r2c1", "2.2" },
              new List<object>() { "r3c1" }
          })
          .ExportAsWorkbook();
wb.SaveAs($"export_with_title.xlsx");
```

### Set styles
```c#
var styles = new ExcelStyleOptions()
{
    HeaderFontSize = 9,
    HeaderBackgroundColor = XLColor.White,
    HeaderFontColor = XLColor.Red
};
using var wb = new ExcelExporter()
    .WithStyles(styles)
    ...
    .ExportAsWorkbook();
```

### Response in controller 
```c#
 public IActionResult Export()
 {
     var wb = new ExcelExporter()
              ...
              .ExportAsWorkbook();
     var memoryStream = new MemoryStream();
     wb.SaveAs(memoryStream);
     wb.Dispose();
     memoryStream.Seek(0, SeekOrigin.Begin);
     return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
     {
         FileDownloadName = "filename.xslx"
     };
 }
```

### Typed object export
```c#
public class Student
 {
     public Student(string name, int age, string gender)
     {
         Name = name;
         Age = age;
         Gender = gender;
     }

     [ExcelColumnHeader("姓名")]
     [ExcelColumnOrder(3)]
     public string Name { get; set; }

     [ExcelColumnHeader("年龄")]
     [ExcelColumnOrder(1)]
     public int Age { get; set; }

     public string Gender { get; set; }


     [ExcelColumnIgnore]
     public string Ignore { get; set; }
 }
```

```c#
using var wb = new ExcelExporter<Student>()
    ...
    .WithRows(new List<Student>
    {
        new Student("John", 16,"Mail"),
        new Student("Lily", 17,"Female")
    })
    .ExportAsWorkbook();
wb.SaveAs("export_typed_objs.xlsx");
```

### Import
```c#
 var list = new ExcelImporter<Student>()
     .FromFile(memoryStream)
     // or .FromFile("students.xlsx")
     .Import();
```
