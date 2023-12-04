using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColdWeaponStore
{
    public partial class Form9 : Form
    {
        private bool edit = false;
        private int id;
        public Form9()
        {
            InitializeComponent();
        }
        public Form9(int id, string name,string pNumber,string email,string address) : this()
        {
            this.id = id;
            textBox1.Text = name;
            textBox2.Text = pNumber;
            textBox3.Text = email;
            textBox4.Text = address;

            edit = true;
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Phone number is empty");
                return false;
            }
            var numberLength = textBox2.Text.ToCharArray().Length;
            if (numberLength > 13)
            {
                MessageBox.Show("Uncorrect client phone number format");
                return false;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Email field is empty");
                return false;
            }
            var letters = textBox3.Text.ToCharArray();
            if (!letters.Contains('@'))
            {
                MessageBox.Show("Uncorrect email format");
                return false;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Address is empty");
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }
            try
            {
                var adapter = new SupplierTableAdapter();
                if (edit)
                {
                    adapter.UpdateQuery(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text,id);
                }
                else
                {
                    adapter.InsertQuery(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                }

                string op = edit ? "edited" : "inserted";
                MessageBox.Show($"Supplier has been {op}");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
