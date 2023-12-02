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
    
    public partial class EditForm5 : Form
    {
        readonly bool edit;
        private int id;
        public EditForm5()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm5(int weaponDetailId, string weaponMaterial, decimal weaponLength, decimal weaponWidth, decimal weaponThickness, string weaponType, int weaponId)
    : this()
        {
            edit = true;
            this.id = weaponDetailId;
            textBox1.Text = weaponMaterial;
            textBox2.Text = weaponLength.ToString();
            textBox3.Text = weaponWidth.ToString();
            textBox4.Text = weaponThickness.ToString();
            textBox5.Text = weaponType;
            textBox6.Text = weaponId.ToString();
        }



        private void button1_Click(object sender, EventArgs e)
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
                    Convert.ToInt32(textBox6.Text), // WeaponID
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
                    Convert.ToInt32(textBox6.Text)); // WeaponID
            }
            Close();
        }
    }
}
