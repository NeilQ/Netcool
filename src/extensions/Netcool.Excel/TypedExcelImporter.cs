using System.Globalization;
using System.Reflection;
using ClosedXML.Excel;

namespace Netcool.Excel;

public class ExcelImporter<T> where T : class, new()
{
    private IList<ExcelColumnMetadata> _columns;
    private string _sheetName;
    private XLWorkbook _workbook;
    private int _headerRowIndex;

    public ExcelImporter()
    {
        var objType = typeof(T);
        var columnMetadataList = new List<ExcelColumnMetadata>();

        var properties = GetPublicProperties(objType);
        if (properties.Count == 0) return;
        foreach (var propertyInfo in properties)
        {
            columnMetadataList.Add(ExcelColumnMetadata.ForPropertyInfo(propertyInfo));
        }

        _columns = columnMetadataList.Where(t => !t.IgnoreColumn).ToList();
    }

    public ExcelImporter<T> FromFile(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new ArgumentException("File path cannot be null.");
        _workbook = new XLWorkbook(path, XLEventTracking.Disabled);
        return this;
    }

    public ExcelImporter<T> FromFile(Stream stream)
    {
        _workbook = new XLWorkbook(stream, XLEventTracking.Disabled);
        return this;
    }

    /// <summary>
    /// Specify the sheet name workbook, if null then reading the first worksheet.
    /// </summary>
    /// <param name="sheetName"></param>
    public ExcelImporter<T> WithSheetName(string sheetName)
    {
        _sheetName = sheetName;
        return this;
    }

    /// <summary>
    /// Specify the header row index, starts from 1, if null then reading the first row.
    /// </summary>
    /// <param name="index"></param>
    public ExcelImporter<T> WithHeaderRowIndex(int index = 1)
    {
        _headerRowIndex = index;
        return this;
    }

    private List<PropertyInfo> GetPublicProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
    }

    public List<T> Import()
    {
        if (_workbook == null) throw new ApplicationException("Workbook did not loaded.");
        IXLWorksheet ws;
        if (string.IsNullOrEmpty(_sheetName))
        {
            ws = _workbook.Worksheet(1);
            if (ws == null) throw new ArgumentException("Cannot read the worksheet at position 1.");
        }
        else
        {
            ws = _workbook.Worksheet(_sheetName);
            if (ws == null) throw new ArgumentException($"Cannot read the worksheet with name {_sheetName}.");
        }

        if (_headerRowIndex <= 0) _headerRowIndex = 1;
        var headerRow = ws.Row(_headerRowIndex);
        if (headerRow == null) throw new ArgumentException($"Cannot read the header row with index {_headerRowIndex}.");
        var headerCells = headerRow.CellsUsed();
        if (headerCells == null || !headerCells.Any())
            throw new ArgumentException($"Cannot read header cells with row index {_headerRowIndex}.");

        var columnDict = _columns.ToDictionary(t => t.HeaderName, t => t);
        foreach (var headerCell in headerCells)
        {
            var columnIndex = headerCell.WorksheetColumn().ColumnNumber();
            var headName = headerCell.GetString();
            if (columnDict.TryGetValue(headName, out var metadata))
            {
                metadata.Index = columnIndex;
            }
        }

        var dataRows = ws.RowsUsed(t => t.RowNumber() > headerRow.RowNumber());
        if (dataRows == null || !dataRows.Any()) return null;

        var list = new List<T>();
        foreach (var row in dataRows)
        {
            var t = new T();
            foreach (var metadata in _columns)
            {
                if (metadata.Index <= 0) continue;
                var cellStr = row.Cell(metadata.Index).GetString();
                var value = MapPropertyValue(t, metadata.PropertyInfo, cellStr);
                if (value != null) metadata.PropertyInfo.SetValue(t, value);
            }

            list.Add(t);
        }

        _workbook.Dispose();

        return list;
    }

    private object MapPropertyValue(T v, PropertyInfo pi, string cellString)
    {
        if (string.IsNullOrEmpty(cellString)) return null;
        object newValue;
        if (pi.PropertyType == typeof(Guid))
        {
            newValue = Guid.Parse(cellString);
        }
        else if (pi.PropertyType == typeof(DateTime))
        {
            if (DateTime.TryParse(cellString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempValue))
                newValue = tempValue;
            else if (DateTime.TryParseExact(cellString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                         out var tempValue2))
                newValue = tempValue2;
            else
                throw new InvalidCastException($"{cellString} can't cast to datetime");
        }
        else if (pi.PropertyType == typeof(bool))
        {
            if (cellString == "1")
                newValue = true;
            else if (cellString == "0")
                newValue = false;
            else
                newValue = bool.Parse(cellString);
        }
        else if (pi.PropertyType == typeof(string))
        {
            newValue = cellString;
        }
        else if (pi.PropertyType.IsEnum)
        {
            newValue = Enum.Parse(pi.PropertyType, cellString, true);
        }
        else
        {
            newValue = Convert.ChangeType(cellString, pi.PropertyType);
        }

        pi.SetValue(v, newValue);
        return newValue;
    }
}
