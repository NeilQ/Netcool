using ClosedXML.Excel;

namespace Netcool.Excel;

public class ExcelExporter
{
    private ExcelStyleOptions _styleOptions;
    private IEnumerable<string> _headers;
    private IEnumerable<IEnumerable<object>> _rows;
    private string _title;
    private string _sheetName;

    public ExcelExporter WithStyles(ExcelStyleOptions styleOptions)
    {
        _styleOptions = styleOptions;
        return this;
    }

    public ExcelExporter WithHeaders(IEnumerable<string> headers)
    {
        _headers = headers;
        return this;
    }

    public ExcelExporter WithRows(IEnumerable<IEnumerable<object>> rows)
    {
        _rows = rows;
        return this;
    }

    public ExcelExporter WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ExcelExporter WithSheetName(string sheetName)
    {
        _sheetName = sheetName;
        return this;
    }

    public IXLWorkbook ExportAsWorkbook()
    {
        if (_headers == null || !_headers.Any()) throw new ArgumentException("Export headers cannot be empty.");
        var wb = new XLWorkbook(XLEventTracking.Disabled);
        var ws = wb.Worksheets.Add(_sheetName ?? "Sheet1");
        _styleOptions ??= ExcelStyleOptions.Default;

        var rowNumber = 1;

        // title
        if (!string.IsNullOrEmpty(_title))
        {
            ws.Cell("A1").Value = _title;
            var rngTitle = ws.Range(ws.Row(rowNumber).Cell(1), ws.Row(rowNumber).Cell(_headers.Count()));
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
        for (var i = 0; i < _headers.Count(); i++)
        {
            ws.Row(rowNumber).Cell(i + 1).Value = _headers.ElementAt(i);
        }

        var rngHeaders = ws.Range(ws.Row(rowNumber).Cell(1), ws.Row(rowNumber).Cell(_headers.Count()));
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
                if (row == null || !row.Any()) continue;
                for (var j = 0; j < row.Count(); j++)
                {
                    var value = row.ElementAt(j);
                    var cell = ws.Row(rowNumber).Cell(j + 1);

                    if (value is string && decimal.TryParse(value.ToString(), out _))
                    {
                        value = "'" + value;
                    }

                    cell.Value = value;
                    if (_styleOptions.ValueFontSize > 0) cell.Style.Font.FontSize = _styleOptions.ValueFontSize;
                    cell.Style.Font.FontName = _styleOptions.FontFamily;
                }

                rowNumber++;
            }
        }

        ws.Columns().AdjustToContents();

        return wb;
    }
}
