using System.Reflection;
using ClosedXML.Excel;
using Netcool.Excel.Attributes;

namespace Netcool.Excel;

public class ExcelExporter<T> where T : class
{
    private ExcelStyleOptions _styleOptions;
    private IList<ExcelColumnMetadata> _columns;
    private IEnumerable<T> _rows;
    private string _title;
    private string _sheetName;

    public ExcelExporter<T> WithStyles(ExcelStyleOptions styleOptions)
    {
        _styleOptions = styleOptions;
        return this;
    }


    public ExcelExporter<T> WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ExcelExporter<T> WithSheetName(string sheetName)
    {
        _sheetName = sheetName;
        return this;
    }

    private List<PropertyInfo> GetPublicProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
    }

    public static IEnumerable<Attribute> GetCustomAttributes(MemberInfo memberInfo)
    {
        var attrs = memberInfo.GetCustomAttributes();
        return attrs;
    }

    public static IEnumerable<Attribute> GetCustomAttributes(MemberInfo memberInfo, Type type)
    {
        var attrs = memberInfo.GetCustomAttributes(type);
        return attrs;
    }

    public class ExcelColumnMetadata
    {
        public string PropertyName { get; internal set; }

        public string HeaderName { get; internal set; }

        public int Order { get; internal set; }

        public int Index { get; internal set; }

        public bool IgnoreColumn { get; internal set; }

        public PropertyInfo PropertyInfo { get; internal set; }

        public static ExcelColumnMetadata ForMemberInfo(PropertyInfo pi)
        {
            var ci = new ExcelColumnMetadata { PropertyInfo = pi };
            var attrs = GetCustomAttributes(pi).ToArray();

            ci.PropertyName = pi.Name;
            var ignoreAttrs = attrs.OfType<ExcelColumnIgnoreAttribute>().ToArray();
            if (ignoreAttrs.Any()) ci.IgnoreColumn = true;

            var headerAttrs = attrs.OfType<ExcelColumnHeaderAttribute>().ToArray();
            ci.HeaderName = headerAttrs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name))?.Name ?? pi.Name;

            var orderAttrs = attrs.OfType<ExcelColumnOrderAttribute>().ToArray();

            ci.Order = orderAttrs.FirstOrDefault()?.Order ?? int.MaxValue;
            return ci;
        }
    }

    public ExcelExporter<T> WithRows(IEnumerable<T> rows)
    {
        _rows = rows;

        var objType = typeof(T);
        var properties = GetPublicProperties(objType);
        if (properties.Count == 0) return this;
        var columnMetadataList = new List<ExcelColumnMetadata>();
        foreach (var propertyInfo in properties)
        {
            columnMetadataList.Add(ExcelColumnMetadata.ForMemberInfo(propertyInfo));
        }

        columnMetadataList.Sort((x, y) => x.Order.CompareTo(y.Order));
        var columnIndex = 1;
        for (var i = 0; i < columnMetadataList.Count; i++)
        {
            if (columnMetadataList[i].IgnoreColumn) continue;
            columnMetadataList[i].Index = columnIndex;
            columnIndex++;
        }

        _columns = columnMetadataList;

        return this;
    }

    public IXLWorkbook ExportAsWorkbook()
    {
        if (_columns == null || !_columns.Any()) throw new ArgumentException("Export columns cannot be empty.");
        var wb = new XLWorkbook(XLEventTracking.Disabled);
        var ws = wb.Worksheets.Add(_sheetName ?? "Sheet1");
        _styleOptions ??= ExcelStyleOptions.Default;

        var rowNumber = 1;

        var exportColumnCount = _columns.Count(t => !t.IgnoreColumn);

        // title
        if (!string.IsNullOrEmpty(_title))
        {
            ws.Cell("A1").Value = _title;
            var rngTitle = ws.Range(ws.Row(rowNumber).Cell(1), ws.Row(rowNumber).Cell(exportColumnCount));
            rngTitle.Merge();
            rngTitle.Style.Font.Bold = true;
            rngTitle.Style.Font.FontName = _styleOptions.FontFamily;
            if (_styleOptions.TitleFontSize > 0) rngTitle.Style.Font.FontSize = _styleOptions.TitleFontSize;
            rngTitle.Style.Alignment.Horizontal = _styleOptions.TitleHorizontalAlignment;
            rngTitle.Style.Font.FontColor = _styleOptions.TitleFontColor;
            rngTitle.Style.Fill.BackgroundColor = _styleOptions.TitleBackgroundColor;
            rowNumber++;
        }


        // header
        for (var i = 0; i < _columns.Count; i++)
        {
            var metadata = _columns[i];
            if (metadata.IgnoreColumn) continue;

            ws.Row(rowNumber).Cell(i + 1).Value = metadata.HeaderName;
        }

        var rngHeaders = ws.Range(ws.Row(rowNumber).Cell(1), ws.Row(rowNumber).Cell(exportColumnCount));
        rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rngHeaders.Style.Font.Bold = true;
        rngHeaders.Style.Font.FontName = _styleOptions.FontFamily;
        if (_styleOptions.HeaderFontSize > 0) rngHeaders.Style.Font.FontSize = _styleOptions.HeaderFontSize;
        rngHeaders.Style.Font.FontColor = _styleOptions.HeaderFontColor;
        rngHeaders.Style.Fill.BackgroundColor = _styleOptions.HeaderBackgroundColor;

        rowNumber++;

        // values
        if (_rows != null && _rows.Any())
        {
            for (var i = 0; i < _rows.Count(); i++)
            {
                var row = _rows.ElementAt(i);
                if (row == null) continue;

                for (var j = 0; j < _columns.Count; j++)
                {
                    var metadata = _columns[j];
                    if (metadata.IgnoreColumn) continue;

                    var cell = ws.Row(rowNumber).Cell(metadata.Index);
                    var propertyValue = metadata.PropertyInfo.GetValue(row);
                    if (propertyValue != null)
                    {
                        if (_styleOptions.ValueFontSize > 0) cell.Style.Font.FontSize = _styleOptions.ValueFontSize;
                        cell.Style.Font.FontName = _styleOptions.FontFamily;
                        cell.Value = metadata.PropertyInfo.GetValue(row) ?? "";
                    }
                }

                rowNumber++;
            }
        }

        ws.Columns().AdjustToContents();


        return wb;
    }
}
