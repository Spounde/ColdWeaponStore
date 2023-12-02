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
    public partial class EditForm3 : Form
    {
        readonly bool edit;
        private int id;
        public EditForm3()
        {
            InitializeComponent();
            edit = false;
        }

        public EditForm3(int orderDetailId, int orderId, int weaponId, int amount, decimal pricePerPiece)
    : this()
        {
            edit = true;
            this.id = orderDetailId;
            textBox1.Text = orderId.ToString();
            textBox2.Text = weaponId.ToString();
            textBox3.Text = amount.ToString();
            textBox4.Text = pricePerPiece.ToString();
        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            var orderDetailAdapter = new OrderDetailTableAdapter();

            if (edit)
            {
                orderDetailAdapter.UpdateQuery(
                    Convert.ToInt32(textBox1.Text), // OrderID
                    Convert.ToInt32(textBox2.Text), // WeaponID
                    Convert.ToInt32(textBox3.Text), // Amount
                    Convert.ToDecimal(textBox4.Text), // PricePerPiece
                    id);
            }
            else
            {
                orderDetailAdapter.InsertQuery(
                    Convert.ToInt32(textBox1.Text), // OrderID
                    Convert.ToInt32(textBox2.Text), // WeaponID
                    Convert.ToInt32(textBox3.Text), // Amount
                    Convert.ToDecimal(textBox4.Text)); // PricePerPiece
            }
            Close();
        }
    }
}
