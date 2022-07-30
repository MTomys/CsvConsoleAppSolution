using ConsoleAppCorrected.Interfaces;
using ConsoleAppCorrected.Models;
using CsvHelper;
using System.Globalization;
using ConsoleAppCorrected.CsvHelperMappings;
using CsvHelper.Configuration;

namespace ConsoleAppCorrected.Impl
{
    public class DataReader : IDataReader
    {
        private IEnumerable<ImportedObject> _importedObjects;
        private List<string> _failedRows;

        public IEnumerable<ImportedObject> ImportedObjects { get => new List<ImportedObject>(_importedObjects); }
        public List<string> FailedRows { get => new(_failedRows); }

        public void ImportAndPrintCsvData(string fileToImport)
        {
            if (!File.Exists(fileToImport))
            {
                throw new ArgumentException($"Could not find file: {fileToImport}");
            }

            var rawImportedObjects = GetRawImportedObjectsFromCsv(fileToImport);

            // clear and correct imported data
            var importedObjects = rawImportedObjects.Select(obj => ClearAndCorrectObject(obj)).ToList();

            // assign number of children
            importedObjects.ForEach(importedObject =>
               importedObject.NumberOfChildren = GetNumberOfChildren(importedObject, importedObjects));

            // print databases details
            PrintDatabaseDetails(importedObjects);

            PrintFailedRows();
        }

        private List<ImportedObject> GetRawImportedObjectsFromCsv(string fileToImport)
        {
            _failedRows = new List<string>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",

                MissingFieldFound = (missingFieldFoundArgs) =>
                {
                    _failedRows.Add($"Failed row found, row number: {missingFieldFoundArgs.Context.Parser.RawRow} Reason: " +
                        $"Invalid number of fields in row");
                }
            };

            var importedObjects = new List<ImportedObject>();
            using (var streamReader = new StreamReader(fileToImport))
            {
                using var csvReader = new CsvReader(streamReader, config);

                csvReader.Context.RegisterClassMap<ImportedObjectClassMap>();
                importedObjects = csvReader.GetRecords<ImportedObject>().ToList();
            }

            return importedObjects;
        }

        private static ImportedObject ClearAndCorrectObject(ImportedObject importedObject)
        {
            return new ImportedObject()
            {
                Type = SanitizeImportedObjectValue(importedObject.Type).ToUpper(),
                Name = SanitizeImportedObjectValue(importedObject.Name),
                Schema = SanitizeImportedObjectValue(importedObject.Schema),
                ParentName = SanitizeImportedObjectValue(importedObject.ParentName),
                DataType = importedObject.DataType,
                ParentType = SanitizeImportedObjectValue(importedObject.ParentType),
                IsNullable = importedObject.IsNullable,
            };
        }

        private static int GetNumberOfChildren(ImportedObject currentImportedObject, List<ImportedObject> importedObjects)
        {
            int numberOfChildren = 0;

            foreach (var importedObject in importedObjects)
            {
                if (importedObject.ParentType == currentImportedObject.Type)
                {
                    if (importedObject.ParentName == currentImportedObject.Name)
                    {
                        numberOfChildren++;
                    }
                }
            }

            return numberOfChildren;
        }

        private static void PrintDatabaseDetails(List<ImportedObject> importedObjects)
        {
            var databases = importedObjects.Where(impObj => impObj.Type == "DATABASE").ToList();

            foreach (var database in databases)
            {
                Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");
                var tables = importedObjects
                    .Where(table => (table.ParentType.ToUpper() == database.Type) && (table.ParentName == database.Name)).ToList();

                foreach (var table in tables)
                {
                    Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");
                    var columns = importedObjects
                        .Where(column => (column.ParentType.ToUpper() == table.Type) && (column.ParentName == table.Name)).ToList();

                    columns.ForEach(column =>
                        Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}"));
                }
            }
        }

        private static string SanitizeImportedObjectValue(string value)
        {
            return value.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
        }

        private void PrintFailedRows()
        {
            Console.WriteLine("Failed rows: ");
            _failedRows.ForEach(failedRow => Console.WriteLine(failedRow));
        }
    }
}
