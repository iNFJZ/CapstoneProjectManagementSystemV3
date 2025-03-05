using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
namespace Infrastructure.Services.CommonServices.ExcelService
{
    public interface IExcelService
    {
        IXLColumn setOptionsToColumn(IXLColumn column, string[] options);

        IXLWorksheet setWorkSheetIntroduction(IXLWorksheet worksheet , string notes);
    }
}
