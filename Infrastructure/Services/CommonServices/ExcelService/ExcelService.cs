using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.ExcelService
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ExcelService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IXLColumn setOptionsToColumn(IXLColumn column, string[] options)
        {
            int rowIndex = 1;
            foreach (var option in options)
            {
                var cell = column.Cell(rowIndex);
                cell.Value = option;
                var cellStyle = cell.Style;
                cellStyle.Font.FontColor = XLColor.White;
                cellStyle.Fill.BackgroundColor = XLColor.White;
                var border = cell.Style.Border;
                border.OutsideBorder = XLBorderStyleValues.Thin;
                border.OutsideBorderColor = XLColor.Gray;
                rowIndex++;
            }
            return column;
        }

        public IXLWorksheet setWorkSheetIntroduction(IXLWorksheet worksheet, string notes)
        {
            IXLCell cellNote = worksheet.Cell("A8");
            cellNote.Value = notes;
            cellNote.Style.Font.Bold = true;
            return worksheet;
        }
    }
}
