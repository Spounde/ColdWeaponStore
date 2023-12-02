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
    public partial class EditForm2 : Form
    {
        readonly bool edit;
        private int id;
        public EditForm2()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm2(int id, int clientID, DateTime date, decimal overallSum)
    : this()
        {
            edit = true;
            this.id = id;
            textBox1.Text = clientID.ToString();
            dateTimePicker1.Value = date;
            textBox2.Text = overallSum.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var ordersAdapter = new OrdersTableAdapter();

            string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"); 

            if (edit)
            {
                ordersAdapter.UpdateQuery(
                    Convert.ToInt32(textBox1.Text),
                    formattedDate,
                    Convert.ToDecimal(textBox2.Text),
                    id);
            }
            else
            {
                ordersAdapter.InsertQuery(
                    Convert.ToInt32(textBox1.Text),
                    formattedDate,
                    Convert.ToDecimal(textBox2.Text));
            }
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
