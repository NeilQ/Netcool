using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Netcool.Excel;
using Netcool.Excel.Attributes;
using NUnit.Framework;

namespace Netcool.Tests;

public class ExcelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ExportToWorkbookWithTitle()
    {
        using var wb = new ExcelExporter()
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
        Assert.Pass();
    }

    [Test]
    public void ExportToWorkbookWithoutTitle()
    {
        using var wb = new ExcelExporter()
            .WithSheetName("MySheet")
            .WithHeaders(new[] { "col1", "col2", "col3" })
            .WithRows(new List<List<object>>
            {
                new List<object>() { "r1c1", "1", DateTime.Now },
                new List<object>() { "r2c1", "2.2" },
                new List<object>() { "r3c1" }
            })
            .ExportAsWorkbook();
        wb.SaveAs($"export_without_title.xlsx");
        Assert.Pass();
    }

    [Test]
    public void ExportToWorkbookWithStyle()
    {
        var styles = new ExcelStyleOptions()
        {
            HeaderFontSize = 9,
            HeaderBackgroundColor = XLColor.White,
            HeaderFontColor = XLColor.Red
        };
        using var wb = new ExcelExporter()
            .WithStyles(styles)
            .WithHeaders(new[] { "col1", "col2", "col3" })
            .WithRows(new List<List<object>>
            {
                new List<object>() { "r1c1", "1", DateTime.Now },
                new List<object>() { "r2c1", "2.2" },
                new List<object>() { "r3c1" }
            })
            .ExportAsWorkbook();
        wb.SaveAs($"export_with_styles.xlsx");
    }

    [Test]
    public void ExportToWorkbookWithTypedRows()
    {
        using var wb = new ExcelExporter<Student>()
            .WithRows(new List<Student>
            {
                new Student("John", 16, "Male", GenderE.Male),
                new Student("Lily", 17, "Female", GenderE.Female)
            })
            .ExportAsWorkbook();
        wb.SaveAs("export_typed_objs.xlsx");
    }

    [Test]
    public void ImportTest()
    {
        using var wb = new ExcelExporter<Student>()
            .WithRows(new List<Student>
            {
                new Student("John", 16, "Male", GenderE.Male),
                new Student("Lily", 17, "Female", GenderE.Female)
            })
            .ExportAsWorkbook();

        var memoryStream = new MemoryStream();
        wb.SaveAs(memoryStream);
        wb.Dispose();
        memoryStream.Seek(0, SeekOrigin.Begin);

        var list = new ExcelImporter<Student>()
            .FromFile(memoryStream)
            .Import();

        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("John", list[0].Name);
        Assert.AreEqual("Lily", list[1].Name);
        Assert.AreEqual(16, list[0].Age);
        Assert.AreEqual(17, list[1].Age);
        Assert.AreEqual(GenderE.Male,list[0].GenderE);
        Assert.AreEqual(GenderE.Female,list[1].GenderE);
    }

    public class Student
    {
        public Student()
        {
        }

        public Student(string name, int age, string gender, GenderE genderE)
        {
            Name = name;
            Age = age;
            Gender = gender;
            GenderE = genderE;
        }

        [ExcelColumnHeader("姓名")]
        [ExcelColumnOrder(3)]
        public string? Name { get; set; }

        [ExcelColumnHeader("年龄")]
        [ExcelColumnOrder(1)]
        public int Age { get; set; }

        public string? Gender { get; set; }

        public GenderE GenderE { get; set; }

        public DateTime Birthday { get; set; } = DateTime.Now;

        [ExcelColumnIgnore]
        public string? Ignore { get; set; }
    }

    public enum GenderE
    {
        Female,
        Male
    }
}
