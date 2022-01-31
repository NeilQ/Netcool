namespace Netcool.Excel.Attributes;

public class ExcelColumnHeaderAttribute : Attribute
{
    public string Name { get; set; }

    public ExcelColumnHeaderAttribute(string name)
    {
        Name = name;
    }
}
