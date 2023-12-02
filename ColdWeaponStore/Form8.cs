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
    public partial class EditForm7 : Form
    {
        readonly bool edit;
        private int id;

        public EditForm7()
        {
            InitializeComponent();
            edit = false;
        }

        public EditForm7(int certificateId, int weaponId, string certificateNumber, string issuingAuthority, DateTime dateOfCertificate)
    : this()
        {
            edit = true;
            this.id = certificateId;
            textBox1.Text = weaponId.ToString();
            textBox2.Text = certificateNumber;
            textBox3.Text = issuingAuthority;
            dateTimePicker1.Value = dateOfCertificate;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var weaponCertificateAdapter = new WeaponCertificateTableAdapter();
            string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            if (edit)
            {
                weaponCertificateAdapter.UpdateQuery(
                    Convert.ToInt32(textBox3.Text),
                    textBox1.Text,
                    textBox2.Text,
                    formattedDate,
                    id);
            }
            else
            {
                weaponCertificateAdapter.InsertQuery(
                    Convert.ToInt32(textBox3.Text),
                    textBox1.Text,
                    textBox2.Text,
                    formattedDate);
            }
            Close();
        }

        private void EditForm7_Load(object sender, EventArgs e)
        {

        }
    }
}
