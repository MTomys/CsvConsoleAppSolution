using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCorrected.Interfaces
{
    public interface IDataReader
    {
        void ImportAndPrintCsvData(string fileToImport);
    }
}
