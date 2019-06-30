using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NsExcel = Microsoft.Office.Interop.Excel;
using System.Xml.Linq;

namespace Task1
{
    public partial class DataExport : Form
    {
        private MainForm mainForm;
        public DataExport(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }
        public List<string[]> SelectData()     ///selection of data for export
        {
            bool successfulAdding = false;
            string[] tempData = new string[6];
            tempData[0] = textBox1.Text;
            tempData[1] = textBox2.Text;
            tempData[2] = textBox3.Text;
            tempData[3] = textBox4.Text;
            tempData[4] = textBox5.Text;
            tempData[5] = textBox6.Text;
            List<string[]> toExport = new List<string[]>();
            foreach (string[] data in mainForm.data)
            {
                bool checkData = true;
                for (int i = 0; i < 6; i++)
                {
                    /* var query = from oneData in data
                                 where tmp != string.Empty
                                 where data[i + 1] == tmp
                                 where i == 5
                                 select tmp;
                                 */
                    if (data[i + 1] != tempData[i]) //check for equality the entered field and the table field
                        if (tempData[i] == string.Empty) continue;
                        else
                        {
                            checkData = false;
                            break;
                        }
                }
                if (checkData)
                {
                    toExport.Add(data);//add to list for export

                    successfulAdding = true;
                }
            }
            if(!successfulAdding)       //add check
            {
                ErrorInput errorInput = new ErrorInput();
                errorInput.ShowDialog();
                Hide();
                if (DialogResult == DialogResult.OK)
                {
                    errorInput.Close();
                    new DataExport(mainForm).ShowDialog();
                }
                return null;
            }
            else 
                return toExport;
        }
        private void Button1_Click(object sender, EventArgs e)/// click on export to excel button
        {
            List<string[]> toExport;
            toExport = SelectData();
            if(toExport != null)
                ListToExcel(toExport);
        }
        public void ListToExcel(List<string[]> list)///export to Excel
        {
            NsExcel.Application ExcelApp = new NsExcel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 15;

            ExcelApp.Cells[1, 1] = "Date";
            ExcelApp.Cells[1, 2] = "First name";
            ExcelApp.Cells[1, 3] = "Last name";
            ExcelApp.Cells[1, 4] = "Surname";
            ExcelApp.Cells[1, 5] = "City";
            ExcelApp.Cells[1, 6] = "Country";

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length - 1; j++)
                {
                    foreach (string[] strings in list)
                        ExcelApp.Cells[i + 2, j + 1] = strings[j + 1];
                }
            }
            ExcelApp.Visible = true;
        }
        private void Button3_Click(object sender, EventArgs e)///click on export to xml button
        {
            List<string[]> toExport;
            toExport = SelectData();
            if(toExport != null)
                ListToXML(toExport);
        }
        public void ListToXML(List<string[]> list) ///export to xml
        {
            var xEle = new XElement("People",
                from person in list
                select new XElement("Person",
                             new XAttribute("ID", person[0].ToString()),
                               new XElement("Date", person[1]),
                               new XElement("FirstName", person[2]),
                               new XElement("LastName", person[3]),
                               new XElement("Surname", person[4]),
                               new XElement("City", person[5]),
                               new XElement("Country", person[6])
                           ));
            xEle.Save("People.xml");
        }
    }
}
