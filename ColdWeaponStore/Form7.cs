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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ColdWeaponStore
{
    public partial class EditForm6 : Form
    {
        readonly bool edit;
        private int id;
        public EditForm6(int weaponHistoryId, DateTime dateAcquired, string country, int weaponId)
    : this()
        {
            edit = true;
            this.id = weaponHistoryId;
            dateTimePicker1.Value = dateAcquired;
            textBox1.Text = country;
            textBox3.Text = weaponId.ToString();
        }
        public EditForm6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var weaponHistoryAdapter = new WeaponHistoryTableAdapter();
            string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");


            if (edit)
            {
                weaponHistoryAdapter.UpdateQuery(
                    formattedDate,
                    textBox1.Text,
                    Convert.ToInt32(textBox3.Text),
                    id);
            }
            else
            {
                weaponHistoryAdapter.InsertQuery(
                    formattedDate,
                    textBox1.Text,
                    Convert.ToInt32(textBox3.Text));
            }
            Close();
        }
    }
}
