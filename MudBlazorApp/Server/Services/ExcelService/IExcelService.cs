using OfficeOpenXml;

namespace MudBlazorApp.Server.Services.ExcelService
{
    public interface IExcelService
    {
        ExcelPackage CreateExcelPackage<TData>(List<TData> dataList,
            Dictionary<string, Func<TData, object>> mappers, 
            string sheet);
    }
}
