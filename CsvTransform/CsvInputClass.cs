using System;
using System.Collections.Generic;
using EliteWealth.Framework.CSVTool;

namespace CsvTransform
{
	public class CsvInputClass : CSVBaseClass
	{
		#region Constructors
		public CsvInputClass(int lineNumber, List<string> headerLines, List<string> contentLines, Dictionary<string, int> csvFieldLToColumnIndexMap)
			: base(lineNumber, headerLines, contentLines, csvFieldLToColumnIndexMap)
		{
            base.IntialiseProperties();
        }
        #endregion Constructors

        #region Properties
        [CSVColumnAttributeClass("Name", false)]
        public string Name { get; set; }
        [CSVColumnAttributeClass("Surname", false)]
        public string Surname { get; set; }
        [CSVColumnAttributeClass("CompanyName", false)]
        public string CompanyName { get; set; }
        [CSVColumnAttributeClass("ID", false)]
        public string ID { get; set; }
        [CSVColumnAttributeClass("ContractNumber", false)]
        public string ContractNumber { get; set; }
        [CSVColumnAttributeClass("Product", false)]
        public string Product { get; set; }
        [CSVColumnAttributeClass("Fund", false)]
        public string Fund { get; set; }
        [CSVColumnAttributeClass("TxNumber", false)]
        public string TxNumber { get; set; }
        [CSVColumnAttributeClass("TxType", false)]
        public string TxType { get; set; }
        [CSVColumnAttributeClass("TxDate", false)]
        public string TxDate { get; set; }
        [CSVColumnAttributeClass("TxAmount", false)]
        public string TxAmount { get; set; }
        [CSVColumnAttributeClass("TxCurrency", false)]
        public string TxCurrency { get; set; }

        #endregion Properties
    }
}
