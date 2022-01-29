namespace Netcool.Excel.Attributes;

public class ExcelHeaderAttribute : Attribute
{
    public string Name { get; set; }

    public ExcelHeaderAttribute(string name)
    {
        Name = name;
    }
}
