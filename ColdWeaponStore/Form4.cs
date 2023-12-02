using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private int pricePerPiece;
        public EditForm3()
        {
            InitializeComponent();
            edit = false;
        }

        public EditForm3(int orderDetailId, int orderId, int weaponId, int amount)
    : this()
        {
            edit = true;
            this.id = orderDetailId;
            orderComboBox.Text = orderId.ToString();
            weaponComboBox.Text = weaponId.ToString();
            textBox3.Text = amount.ToString();
        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(!validation())
            {
                return;
            }

            try
            {
                var orderDetailAdapter = new OrderDetailTableAdapter();
                pricePerPiece = defineOveralSum(int.Parse(orderComboBox.Text), int.Parse(weaponComboBox.Text), int.Parse(textBox3.Text));


                if (edit)
                {
                    orderDetailAdapter.UpdateQuery(
                        Convert.ToInt32(orderComboBox.Text), // OrderID
                        Convert.ToInt32(weaponComboBox.Text), // WeaponID
                        Convert.ToInt32(textBox3.Text), // Amount
                        pricePerPiece, // PricePerPiece
                        id);
                }
                else
                {
                    orderDetailAdapter.InsertQuery(
                        Convert.ToInt32(orderComboBox.Text), // OrderID
                        Convert.ToInt32(weaponComboBox.Text), // WeaponID
                        Convert.ToInt32(textBox3.Text), // Amount
                        pricePerPiece); // PricePerPiece
                }
                string operation = edit ? "edited" : "inserted";
                MessageBox.Show($"Order details has been {operation}");

                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int defineOveralSum(int orderDetailsId, int weaponId, int amount)
        {
            string connectionString = "Data Source=DESKTOP-JGN4EB0;Initial Catalog=ColdWeaponStore;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlCommand = @"SELECT Price * @amount
                FROM OrderDetail
                JOIN Weapon w ON w.WeaponID = @weaponId
                WHERE OrderDetail.OrderDetailID = @orderDetailId";
                using (var command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(@"orderDetailId", SqlDbType.Int).Value = orderDetailsId;
                    command.Parameters.Add(@"weaponId", SqlDbType.Int).Value=weaponId;
                    command.Parameters.Add(@"amount", SqlDbType.Int).Value=amount;
                    var result = command.ExecuteScalar();

                    return result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }

        private void EditForm3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Weapon' table. You can move, or remove it, as needed.
            this.weaponTableAdapter.Fill(this.coldWeaponStoreDataSet.Weapon);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Orders' table. You can move, or remove it, as needed.
            this.ordersTableAdapter.Fill(this.coldWeaponStoreDataSet.Orders);

        }

        private bool validation()
        {
            if(string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Amount field is empty");
                return false;
            }
            if(!int.TryParse(textBox3.Text,out var _))
            {
                MessageBox.Show("Amount nust be a number");
                return false;
            }

            return true;
        }
    }
}
