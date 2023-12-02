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
    public partial class EditForm4 : Form
    {
        readonly bool edit;
        private int id;
        public EditForm4()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm4(int weaponId, string weaponName, decimal price, int weaponDetailId, int supplierId)
    : this()
        {
            edit = true;
            this.id = weaponId;
            textBox1.Text = weaponName;
            textBox2.Text = price.ToString();
            textBox3.Text = weaponDetailId.ToString();
            textBox4.Text = supplierId.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var weaponAdapter = new WeaponTableAdapter();

            if (edit)
            {
                weaponAdapter.UpdateQuery(
                    textBox1.Text, 
                    Convert.ToDecimal(textBox2.Text), 
                    Convert.ToInt32(textBox3.Text), 
                    Convert.ToInt32(textBox4.Text), 
                    id);
            }
            else
            {
                weaponAdapter.InsertQuery(
                    textBox1.Text, 
                    Convert.ToDecimal(textBox2.Text), 
                    Convert.ToInt32(textBox3.Text), 
                    Convert.ToInt32(textBox4.Text)); 
            }
            Close();
        }
    }
}
