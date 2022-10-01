using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteWealth.Framework.CSVTool;

namespace CsvTransform
{
	public class CsvOutputClass
	{
        //Output file properties
        //*Tip: check CsvInputClass.cs

        [CSVColumnAttributeClass("FirstNames", false)]
        public string FirstNames { get; set; }
        [CSVColumnAttributeClass("SurnameOrCompanyName", false)]
        public string SurnameOrCompanyName { get; set; }
        [CSVColumnAttributeClass("IdNumber", false)]
        public string IdNumber { get; set; }
        [CSVColumnAttributeClass("ReferenceNumber", false)]
        public string ReferenceNumber { get; set; }
        [CSVColumnAttributeClass("Product", false)]
        public string Product { get; set; }
        [CSVColumnAttributeClass("Fund", false)]
        public string Fund { get; set; }
        [CSVColumnAttributeClass("DateApplicable", false)]
        public string DateApplicable { get; set; }
        [CSVColumnAttributeClass("Amount", false)]
        public string Amount { get; set; }
        [CSVColumnAttributeClass("Currency", false)]
        public string Currency { get; set; }
    }
}
