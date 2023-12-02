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

            var clientAdapter = new ClientTableAdapter();

            if (edit)
                {
                clientAdapter.UpdateQuery(
                        textBox1.Text,
                        textBox2.Text,
                        textBox3.Text,
                        textBox4.Text,
                        id);
                }
                else
                {
                clientAdapter.InsertQuery(
                        textBox1.Text,
                        textBox2.Text,
                        textBox3.Text,
                        textBox4.Text);
                }
                Close();
            

        }
    }
}
