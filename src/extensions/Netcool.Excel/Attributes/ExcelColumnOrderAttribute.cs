namespace Netcool.Excel.Attributes;

public class ExcelColumnOrderAttribute : Attribute
{
    /// <summary>
    /// Column index, start from 1.
    /// </summary>
    public int Order { get; set; }

    public ExcelColumnOrderAttribute(int order)
    {
        Order = order;
    }
}
