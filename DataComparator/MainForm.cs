﻿using System;
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

            string sqlConnString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(sqlConnString))
            {
                sqlConn.Open();
                SqlCommand sqlcmd = new SqlCommand("SELECT sync_point_no, enterprise_no, dc_name FROM dbo.distributors", sqlConn);
                using (SqlDataReader sqldr = sqlcmd.ExecuteReader())
                {
                    while (sqldr.Read())
                    {
                        cmbbx_dc_list.Items.Add(sqldr.GetInt32(0) + "/" + sqldr.GetInt32(1) + "/" + sqldr.GetString(2));
                        cmbbx_dc_list.BackColor = System.Drawing.Color.LightGray;
                    }
                };
                sqlConn.Close();
            };
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
                listOfXlQueries.Add("Select * From [DataFromDC$C7:N11]");//№1 - sales
                listOfXlQueries.Add("Select * From [DataFromDC$C13:N15]");//№2 - archived stocks
                listOfXlQueries.Add("Select * From [DataFromDC$C19:G20]");//№3 - debt KEG
                listOfXlQueries.Add("Select * From [DataFromDC$C22:H23]");//№4 - debt MONEY
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Sales_DC");
                listOfTableNames.Add("Archived_Stocks_DC");
                listOfTableNames.Add("Debt_KEG_DC");
                listOfTableNames.Add("Debt_MONEY_DC");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            else if (chbx_SalesFromBAT.Checked)
            {
                listOfXlQueries.Add("Select * From [SalesFromBAT$B1:M57]");
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Sales_BAT");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            else if (chbx_DebtFromBAT.Checked)
            {
                listOfXlQueries.Add("Select * From [DebtFromBAT$B1:I15]");
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Debt_BAT");
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            else if (chbx_ArchivedStockstFromBAT.Checked)
            {
                listOfXlQueries.Add("Select * From [ArchivedStocksBAT$B1:M27]");// діапазон залишків з файла з БАТ
                arrayOfXlQueries = listOfXlQueries.ToArray();

                listOfTableNames.Add("Archived_Stocks_BAT");// назва таблиці
                arrayOfTableNames = listOfTableNames.ToArray();
            }
            DataSet ds = RetrieveDataFromExcel(tbx_FilePath.Text, arrayOfTableNames, arrayOfXlQueries);

            WriteDataToSQL(ds);
            chbx_DataFromDC.Checked = false;
            chbx_SalesFromBAT.Checked = false;
            chbx_DebtFromBAT.Checked = false;
            chbx_ArchivedStockstFromBAT.Checked = false;
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
                        case "Sales_DC":
                            storeProc = "insert_update_sales_from_dc";
                            tableValueParam = "SalesFromDC";
                            break;
                        case "Archived_Stocks_DC":
                            storeProc = "insert_update_archived_stocks_from_dc";
                            tableValueParam = "ArchivedStocksFromDC";
                            break;
                        case "Debt_KEG_DC":
                            storeProc = "insert_update_debt_KEG_from_dc";
                            tableValueParam = "DebtKEGFromDC";
                            break;
                        case "Debt_MONEY_DC":
                            storeProc = "insert_update_debt_MONEY_from_dc";
                            tableValueParam = "DebtMONEYFromDC";
                            break;
                        case "Sales_BAT":
                            storeProc = "insert_update_sales_from_bat";
                            tableValueParam = "SalesFromBAT";
                            break;
                        case "Debt_BAT":
                            storeProc = "insert_update_debt_from_bat";
                            tableValueParam = "DebtFromBAT";
                            break;
                        case "Archived_Stocks_BAT":
                            storeProc = "insert_update_archived_stocks_from_BAT";
                            tableValueParam = "ArchivedStocksBAT";
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

        private void btn_Export_Click(object sender, EventArgs e)
        {
            //int spn = 0;
            //int enterpiseCode = 0;
            int debDate = Int32.Parse(dateTimePicker1.Value.Date.ToString("yyyyMMdd"));
            int reportPeriod = Int32.Parse(dateTimePicker1.Value.Date.ToString("yyyyMM"));
            int archStockDate = debDate;
            DataTable dt, dt1, dt2;
            SqlDataAdapter sqlda, sqlda1, sqlda2;
            string fullPath = string.Empty;
            string path = Directory.GetCurrentDirectory();


            //if (cmbbx_dc_list.SelectedIndex == -1) //нічого не вибрано, значить експортуємо всі ТС
            //{

            //}
            //else if (cmbbx_dc_list.SelectedIndex >= 0) //вибрана одна ТС для експорту
            //{
            //    string selectedItem = cmbbx_dc_list.Items[cmbbx_dc_list.SelectedIndex].ToString();
            //    string[] arrayWithDCData = selectedItem.Split('/');
            //    spn = Int32.Parse(arrayWithDCData[0]);
            //    enterpiseCode = Int32.Parse(arrayWithDCData[1]);
            //    debDate = Int32.Parse(dateTimePicker1.Value.Date.ToString("yyyyMMdd"));
            //}

            string sqlConnString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(sqlConnString))
            {
                DataSet ds = new DataSet();
                sqlConn.Open();
                using (SqlCommand sqlcmd = new SqlCommand("SELECT * FROM dbo.GetPivotTableDebt(@deb_date)", sqlConn))
                {
                    sqlcmd.Parameters.AddWithValue("@deb_date", debDate);
                    sqlda = new SqlDataAdapter(sqlcmd);
                    dt = new DataTable();
                    dt.TableName = "Debt";
                    sqlda.Fill(dt);
                    ds.Tables.Add(dt);
                };
                using (SqlCommand sqlcmd1 = new SqlCommand("SELECT * FROM dbo.GetPivotTableSales(@report_period)", sqlConn))
                {
                    sqlcmd1.Parameters.AddWithValue("@report_period", reportPeriod);
                    sqlda1 = new SqlDataAdapter(sqlcmd1);
                    dt1 = new DataTable();
                    dt1.TableName = "Sales";
                    sqlda1.Fill(dt1);
                    ds.Tables.Add(dt1);
                };
                using (SqlCommand sqlcmd2 = new SqlCommand("SELECT * FROM dbo.GetPivotTableArchivedStocks(@arch_stock_date)", sqlConn))
                {
                    sqlcmd2.Parameters.AddWithValue("@arch_stock_date", archStockDate);
                    sqlda2 = new SqlDataAdapter(sqlcmd2);
                    dt2 = new DataTable();
                    dt2.TableName = "Archived_Stocks";
                    sqlda2.Fill(dt2);
                    ds.Tables.Add(dt2);
                };
                sqlConn.Close();

                try
                {
                    foreach (DataTable dataTable in ds.Tables)
                    {
                        switch (dataTable.TableName)
                        {
                            case "Debt":
                                fullPath = path + "\\PivotTableDebt.xls";
                                break;
                            case "Sales":
                                fullPath = path + "\\PivotTableSales.xls";
                                break;
                            case "Archived_Stocks":
                                fullPath = path + "\\PivotTableArchivedStocks.xls";
                                break;
                        }

                        StreamWriter strmWrtr = new StreamWriter(fullPath, false, Encoding.GetEncoding("Windows-1251"));
                        for (int columnID = 0; columnID < dataTable.Columns.Count; columnID++)
                        {
                            strmWrtr.Write(dataTable.Columns[columnID].ToString().ToUpper() + "\t");
                        }
                        strmWrtr.WriteLine();
                        for (int rowID = 0; rowID < dataTable.Rows.Count; rowID++)
                        {
                            for (int columnID = 0; columnID < dataTable.Columns.Count; columnID++)
                            {
                                if (dataTable.Rows[rowID][columnID] != null)
                                {
                                    strmWrtr.Write(Convert.ToString(dataTable.Rows[rowID][columnID]) + "\t");
                                }
                                else
                                {
                                    strmWrtr.Write("\t");
                                }
                            }
                            strmWrtr.WriteLine();
                        }
                        strmWrtr.Close();
                        MessageBox.Show("Дані успішно експортовано в Ексель-файл");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            };
        }
    }
}
