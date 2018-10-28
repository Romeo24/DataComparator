using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // для отримання розширення файлу
using System.Configuration; //для роботи з SQL-підключенням з App.config
using System.Data.SqlClient; //для роботи з MS SQL

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
            DataSet ds = RetrieveDataFromExcel(tbx_FilePath.Text, arrayOfTableNames, arrayOfXlQueries);

            WriteDataToSQL(ds);
            chbx_DataFromDC.Checked = false;
            chbx_SalesFromBAT.Checked = false;
            chbx_DebtFromBAT.Checked = false;
        }

        private static DataSet RetrieveDataFromExcel(string xlFilePath, string[] arrayOfTableNames, string[] arrayOfXlQueries)
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
                        //excelConnString = Properties.Settings.Default.excel97ConnectionString;
                        break;
                    case ".xlsx":
                        excelConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + xlFilePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                        break;
                }
                using (OleDbConnection oleDbConn = new OleDbConnection(excelConnString))
                {
                    string excelQuery = null;
                    OleDbCommand oleDbCmd;
                    OleDbDataAdapter odda;
                    DataTable dt = null;

                    oleDbConn.Open();
                    for (int i = 0; i < arrayOfTableNames.Count(); i++)
                    {
                        excelQuery = arrayOfXlQueries[i];
                        oleDbCmd = new OleDbCommand(excelQuery, oleDbConn);
                        odda = new OleDbDataAdapter(oleDbCmd);
                        dt = new DataTable();
                        dt.TableName = arrayOfTableNames[i];
                        odda.Fill(dt);
                        ds.Tables.Add(dt);
                    }
                    oleDbCmd = null;
                    oleDbConn.Close();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return ds;
        }

        private static void WriteDataToSQL(DataSet ds)
        {
            string sqlConnString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;
            string storeProc = null;
            string tableValueParam = null;
            try
            {
                for (int tableNameID = 0; tableNameID < ds.Tables.Count; tableNameID++)
                {
                    switch (ds.Tables[tableNameID].ToString())
                    {
                        case "Sales_from_DC":
                            storeProc = "insert_update_sales_from_dc";
                            tableValueParam = "SalesFromDC";
                            break;
                        case "Debt_KEG":
                            storeProc = "insert_update_debt_KEG_from_dc";
                            tableValueParam = "DebtKEGFromDC";
                            break;
                        case "Debt_MONEY":
                            storeProc = "insert_update_debt_MONEY_from_dc";
                            tableValueParam = "DebtMONEYFromDC";
                            break;
                        case "Sales_From_BAT":
                            storeProc = "insert_update_sales_from_bat";
                            tableValueParam = "SalesFromBAT";
                            break;
                        case "Debt_From_BAT":
                            storeProc = "insert_update_debt_from_bat";
                            tableValueParam = "DebtFromBAT";
                            break;
                    }
                    using (SqlConnection sqlConn = new SqlConnection(sqlConnString))
                    {
                        sqlConn.Open();
                        SqlCommand sqlCmd = new SqlCommand(storeProc, sqlConn);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue(tableValueParam, ds.Tables[tableNameID]);
                        sqlCmd.ExecuteNonQuery();
                        sqlConn.Close();
                    };
                }
                MessageBox.Show("Дані успішно імпортовано з Excel-файлу");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}
