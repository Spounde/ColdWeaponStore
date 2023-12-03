using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ColdWeaponStore
{
    public partial class EditForm4 : Form
    {
        readonly bool edit;
        private int id;
        private int supplierID;
        public EditForm4()
        {
            InitializeComponent();
            edit = false;
        }
        public EditForm4(int weaponId, string weaponName, decimal price, int supplierId) : this()
        {
            edit = true;
            this.id = weaponId;
            textBox1.Text = weaponName;
            textBox2.Text = price.ToString();
            supplierID = supplierId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!validate())
            {
                return;
            }
            try
            {
                var weaponAdapter = new WeaponTableAdapter();

                if (edit)
                {
                    weaponAdapter.UpdateQuery(
                        textBox1.Text,
                        Convert.ToDecimal(textBox2.Text),
                        Convert.ToInt32(comboBox1.Text),
                        id);

                    string connectionString = "Data Source=DESKTOP-JGN4EB0;Initial Catalog=ColdWeaponStore;Integrated Security=True";
                    string sqlCommand = @"
                    SELECT OrderDetail.OrderDetailID, Price * OrderDetail.Amount AS PricePerPieceValue
                    FROM OrderDetail
                    JOIN Weapon w ON w.WeaponID = OrderDetail.WeaponID";
                    string updateCommand = @"
                    UPDATE OrderDetail
                    SET 
                    PricePerPiece = @value
                    WHERE 
                    OrderDetailID = @orderDetailID";
                    string updateOrders = @"
                    UPDATE Orders
                    SET OverallSum = (
                        SELECT SUM(PricePerPiece)
                        FROM OrderDetail
                        WHERE OrderDetail.OrderID = Orders.OrderID
                    )";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand(sqlCommand, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var orderDetailID = (int)reader[0];
                                    var value = reader[1];

                                    using (var updateConnection = new SqlConnection(connectionString))
                                    {
                                        updateConnection.Open();
                                        using (var updCommand = new SqlCommand(updateCommand, updateConnection))
                                        {
                                            updCommand.Parameters.Add("@value", SqlDbType.Int).Value = int.Parse(value.ToString());
                                            updCommand.Parameters.Add("@orderDetailID", SqlDbType.Int).Value = orderDetailID;

                                            updCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }



                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(updateOrders, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    weaponAdapter.InsertQuery(
                        textBox1.Text,
                        Convert.ToDecimal(textBox2.Text),
                        Convert.ToInt32(comboBox1.Text));
                }

                string operation = edit ? "edited" : "inserted";
                MessageBox.Show($"Weapon has been {operation}");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void EditForm4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Supplier' table. You can move, or remove it, as needed.
            this.supplierTableAdapter.Fill(this.coldWeaponStoreDataSet.Supplier);
            DataTable supplierTable = coldWeaponStoreDataSet.Tables["Supplier"];

            if (supplierTable != null)
            {
                foreach (DataRow row in supplierTable.Rows)
                {
                    if (row["SupplierID"] != DBNull.Value)
                    {
                        comboBox1.Items.Add(row["SupplierID"]);
                    }
                }
            }

            if (comboBox1.Items.Contains(supplierID))
            {
                comboBox1.Text = supplierID.ToString();
            }
        }

        private bool validate()
        {
            if(string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Supplier field is empty");
                return false;
            }
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Weapon name is empty");
                return false;
            }
            if(string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Price field is empty");
                return false;
            }
            if(!int.TryParse(textBox2.Text, out var _))
            {
                MessageBox.Show("Price must be a number");
                return false;
            }

            return true;
        }
    }
}
