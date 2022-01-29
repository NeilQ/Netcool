using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using Netcool.Excel;
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
}
