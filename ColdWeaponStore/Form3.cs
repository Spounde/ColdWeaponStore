using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColdWeaponStore
{
    public partial class EditForm2 : Form
    {
        readonly bool edit;
        private int id;
        private decimal overalSum;
        private int clientId;
        private DateTime date;
        public EditForm2()
        {
            InitializeComponent();
            edit = false;
            dateTimePicker1.MaxDate = DateTime.Now;
        }
        public EditForm2(int id, int clientID, DateTime date) : this()
        { 
            edit = true;
            this.id = id;

            clientId = clientID;

            this.date = date;
        }


        private void EditForm2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.coldWeaponStoreDataSet.Client);

            DataTable clientTable = coldWeaponStoreDataSet.Tables["Client"]; 

            if (clientTable != null)
            {
                foreach (DataRow row in clientTable.Rows)
                {
                    if (row["ClientID"] != DBNull.Value)
                    {
                        clientComboBox.Items.Add(row["ClientID"]);
                    }
                }
            }

            if (clientComboBox.Items.Contains(clientId))
            {
                clientComboBox.Text = clientId.ToString();
            }

            dateTimePicker1.Value = date;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(!validate())
            {
                return;
            }

            var ordersAdapter = new OrdersTableAdapter();
            try
            {
                string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                if (edit)
                {
                    ordersAdapter.UpdateQuery(
                        Convert.ToInt32(clientComboBox.Text),
                        formattedDate,
                        overalSum,
                        id);
                }
                else
                {
                    ordersAdapter.InsertQuery(
                        Convert.ToInt32(clientComboBox.Text),
                        formattedDate,
                        overalSum);
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
            if(string.IsNullOrEmpty(clientComboBox.Text))
            {
                MessageBox.Show("Client insn`t selected");
                return false;
            }

            overalSum = defineOveralSum(id);
            if(overalSum == 0)
            {
                MessageBox.Show("Incorrect order. We don`t have information about that order details");
                return false;
            }
            return true;   
        }

        private decimal defineOveralSum(int orderID)
        {
            string connectionString = "Data Source=DESKTOP-JGN4EB0;Initial Catalog=ColdWeaponStore;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlCommand = @"SELECT SUM(PricePerPiece) FROM OrderDetail WHERE OrderID = @orderId";
                using (var command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(@"orderId", SqlDbType.Int).Value = orderID;
                    var result = command.ExecuteScalar();

                    return result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }
    }
}
