using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppCorrected.Models;

namespace ConsoleAppCorrected.Interfaces
{
    public interface IDataReader
    {
        /// <summary>
        /// Prints out information of database objects fetched from a CSV file
        /// </summary>
        /// <remarks>
        /// Uses a collection of <see cref="ImportedObject"/> objects to display the data
        /// Additionally, prints a <see cref="List{string}"/> containing row numbers that failed to load
        /// </remarks>
        /// <returns>void</returns>
        /// <param name="fileToImport"> input: path to a CSV file</param>
        void ImportAndPrintCsvData(string fileToImport);
    }
}
