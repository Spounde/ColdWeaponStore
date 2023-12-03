using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
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
    public partial class SupplierEditForm : Form
    {
        private bool edit = false;
        private int id;
        public SupplierEditForm()
        {
            InitializeComponent();
        }

        public SupplierEditForm(int id, string supplierName, string supplierNumber, string supplierEmail, string supplierAdress) : this() 
        {
            edit = true;
            label1.Text = supplierName;
            label2.Text = supplierNumber;
            label3.Text = supplierEmail;
            label4.Text = supplierAdress;
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
                if(edit)
                {
                    adapter.Insert(label1.Text, label2.Text, label3.Text,label4.Text);
                }
                else
                {
                    adapter.Update(label1.Text, label2.Text, label3.Text, label4.Text);
                }
                var op = edit ? "edited" : "inserted";
                MessageBox.Show($"Supplier has been {op}");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Username is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Client phone number is empty");
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
    }
}
