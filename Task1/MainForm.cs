using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Task1
{
    public partial class MainForm : Form
    {
        List<Person> listOfPerson = new List<Person>();
        public List<string[]> data = new List<string[]>();
        public MainForm()
        {
            InitializeComponent();
            ReadCSV();
            LoadData();
        }
        public void ReadCSV()///read data from .csv
        {
            using (TextFieldParser textFieldParser = new TextFieldParser(@"People.csv"))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(";");
                while (!textFieldParser.EndOfData)
                {
                    string[] dataAboutPeople = textFieldParser.ReadFields();
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.Persons.Add(new Person(date: dataAboutPeople[0], firstName: dataAboutPeople[1], lastName: dataAboutPeople[2], surname: dataAboutPeople[3], city: dataAboutPeople[4], country: dataAboutPeople[5]));
                        db.SaveChanges();
                    }
                }
            }
            WriteToSql();
        }    
        public void WriteToSql()///database entry
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (Person person in listOfPerson)
                {
                    db.Persons.Add(person);
                    db.SaveChanges();
                }
            }
        }
        private void LoadData()///data loading in dataGridView
        {
            string connectionString = "Data Source= .\\SQLEXPRESS;Initial Catalog=People;Integrated Security=True;";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string query = "SELECT * FROM People ORDER BY Id";//request for selection and sorting by key
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                data.Add(new string[7]);
                data[data.Count - 1][0] = sqlDataReader[0].ToString();
                data[data.Count - 1][1] = sqlDataReader[1].ToString();
                data[data.Count - 1][2] = sqlDataReader[2].ToString();
                data[data.Count - 1][3] = sqlDataReader[3].ToString();
                data[data.Count - 1][4] = sqlDataReader[4].ToString();
                data[data.Count - 1][5] = sqlDataReader[5].ToString();
                data[data.Count - 1][6] = sqlDataReader[6].ToString();
            }
            sqlDataReader.Close();
            sqlConnection.Close();

            foreach (string[] s in data)
                dataGridViewMain.Rows.Add(s);
        }
        private void Button1_Click(object sender, EventArgs e)///pressing the button for export
        {
            DataExport dataExport = new DataExport(this);
            dataExport.ShowDialog();
        }
        private void CancelButton_Click(object sender, EventArgs e)///pressing the exit button
        {
            Environment.Exit(0);
        }
    }
}
