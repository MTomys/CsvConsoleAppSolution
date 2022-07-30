using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCorrected.Models
{
    /// <summary>
    /// This class represents an object in the SQL database model
    /// </summary>
    public class ImportedObject : ImportedObjectBase
    {
        public string Schema { get; set; }
        public string ParentName { get; set; }
        public string ParentType { get; set; }
        public string DataType { get; set; }
        public string IsNullable { get; set; }
        /// <summary>
        /// This describes the number of children an object has. 
        /// Example (Table has 2 columns, NumberOfChildren: 2)
        /// </summary>
        public int NumberOfChildren { get; set; }
    }
}
