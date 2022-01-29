namespace Netcool.Excel.Attributes;

public class ExcelColumnWidthAttribute : Attribute
{
    public int Width { get; set; }

    public ExcelColumnWidthAttribute(int width)
    {
        Width = width;
    }
}
public class ExcelColumnIndexAttribute : Attribute
{
    /// <summary>
    /// Column index, start from 1.
    /// </summary>
    public int Index { get; set; }

    public ExcelColumnIndexAttribute(int index)
    {
        Index = index;
    }
}
