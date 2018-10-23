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
    public partial class Form1 : Form
    {
        public Form1()
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
            GetDataFromExcel(tbx_FilePath.Text);
        }

        private static DataSet GetDataFromExcel(string xlFilePath)
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

                using (OleDbConnection oleDbConn = new OleDbConnection(excelConnString))
                {
                    oleDbConn.Open();
                    OleDbCommand oledbcmd1 = new OleDbCommand();
                    OleDbCommand oledbcmd2 = new OleDbCommand();
                    OleDbCommand oledbcmd3 = new OleDbCommand();

                    oledbcmd1.CommandText = "Select * From [DataFromDC$C7:N11]"; //продажі
                    oledbcmd1.Connection = oleDbConn;
                    oledbcmd2.CommandText = "Select * From [DataFromDC$C13:G14]"; //дебіторка КЕГ
                    oledbcmd2.Connection = oleDbConn;
                    oledbcmd3.CommandText = "Select * From [DataFromDC$C16:H17]"; //дебіторка ГРН
                    oledbcmd3.Connection = oleDbConn;

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "Sales";
                    DataTable dt2 = new DataTable();
                    dt2.TableName = "DebtKEG";
                    DataTable dt3 = new DataTable();
                    dt3.TableName = "DebtMONEY";
                    OleDbDataAdapter odda1 = new OleDbDataAdapter(oledbcmd1);
                    OleDbDataAdapter odda2 = new OleDbDataAdapter(oledbcmd2);
                    OleDbDataAdapter odda3 = new OleDbDataAdapter(oledbcmd3);

                    odda1.Fill(dt1);
                    odda2.Fill(dt2);
                    odda3.Fill(dt3);
                    ds.Tables.Add(dt1);
                    ds.Tables.Add(dt2);
                    ds.Tables.Add(dt3);

                    oleDbConn.Close();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return ds;
        }
    }
}
