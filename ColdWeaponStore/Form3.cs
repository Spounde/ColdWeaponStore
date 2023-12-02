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
            dateTimePicker1.MaxDate = DateTime.Now;
        }
        public EditForm2(int id, int clientID, DateTime date, decimal overallSum)
    : this()
        {
            edit = true;
            this.id = id;
            comboBox1.Text = clientID.ToString();
            dateTimePicker1.Value = date;
            textBox2.Text = overallSum.ToString();
        }

        private void EditForm2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.coldWeaponStoreDataSet.Client);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var ordersAdapter = new OrdersTableAdapter();

            try
            {
                string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                if (edit)
                {
                    ordersAdapter.UpdateQuery(
                        Convert.ToInt32(comboBox1.Text),
                        formattedDate,
                        Convert.ToDecimal(textBox2.Text),
                        id);
                }
                else
                {
                    ordersAdapter.InsertQuery(
                        Convert.ToInt32(comboBox1.Text),
                        formattedDate,
                        Convert.ToDecimal(textBox2.Text));
                }
                string action = edit ? "edited" : "inserted";

                MessageBox.Show($"Order has been {action}");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool validate()
        {
            if(string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Client insn`t selected");
                return false;
            }

            return true;   
        }
    }
}
