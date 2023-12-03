using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColdWeaponStore
{
    public partial class EditForm7 : Form
    {
        readonly bool edit;
        private int id;
        private int weaponID;

        public EditForm7()
        {
            InitializeComponent();
            edit = false;
        }

        public EditForm7(int certificateId, int weaponId, string certificateNumber, string issuingAuthority, DateTime dateOfCertificate)
    : this()
        {
            edit = true;
            this.id = certificateId;
            weaponID = weaponId;
            textBox2.Text = certificateNumber;
            textBox3.Text = issuingAuthority;
            dateTimePicker1.Value = dateOfCertificate;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }
            try
            {
                var weaponCertificateAdapter = new WeaponCertificateTableAdapter();
                string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                if (edit)
                {
                    weaponCertificateAdapter.UpdateQuery(
                        Convert.ToInt32(comboBox1.Text),
                        textBox2.Text,
                        textBox3.Text,
                        formattedDate,
                        id);
                }
                else
                {
                    weaponCertificateAdapter.InsertQuery(
                        Convert.ToInt32(comboBox1.Text),
                        textBox2.Text,
                        textBox3.Text,
                        formattedDate);
                }
                string op = edit ? "edited" : "inserted";
                MessageBox.Show($"Weapon certificate has been {op}");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditForm7_Load(object sender, EventArgs e)
        {
            fillComboBox();
            if(comboBox1.Items.Contains(weaponID))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(weaponID);
            }
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

        private bool validation()
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Weapon ID can`t be empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Certificate number can`t be empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Issuing authority can`t be empty");
                return false;
            }

            return true;
        }
    }
}
