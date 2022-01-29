using ClosedXML.Excel;

namespace Netcool.Excel;

public class ExcelStyleOptions
{
    public static readonly ExcelStyleOptions Default = new();

    public string FontFamily { get; set; } = "Microsoft YaHei";

    public double ValueFontSize { get; set; } = 11;

    public double TitleFontSize { get; set; } = 16;
    public XLAlignmentHorizontalValues TitleHorizontalAlignment { get; set; } = XLAlignmentHorizontalValues.Left;
    public XLColor TitleFontColor { get; set; } = XLColor.Black;
    public XLColor TitleBackgroundColor { get; set; } = XLColor.White;

    public double HeaderFontSize { get; set; } = 12;
    public XLColor HeaderFontColor { get; set; } = XLColor.White;
    public XLColor HeaderBackgroundColor { get; set; } = XLColor.FromArgb(64, 64, 64);
    public XLAlignmentHorizontalValues HeaderHorizontalAlignment { get; set; } = XLAlignmentHorizontalValues.Center;
}