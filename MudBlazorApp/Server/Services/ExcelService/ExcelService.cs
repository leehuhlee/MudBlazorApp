using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace MudBlazorApp.Server.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        public ExcelPackage CreateExcelPackage<TData>(List<TData> dataList, 
            Dictionary<string, Func<TData, object>> mappers, 
            string sheet = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = sheet;
            package.Workbook.Properties.Author = "Kehag";
            package.Workbook.Properties.Subject = sheet;
            package.Workbook.Properties.Keywords = sheet;

            var worksheet = package.Workbook.Worksheets.Add(sheet);
            worksheet.Name = sheet;
            worksheet.Cells.Style.Font.Size = 11;
            worksheet.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = worksheet.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    worksheet.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = worksheet.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            return package;
        }
    }
}
