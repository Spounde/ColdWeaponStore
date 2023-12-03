using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColdWeaponStore
{

    public partial class EditForm5 : Form
    {
        readonly bool edit;
        private int id;
        private int weaponID;
        public EditForm5()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm5(int weaponDetailId, string weaponMaterial, decimal weaponLength, decimal weaponWidth, decimal weaponThickness, string weaponType, int weaponId)
    : this()
        {
            edit = true;
            id = weaponDetailId;
            textBox1.Text = weaponMaterial;
            textBox2.Text = weaponLength.ToString();
            textBox3.Text = weaponWidth.ToString();
            textBox4.Text = weaponThickness.ToString();
            textBox5.Text = weaponType;
            weaponID = weaponId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }
            try
            {
                var weaponDetailsAdapter = new WeaponDetailsTableAdapter();

                if (edit)
                {
                    weaponDetailsAdapter.UpdateQuery(
                        textBox1.Text, // WeaponMaterial
                        Convert.ToDecimal(textBox2.Text), // WeaponLength
                        Convert.ToDecimal(textBox3.Text), // WeaponWidth
                        Convert.ToDecimal(textBox4.Text), // WeaponThickness
                        textBox5.Text, // WeaponType
                        Convert.ToInt32(comboBox1.Text), // WeaponID
                        id);
                }
                else
                {
                    weaponDetailsAdapter.InsertQuery(
                        textBox1.Text, // WeaponMaterial
                        Convert.ToDecimal(textBox2.Text), // WeaponLength
                        Convert.ToDecimal(textBox3.Text), // WeaponWidth
                        Convert.ToDecimal(textBox4.Text), // WeaponThickness
                        textBox5.Text, // WeaponType
                        Convert.ToInt32(comboBox1.Text)); // WeaponID
                }
                string operation = edit ? "edited" : "inserted";
                MessageBox.Show($"Weapon detail has been {operation}");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    while(reader.Read())
                    {
                        ids.Add((int)reader[0]);
                    }
                }
            }
            foreach(var id in ids)
            {
                comboBox1.Items.Add(id);
            }
        }


        private void EditForm5_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.WeaponDetails' table. You can move, or remove it, as needed.
            this.weaponDetailsTableAdapter.Fill(this.coldWeaponStoreDataSet.WeaponDetails);

            fillComboBox();
            if(comboBox1.Items.Contains(weaponID))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(weaponID);
            }
        }

        private bool validation()
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Weapon material can`t be empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Weapon length can`t be empty");
                return false;
            }
            if(!int.TryParse(textBox2.Text, out int _))
            {
                MessageBox.Show("Weapon length must be a number");
                return false;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Weapon width can`t be empty");
                return false;
            }
            if (!int.TryParse(textBox3.Text, out int _))
            {
                MessageBox.Show("Weapon width must be a number");
                return false;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Weapon thickness can`t be empty");
                return false;
            }
            if (!int.TryParse(textBox4.Text, out int _))
            {
                MessageBox.Show("Weapon thickness must be a number");
                return false;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Weapon type can`t be empty");
                return false;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Weapon ID can`t be empty");
                return false;
            }

            return true;
        }
    }
}
