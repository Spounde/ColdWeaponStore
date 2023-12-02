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
    public partial class EditForm : Form
    {
        private int id;

        readonly bool edit;
        public EditForm()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm(int id, string clientName, string clientPhoneNumber, string clientEmail, string clientAddress)
    : this()
        {
            edit = true;
            this.id = id;
            textBox1.Text = clientName;
            textBox2.Text = clientPhoneNumber;
            textBox3.Text = clientEmail;
            textBox4.Text = clientAddress;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }

            var clientAdapter = new ClientTableAdapter();

            try
            {
                if (edit)
                {
                    clientAdapter.UpdateQuery(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, id);
                }
                else
                {
                    clientAdapter.InsertQuery(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                }

                string action = edit ? "edited" : "inserted";
                MessageBox.Show($"Client has been {action}");
                Close();
            }
            catch (Exception ex)
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
            if(numberLength > 13)
            {
                MessageBox.Show("Uncorrect client phone number format");
                return false;
            }
            if(string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Email field is empty");
                return false;
            }
            var letters = textBox3.Text.ToCharArray();
            if(!letters.Contains('@'))
            {
                MessageBox.Show("Uncorrect email format");
                return false;
            }
            if(string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Address is empty");
                return false;
            }

            return true;
        }
    }
}
