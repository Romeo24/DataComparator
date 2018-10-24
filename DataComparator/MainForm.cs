using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
//using System.Data.Odbc; // для роботи з ODBCConnection
using System.Drawing;
using System.IO; // для отримання розширення файлу
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataComparator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "C:\\";
                ofd.DefaultExt = "*.xls;*.xlsx";
                ofd.Title = "Вибір Excel-файлу для імпорту";
                ofd.Filter = "Excel 97-2003 (*.xls)|*.xls|Excel 2007-2010 (*.xlsx)|*.xlsx";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                    tbx_FilePath.Text = ofd.FileName;
            };
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            string[] arrayOfXlQueries = null;
            List<string> listOfXlQueries = new List<string>();

            string[] arrayOfTableNames = null;
            List<string> listOfTableNames = new List<string>();

            if (chbx_DataFromDC.Checked)
            {
                listOfXlQueries.Add("Select * From [DataFromDC$C7:N11]");
                listOfXlQueries.Add("Select * From [DataFromDC$C13:G14]");
                listOfXlQueries.Add("Select * From [DataFromDC$C16:H17]");
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Sales_from_DC");
                listOfTableNames.Add("Debt_KEG");
                listOfTableNames.Add("Debt_MONEY");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            else if (chbx_SalesFromBAT.Checked)
            {
                listOfXlQueries.Add("Select * From [SalesFromBAT$B1:M57]");
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Sales_From_BAT");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            else if (chbx_DebtFromBAT.Checked)
            {
                listOfXlQueries.Add("Select * From [DebtFromBAT$B1:I15]");
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Debt_From_BAT");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            GetDataFromExcel(tbx_FilePath.Text, arrayOfTableNames, arrayOfXlQueries);
        }

        private static DataSet GetDataFromExcel(string xlFilePath, string[] arrayOfTableNames, string[] arrayOfXlQueries)
        {
            DataSet ds = new DataSet();
            try
            {
                string fileExtension = Path.GetExtension(xlFilePath);
                string excelConnString = string.Empty;

                switch (fileExtension)
                {
                    case ".xls":
                        excelConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlFilePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                        break;
                    case ".xlsx":
                        excelConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + xlFilePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                        break;
                }

                #region Retreive data from Excel
                using (OleDbConnection oleDbConn = new OleDbConnection(excelConnString))
                {
                    string ExcelQuery1 = null;
                    string ExcelQuery2 = null;
                    string ExcelQuery3 = null;
                    OleDbCommand oleDbCmd1, oleDbCmd2, oleDbCmd3;
                    DataTable dt1, dt2, dt3;
                    OleDbDataAdapter odda1, odda2, odda3;
                    oleDbConn.Open();
                    if (arrayOfXlQueries.Count() == 1)
                    {
                        ExcelQuery1 = arrayOfXlQueries[0];
                        oleDbCmd1 = new OleDbCommand(ExcelQuery1, oleDbConn);
                        odda1 = new OleDbDataAdapter(oleDbCmd1);
                        dt1 = new DataTable();
                        dt1.TableName = arrayOfTableNames[0];
                        odda1.Fill(dt1);
                        ds.Tables.Add(dt1);
                    }
                    else if (arrayOfXlQueries.Count() == 3)
                    {
                        ExcelQuery1 = arrayOfXlQueries[0];
                        ExcelQuery2 = arrayOfXlQueries[1];
                        ExcelQuery3 = arrayOfXlQueries[2];
                        oleDbCmd1 = new OleDbCommand(ExcelQuery1, oleDbConn);
                        oleDbCmd2 = new OleDbCommand(ExcelQuery2, oleDbConn);
                        oleDbCmd3 = new OleDbCommand(ExcelQuery3, oleDbConn);

                        odda1 = new OleDbDataAdapter(oleDbCmd1);
                        odda2 = new OleDbDataAdapter(oleDbCmd2);
                        odda3 = new OleDbDataAdapter(oleDbCmd3);

                        dt1 = new DataTable();
                        dt1.TableName = arrayOfTableNames[0];
                        dt2 = new DataTable();
                        dt2.TableName = arrayOfTableNames[1];
                        dt3 = new DataTable();
                        dt3.TableName = arrayOfTableNames[2];

                        odda1.Fill(dt1);
                        odda2.Fill(dt2);
                        odda3.Fill(dt3);
                        ds.Tables.Add(dt1);
                        ds.Tables.Add(dt2);
                        ds.Tables.Add(dt3);
                    }
                    oleDbCmd1 = null;
                    oleDbCmd2 = null;
                    oleDbCmd3 = null;
                    oleDbConn.Close();
                };
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return ds;
        }
    }
}
