using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColdWeaponStore
{
    public partial class EditForm6 : Form
    {
        readonly bool edit;
        private int id;
        private int weaponID;
        public EditForm6(int weaponHistoryId, DateTime dateAcquired, string country, int weaponId)
    : this()
        {
            edit = true;
            id = weaponHistoryId;
            dateTimePicker1.Value = dateAcquired;
            textBox1.Text = country;
            weaponID = weaponId;
        }
        public EditForm6()
        {
            InitializeComponent();
        }

        private void fillComboBox()
        {
            var ids = new List<int>();
            string sqlCommand = @"SELECT DISTINCT WeaponID FROM Weapon;";
            string connectionString = @"Data Source=DESKTOP-JGN4EB0;Initial Catalog=ColdWeaponStore;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlCommand, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ids.Add((int)reader[0]);
                    }
                }
            }
            foreach (var id in ids)
            {
                comboBox1.Items.Add(id);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }

            try
            {
                var weaponHistoryAdapter = new WeaponHistoryTableAdapter();
                string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");


                if (edit)
                {
                    weaponHistoryAdapter.UpdateQuery(
                        formattedDate,
                        textBox1.Text,
                        Convert.ToInt32(comboBox1.Text),
                        id);
                }
                else
                {
                    weaponHistoryAdapter.InsertQuery(
                        formattedDate,
                        textBox1.Text,
                        Convert.ToInt32(comboBox1.Text));
                }
                string op = edit ? "edited" : "inserted";
                MessageBox.Show($"Weapon history has been {op}");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void EditForm6_Load(object sender, EventArgs e)
        {
            fillComboBox();
            if (comboBox1.Items.Contains(weaponID))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(weaponID);
            }
        }

        private bool validation()
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Coutry field can`t be empty");
                return false;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Weapon ID field can`t be empty");
                return false;
            }

            return true;
        }
    }
}
