using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteWealth.Framework.CSVTool;

namespace CsvTransform
{
	public partial class frmCsvTransform : Form
	{
		public frmCsvTransform()
		{
			InitializeComponent();
		}

		private void btnTransform_Click(object sender, EventArgs e)
		{
			OpenFileDialog fdlg = new OpenFileDialog();
			fdlg.Title = "Select file";
			fdlg.InitialDirectory = @"C:\JobInterview\CodeTest\CsvTransform\Resources";
			fdlg.Filter = "CSV files (*.csv)|*.csv";
			fdlg.FilterIndex = 1;
			fdlg.RestoreDirectory = true;
			if (fdlg.ShowDialog() == DialogResult.OK)
			{
				try
				{
					TransformCsvFile(fdlg.FileName);
				}
				catch (IOException ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		private void TransformCsvFile(string filePathAndName)
		{
			byte[] sourceFileContent = File.ReadAllBytes(filePathAndName);
			CSVFileClass csvFile = CSVEngine.CSVFileLoad(sourceFileContent);

			if (!CSVEngine.CSVFileValidate2<CsvInputClass>(csvFile, out Dictionary<string, int> mapCSVColumnNameToColumnIndex, out string validationMessage))
				return;

			int lineNumber = 2;
			List<CsvInputClass> csvInputLines = new List<CsvInputClass>(csvFile.Content.Count);

			foreach (List<string> csvContentLine in csvFile.Content)
			{
				CsvInputClass csvInputLine = Activator.CreateInstance(typeof(CsvInputClass), lineNumber, csvFile.HeaderLine, csvContentLine, mapCSVColumnNameToColumnIndex) as CsvInputClass;
				csvInputLines.Add(csvInputLine);
			}

			//Method to return transformed list csvOutputLines
			//**
			List<CsvOutputClass> csvOutputLines = TransformCsvToOutputFile(csvInputLines);

			CSVFileClass outCsvFile = CSVEngine.CreateCsvFile(csvOutputLines);
			File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(filePathAndName), "OutputFile.csv"), CSVEngine.ToByteArray(outCsvFile));
		}

		private static List<CsvOutputClass> TransformCsvToOutputFile(List<CsvInputClass> csvInputLines)
		{
			List<CsvOutputClass> csvOutputLines = new List<CsvOutputClass>();
			bool shouldAdd = true;
            CsvOutputClass PreCalcCSVLine = new CsvOutputClass()
            {
                FirstNames = csvInputLines[0].Name,
                SurnameOrCompanyName = csvInputLines[0].Surname,
                IdNumber = csvInputLines[0].ID,
				Fund = csvInputLines[0].Fund,
                ReferenceNumber = csvInputLines[0].ContractNumber,
                Product = csvInputLines[0].Product,
                DateApplicable = (DateTime.Parse(csvInputLines[0].TxDate)).ToString("dd/MM/yyyy"),
                Amount = "0",
                Currency = csvInputLines[0].TxCurrency
            };
			decimal AmountCalc = 0;
            foreach (CsvInputClass csvInputLine in csvInputLines)
			{
				if (csvInputLine.ID == PreCalcCSVLine.IdNumber && shouldAdd == true)
				{
					if (PreCalcCSVLine.SurnameOrCompanyName == "")
					{
						PreCalcCSVLine.SurnameOrCompanyName = csvInputLine.CompanyName;
					}
					if (csvInputLine.TxType == "Investment")
					{
						PreCalcCSVLine.Amount = csvInputLine.TxAmount;
						AmountCalc = decimal.Parse(PreCalcCSVLine.Amount);
						csvOutputLines.Add(PreCalcCSVLine);
						shouldAdd = false;
						
					}
					else if (GetAdditionSubtraction(csvInputLine.TxType))
				    {
                        AmountCalc += decimal.Parse(csvInputLine.TxAmount);
                    }
					else
					{
                        AmountCalc -= decimal.Parse(csvInputLine.TxAmount);
                    }
					PreCalcCSVLine.Amount = AmountCalc.ToString();
                    continue;
                } 
				if (shouldAdd == true)
				{
					csvOutputLines.Add(PreCalcCSVLine);
				}
				if (PreCalcCSVLine.IdNumber != csvInputLine.ID)
				{
					shouldAdd = true;
					PreCalcCSVLine = new CsvOutputClass()
					{
						FirstNames = csvInputLine.Name,
						SurnameOrCompanyName = csvInputLine.Surname,
						IdNumber = csvInputLine.ID,
						ReferenceNumber = csvInputLine.ContractNumber,
						Product = csvInputLine.Product,
						Fund = csvInputLine.Fund,
						DateApplicable = DateTime.Parse(csvInputLine.TxDate).ToString("dd/MM/yyyy"),
						Amount = csvInputLine.TxAmount,
						Currency = csvInputLine.TxCurrency
					};
					if (GetAdditionSubtraction(csvInputLine.TxType))
					{
						AmountCalc = decimal.Parse(csvInputLine.TxAmount);
					}
					else
					{
						AmountCalc = 0 - decimal.Parse(csvInputLine.TxAmount);
                    }
					if (csvInputLine.TxType == "Investment")
					{
						if (PreCalcCSVLine.SurnameOrCompanyName == "")
						{
							PreCalcCSVLine.SurnameOrCompanyName = csvInputLine.CompanyName;
						}
						shouldAdd = false;
						csvOutputLines.Add(PreCalcCSVLine);
					}
				}
            }
            csvOutputLines.Add(PreCalcCSVLine);
            return csvOutputLines;
		}

		private static bool GetAdditionSubtraction (string Type)
		{
            switch (Type)
            {                
                case "Net Investment":
					return true;              
                case "Initial Fee":
					return true;                 
                case "Correction":
					return false;                    
				default:
					return true;
            }
        }
	}
}
