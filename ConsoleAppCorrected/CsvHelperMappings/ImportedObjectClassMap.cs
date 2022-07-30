using ConsoleAppCorrected.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCorrected.CsvHelperMappings
{
    public class ImportedObjectClassMap : ClassMap<ImportedObject>
    {
        public ImportedObjectClassMap()
        {
            Map(importedObject => importedObject.Type).Name("Type");
            Map(importedObject => importedObject.Name).Name("Name");
            Map(importedObject => importedObject.Schema).Name("Schema");
            Map(importedObject => importedObject.ParentName).Name("ParentName");
            Map(importedObject => importedObject.ParentType).Name("ParentType");
            Map(importedObject => importedObject.DataType).Name("DataType");
            Map(importedObject => importedObject.IsNullable).Name("IsNullable");
        }
    }
}
