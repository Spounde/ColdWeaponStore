using ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace ColdWeaponStore
{
    public partial class Form1 : Form
    {
        private bool edit;
        private string currentTable = "Client";
        private string currentFilter = "";
        private Dictionary<string, List<string>> tablesWithColums = new Dictionary<string, List<string>>()
        {
            { 
                "Client", new List<string>
                {
                    "ClientID",
                    "ClientName",
                    "ClientPhoneNumber",
                    "ClientEmail",
                    "ClientAddress"
                }
            },
            { 
                "Orders", new List<string>
                {
                    "OrderID",
                    "ClientID",
                    "Date",
                    "OverallSum"
                }
            },
            { 
                "OrderDetails", new List<string>
                {
                   "OrderDetailID",
                   "OrderID",
                   "WeaponID",
                   "Amount",
                   "PricePerPiece"
                }
            },
            { 
                "Weapon", new List<string>
                {
                    "WeaponID",
                    "WeaponName",
                    "Price",
                    "SupplierID"
                }
            },
            { 
                "WeaponCertificate", new List<string>
                {
                    "CertificateID",
                    "WeaponID",
                    "CertificateNumber",
                    "IssuingAuthority",
                    "DateOfCertificate"
                }
            },
            { 
                "WeaponHistory", new List<string>
                {
                    "HistoryID",
                    "DateAcquired",
                    "Country",
                    "WeaponID"
                }
            },
            { 
                "Supplier", new List<string>
                {
                    "SupplierID",
                    "SupplierName",
                    "SupplierPhoneNumber",
                    "SupplierEmail",
                    "SupplierAddress"
                }
            },
            { 
                "WeaponDetails", new List<string>
                {
                    "WeaponDetailsID",
                    "WeaponMaterial",
                    "WeaponLength",
                    "WeaponWidth",
                    "WeaponThickness",
                    "WeaponType",
                    "WeaponID"
                }
            },

        };

        public Form1()
        {
            InitializeComponent();

            textBoxMinPrice.Text = "0";
            textBoxMaxPrice.Text = "10000";
        }

        private void setValuesForSearchCombobox()
        {
            comboBox1.Text = string.Empty;
            if (tablesWithColums.TryGetValue(currentTable, out var values))
            {
                comboBox1.Items.Clear();
                foreach(var item in values)
                {
                    comboBox1.Items.Add(item);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.WeaponDetails' table. You can move, or remove it, as needed.
            this.weaponDetailsTableAdapter.Fill(this.coldWeaponStoreDataSet.WeaponDetails);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.WeaponHistory' table. You can move, or remove it, as needed.
            this.weaponHistoryTableAdapter.Fill(this.coldWeaponStoreDataSet.WeaponHistory);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.WeaponCertificate' table. You can move, or remove it, as needed.
            this.weaponCertificateTableAdapter.Fill(this.coldWeaponStoreDataSet.WeaponCertificate);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Weapon' table. You can move, or remove it, as needed.
            this.weaponTableAdapter.Fill(this.coldWeaponStoreDataSet.Weapon);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Supplier' table. You can move, or remove it, as needed.
            this.supplierTableAdapter.Fill(this.coldWeaponStoreDataSet.Supplier);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Orders' table. You can move, or remove it, as needed.
            this.ordersTableAdapter.Fill(this.coldWeaponStoreDataSet.Orders);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.OrderDetail' table. You can move, or remove it, as needed.
            this.orderDetailTableAdapter.Fill(this.coldWeaponStoreDataSet.OrderDetail);
            // TODO: This line of code loads data into the 'coldWeaponStoreDataSet.Client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.coldWeaponStoreDataSet.Client);
            dataGridView1.DataSource = clientBindingSource;
            bindingNavigator1.BindingSource = clientBindingSource;
            SetSearchControlsVisibility(false);
            dataGridView1.MultiSelect = false;

        }

        private void SetSearchControlsVisibility(bool visible)
        {
            Search.Visible = visible;
            textBox1.Visible = visible;
            label1.Visible = visible;
            button1.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            button2.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            button3.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;

            textBoxMinPrice.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            textBoxMaxPrice.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            dateTimePickerStart.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            dateTimePickerEnd.Visible = currentTable == "Weapon" || currentTable == "Orders" && visible;
            btnShowStats.Visible = (currentTable == "Weapon" || currentTable == "Orders" || currentTable == "Client" || currentTable == "WeaponDetails") && visible;
            if (currentTable == "Weapon")
            {
                btnShowStats.Text = "Show average price of weapons";
            }
            else if (currentTable == "Orders")
            {
                btnShowStats.Text = "Show average order amount";
            }
            else if (currentTable == "Client")
            {
                btnShowStats.Text = "Show the customer with the highest number of orders";
            }
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = clientBindingSource;


            bindingNavigator1.BindingSource = clientBindingSource;

            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            SetSearchControlsVisibility(true);
            currentTable = "Client";
            label1.Text = "Please enter Client name  you want to see";
            label2.Text = "Client";

            setValuesForSearchCombobox();
        }
        private void ShowClientWithMostOrders()
        {
            if (coldWeaponStoreDataSet.Orders.Rows.Count > 0)
            {
                int maxOrderCount = coldWeaponStoreDataSet.Orders
                    .AsEnumerable()
                    .GroupBy(order => order.Field<int>("ClientID"))
                    .Max(group => group.Count());

                var clientsWithMostOrders = coldWeaponStoreDataSet.Orders
                    .AsEnumerable()
                    .GroupBy(order => order.Field<int>("ClientID"))
                    .Select(group => new
                    {
                        ClientID = group.Key,
                        TotalOrders = group.Count(),
                        TotalSum = group.Sum(order => order.Field<decimal>("OverallSum"))
                    })
                    .Where(result => result.TotalOrders == maxOrderCount)
                    .ToList();

                if (clientsWithMostOrders.Any())
                {
                    StringBuilder message = new StringBuilder("Clients with the most orders:\n\n");
                    foreach (var clientStats in clientsWithMostOrders)
                    {
                        var client = coldWeaponStoreDataSet.Client
                            .AsEnumerable()
                            .FirstOrDefault(c => c.Field<int>("ClientID") == clientStats.ClientID);

                        if (client != null)
                        {
                            message.AppendLine($"Client ID: {clientStats.ClientID}, Name: {client.Field<string>("ClientName")}, " +
                                               $"Number of Orders: {clientStats.TotalOrders}, " +
                                               $"Total Order Value: {clientStats.TotalSum:C2}");
                        }
                    }

                    MessageBox.Show(message.ToString(), "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No clients with orders found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Order table is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void ApplyFilter(string searchFilter = "")
        {
            string combinedFilter = currentFilter;

            if (!string.IsNullOrEmpty(searchFilter))
            {
                if (!string.IsNullOrEmpty(combinedFilter))
                {
                    combinedFilter += " AND ";
                }
                combinedFilter += searchFilter;
            }

            if (currentTable == "Weapon")
            {
                weaponBindingSource.Filter = combinedFilter;
            }
            else if (currentTable == "Orders")
            {
                ordersBindingSource.Filter = combinedFilter;
            }

            dataGridView1.Refresh();
        }

        private void ResetFilter()
        {
            currentFilter = "";
            if (currentTable == "Client")
            {
                clientBindingSource.RemoveFilter();
            }
            else if (currentTable == "Orders")
            {
                ordersBindingSource.RemoveFilter();
            }
            else if (currentTable == "Weapon")
            {
                weaponBindingSource.RemoveFilter();
            }

            textBox1.Text = "";
            dataGridView1.Refresh();
        }



        private void UpdateFilter()
        {
            List<string> filterConditions = new List<string>();

            if (decimal.TryParse(textBoxMinPrice.Text, out decimal minPrice) &&
                decimal.TryParse(textBoxMaxPrice.Text, out decimal maxPrice))
            {
                if (currentTable == "Weapon")
                {
                    filterConditions.Add($"Convert([Price], 'System.Decimal') >= {minPrice}");
                    filterConditions.Add($"Convert([Price], 'System.Decimal') <= {maxPrice}");
                }
                else if (currentTable == "Orders")
                {
                    filterConditions.Add($"Convert([OverallSum], 'System.Decimal') >= {minPrice}");
                    filterConditions.Add($"Convert([OverallSum], 'System.Decimal') <= {maxPrice}");
                }
            }

            string startDate = dateTimePickerStart.Value.ToString("MM/dd/yyyy");
            string endDate = dateTimePickerEnd.Value.ToString("MM/dd/yyyy");

            if (currentTable == "Orders")
            {
                filterConditions.Add($"Date >= #{startDate}#");
                filterConditions.Add($"Date <= #{endDate}#");
            }

            string finalFilter = string.Join(" AND ", filterConditions);

            if (currentTable == "Weapon")
            {
                weaponBindingSource.Filter = finalFilter;
            }
            else if (currentTable == "Orders")
            {
                ordersBindingSource.Filter = finalFilter;
            }
            currentFilter = string.Join(" AND ", filterConditions);

            ApplyFilter();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateFilter();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
            if (currentTable == "Weapon")
            {
                weaponBindingSource.Sort = "Price ASC";
                dataGridView1.Refresh();
            }
            else if (currentTable == "Orders")
            {
                ordersBindingSource.Sort = "OverallSum ASC";
                dataGridView1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            if (currentTable == "Weapon")
            {
                weaponBindingSource.Sort = "Price DESC";
                dataGridView1.Refresh();
            }
            else if (currentTable == "Orders")
            {
                ordersBindingSource.Sort = "OverallSum DESC";
                dataGridView1.Refresh();
            }
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ordersBindingSource;
            bindingNavigator1.BindingSource = ordersBindingSource;
            SetSearchControlsVisibility(true);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            currentTable = "Orders";
            label1.Text = "Please enter ID of order wich details you want to see";
            label2.Text = "Orders";
            setValuesForSearchCombobox();
        }


        private void orderDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = orderDetailBindingSource;


            bindingNavigator1.BindingSource = orderDetailBindingSource;
            SetSearchControlsVisibility(true);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            label2.Text = "OrderDetails";
            currentTable = "OrderDetails";
            setValuesForSearchCombobox();
        }

        private void weaponToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = weaponBindingSource;
            bindingNavigator1.BindingSource = weaponBindingSource;
            SetSearchControlsVisibility(true);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            currentTable = "Weapon";
            label1.Text = "Please enter ID of weapon wich details you want to see";
            label2.Text = "Weapon";
            setValuesForSearchCombobox();
        }


        private void weaponDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = weaponDetailsBindingSource;

            currentTable = "WeaponDetails";
            bindingNavigator1.BindingSource = weaponDetailsBindingSource;
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            SetSearchControlsVisibility(true);
            label2.Text = "WeaponDetails";
            setValuesForSearchCombobox();
        }
        private void ShowAverageLengthByWeaponType()
        {
            if (coldWeaponStoreDataSet.WeaponDetails.Rows.Count > 0)
            {
                var averageLengthByType = coldWeaponStoreDataSet.WeaponDetails
                    .AsEnumerable()
                    .GroupBy(detail => detail.Field<string>("WeaponType"))
                    .Select(group => new
                    {
                        WeaponType = group.Key,
                        AverageLength = group.Average(detail => detail.Field<decimal>("WeaponLength"))
                    })
                    .ToList();

                StringBuilder message = new StringBuilder("Average weapon length by type:\n");
                foreach (var type in averageLengthByType)
                {
                    message.AppendLine($"{type.WeaponType}: {type.AverageLength:F2} см");
                }

                MessageBox.Show(message.ToString(), "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Weapon parts table is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void clientToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = clientBindingSource;


            bindingNavigator1.BindingSource = clientBindingSource;

            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            SetSearchControlsVisibility(false);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm();
            edt.ShowDialog();
            clientTableAdapter.Fill(coldWeaponStoreDataSet.Client);
            coldWeaponStoreDataSet.AcceptChanges();
        }



        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = true;
            var clientRow = new ColdWeaponStoreDataSet.ClientDataTable();
            clientTableAdapter.FillBy(clientRow, Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            object[] row = clientRow.Rows[0].ItemArray;
            var edt = new EditForm(
                Convert.ToInt32(row[0]),
                row[1].ToString(),
                row[2].ToString(),
                row[3].ToString(),
                row[4].ToString()
            );
            edt.ShowDialog();
            clientTableAdapter.Fill(coldWeaponStoreDataSet.Client);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void clientDeleteToolStripMenu_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            int clientId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            DialogResult result = MessageBox.Show($"Do you want to delete client with id {clientId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                var orderDetailAdapter = new OrderDetailTableAdapter();
                orderDetailAdapter.DeleteQueryByClientID(clientId);

                var ordersAdapter = new OrdersTableAdapter();
                ordersAdapter.DeleteQueryForClientDelete(clientId);

                var clientAdapter = new ClientTableAdapter();
                clientAdapter.DeleteQuery(clientId);

                clientAdapter.Fill(coldWeaponStoreDataSet.Client);
                ordersAdapter.Fill(coldWeaponStoreDataSet.Orders);
                orderDetailAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);
                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm2();
            edt.ShowDialog();
            ordersTableAdapter.Fill(coldWeaponStoreDataSet.Orders);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    object[] rowValues = new object[selectedRow.Cells.Count];
                    for (int i = 0; i < selectedRow.Cells.Count; i++)
                    {
                        rowValues[i] = selectedRow.Cells[i].Value;
                    }

                    var edt = new EditForm2(
                        Convert.ToInt32(rowValues[0]),     
                        Convert.ToInt32(rowValues[1]),     
                        Convert.ToDateTime(rowValues[2])  
                    );

                    edt.ShowDialog();

                    ordersTableAdapter.Fill(coldWeaponStoreDataSet.Orders);
                    coldWeaponStoreDataSet.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            
            var orderDetailsAdapter = new OrderDetailTableAdapter();

            orderDetailsAdapter.DeleteQueryByOrderID(orderId);

            var ordersAdapter = new OrdersTableAdapter();
            ordersAdapter.DeleteQuery(orderId);

            ordersAdapter.Fill(coldWeaponStoreDataSet.Orders);
            coldWeaponStoreDataSet.AcceptChanges();

        }

        private void addToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm3(); 
            edt.ShowDialog();
            orderDetailTableAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            edit = true;
            int selectedOrderDetailId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var orderDetailRow = new ColdWeaponStoreDataSet.OrderDetailDataTable();
            orderDetailTableAdapter.FillBy(orderDetailRow, selectedOrderDetailId);

            if (orderDetailRow.Rows.Count == 0)
            {
                MessageBox.Show("No data was found for the selected OrderDetailID.");
                return;
            }

            object[] row = orderDetailRow.Rows[0].ItemArray;
            var edt = new EditForm3(
                Convert.ToInt32(row[0]), 
                Convert.ToInt32(row[1]),
                Convert.ToInt32(row[2]),
                Convert.ToInt32(row[3])
            );

            edt.ShowDialog();
            orderDetailTableAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            int orderDetailId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value); 

            var result = MessageBox.Show($"Do you want to delete order details with id {orderDetailId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                var orderDetailAdapter = new OrderDetailTableAdapter();
                orderDetailAdapter.DeleteQuery(orderDetailId);

                orderDetailAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);
                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void databaseRedactionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm4();
            weaponTableAdapter.Fill(coldWeaponStoreDataSet.Weapon); 
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to edi.");
                return;
            }

            edit = true;
            int selectedWeaponId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value); 
            var weaponRow = new ColdWeaponStoreDataSet.WeaponDataTable();
            weaponTableAdapter.FillBy(weaponRow, selectedWeaponId);

            if (weaponRow.Rows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            object[] row = weaponRow.Rows[0].ItemArray;
            var edt = new EditForm4(
                Convert.ToInt32(row[0]), 
                row[1].ToString(),       
                Convert.ToDecimal(row[2]),
                Convert.ToInt32(row[3]) 
            );

            edt.ShowDialog();
            weaponTableAdapter.Fill(coldWeaponStoreDataSet.Weapon);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void addToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm5();
            edt.ShowDialog();
            weaponDetailsTableAdapter.Fill(coldWeaponStoreDataSet.WeaponDetails);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to edit.");
                return;
            }

            edit = true;
            int selectedWeaponDetailId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var weaponDetailsRow = new ColdWeaponStoreDataSet.WeaponDetailsDataTable();
            weaponDetailsTableAdapter.FillBy(weaponDetailsRow, selectedWeaponDetailId);

            if (weaponDetailsRow.Rows.Count == 0)
            {
                MessageBox.Show("No data was found for the selected WeaponDetailID.");
                return;
            }

            object[] row = weaponDetailsRow.Rows[0].ItemArray;
            var edt = new EditForm5(
                Convert.ToInt32(row[0]), 
                row[1].ToString(),       
                Convert.ToDecimal(row[2]), 
                Convert.ToDecimal(row[3]), 
                Convert.ToDecimal(row[4]), 
                row[5].ToString(),      
                Convert.ToInt32(row[6])  
            );

            edt.ShowDialog();
            weaponDetailsTableAdapter.Fill(coldWeaponStoreDataSet.WeaponDetails); 
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void weaponHistoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = weaponHistoryBindingSource;


            bindingNavigator1.BindingSource = weaponHistoryBindingSource;
            SetSearchControlsVisibility(false);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            currentTable = "WeaponHistory";
            setValuesForSearchCombobox();

        }

        private void weaponCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = weaponCertificateBindingSource;


            bindingNavigator1.BindingSource = weaponCertificateBindingSource;
            SetSearchControlsVisibility(false);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            currentTable = "WeaponCertificate";
            setValuesForSearchCombobox();

        }

        private void addToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm6(); 
            edt.ShowDialog();
            weaponHistoryTableAdapter.Fill(coldWeaponStoreDataSet.WeaponHistory); 
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to edit.");
                return;
            }
            edit = true;
            var weaponHistoryRow = new ColdWeaponStoreDataSet.WeaponHistoryDataTable();
            weaponHistoryTableAdapter.FillBy(weaponHistoryRow, Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            if (weaponHistoryRow.Rows.Count > 0)
            {
                object[] row = weaponHistoryRow.Rows[0].ItemArray;
                var edt = new EditForm6(
                    Convert.ToInt32(row[0]), 
                    Convert.ToDateTime(row[1]), 
                    row[2].ToString(), 
                    Convert.ToInt32(row[3]) 
                );
                edt.ShowDialog();
                weaponHistoryTableAdapter.Fill(coldWeaponStoreDataSet.WeaponHistory); 
                coldWeaponStoreDataSet.AcceptChanges();
            }
            else
            {
                MessageBox.Show("No data was found for the selected weapon history ID.");
            }
        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            int weaponHistoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            DialogResult result = MessageBox.Show($"Do you want to delete weapon history with id {weaponHistoryId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                var weaponHistoryAdapter = new WeaponHistoryTableAdapter();

                weaponHistoryAdapter.DeleteQuery(weaponHistoryId);

                weaponHistoryAdapter.Fill(coldWeaponStoreDataSet.WeaponHistory);
                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void addToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            edit = false;
            var edt = new EditForm7();
            edt.ShowDialog();
            weaponCertificateTableAdapter.Fill(coldWeaponStoreDataSet.WeaponCertificate);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to edit.");
                return;
            }
            edit = true;
            var weaponCertificateRow = new ColdWeaponStoreDataSet.WeaponCertificateDataTable();
            weaponCertificateTableAdapter.FillBy(weaponCertificateRow, Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            if (weaponCertificateRow.Rows.Count > 0)
            {
                object[] row = weaponCertificateRow.Rows[0].ItemArray;
                var edt = new EditForm7(
                    Convert.ToInt32(row[0]), 
                    Convert.ToInt32(row[1]),
                    row[2].ToString(), 
                    row[3].ToString(),
                    Convert.ToDateTime(row[4])
                );
                edt.ShowDialog();
                weaponCertificateTableAdapter.Fill(coldWeaponStoreDataSet.WeaponCertificate); 
                coldWeaponStoreDataSet.AcceptChanges();
            }
            else
            {
                MessageBox.Show("No data was found for the selected Weapon Certificate ID.");
            }
        }

        private void deleteToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }

            int certificateId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            DialogResult result = MessageBox.Show($"Do you want to delete weapon certificate with id {certificateId}?", "Заголовок", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                var weaponCertificateAdapter = new WeaponCertificateTableAdapter();

                weaponCertificateAdapter.DeleteQuery(certificateId);

                weaponCertificateAdapter.Fill(coldWeaponStoreDataSet.WeaponCertificate);
                coldWeaponStoreDataSet.AcceptChanges();
            }
        }
        private void ShowAverageWeaponPrice()
        {
            if (coldWeaponStoreDataSet.Weapon.Rows.Count > 0)
            {
                var averagePrice = coldWeaponStoreDataSet.Weapon
                    .AsEnumerable()
                    .Average(row => row.Field<decimal>("Price"));

                MessageBox.Show($"Average price of a weapon: {averagePrice:C2}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Weapons table empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowAverageOrderSum()
        {
            if (coldWeaponStoreDataSet.Orders.Rows.Count > 0)
            {
                var averageOrderSum = coldWeaponStoreDataSet.Orders
                    .AsEnumerable()
                    .Average(row => row.Field<decimal>("OverallSum"));

                MessageBox.Show($"Average cost of order: {averageOrderSum:C2}", "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Orders table is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowAvgPrice_Click(object sender, EventArgs e)
        {
            switch (currentTable)
            {
                case "Weapon":
                    ShowAverageWeaponPrice();
                    break;
                case "Orders":
                    ShowAverageOrderSum();
                    break;
                case "Client":
                    ShowClientWithMostOrders();
                    break;
                case "WeaponDetails":
                    ShowAverageLengthByWeaponType();
                    break;
            }

        }

        private void btnGenerateTopCustomersReport_Click(object sender, EventArgs e)
        {
            Document pdfDocument = new Document(PageSize.A4);
            try
            {
                string path = Path.Combine(Application.StartupPath, "top_customers_report.pdf");
                PdfWriter.GetInstance(pdfDocument, new FileStream(path, FileMode.Create));
                pdfDocument.Open();

                var titleFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));
                var regularFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));

                pdfDocument.Add(new Paragraph("Top Customer Report", titleFont));
                pdfDocument.Add(Chunk.NEWLINE);

                var allCustomers = coldWeaponStoreDataSet.Orders
                                       .AsEnumerable()
                                       .GroupBy(o => o.Field<int>("ClientID"))
                                       .Select(group => new
                                       {
                                           ClientID = group.Key,
                                           TotalValue = group.Sum(o => o.Field<decimal>("OverallSum")),
                                           OrderCount = group.Count()
                                       })
                                       .OrderByDescending(x => x.TotalValue)
                                       .ToList();

                foreach (var customer in allCustomers)
                {
                    string clientName = coldWeaponStoreDataSet.Client
                        .AsEnumerable()
                        .FirstOrDefault(c => c.Field<int>("ClientID") == customer.ClientID)?
                        .Field<string>("ClientName") ?? "Unknown";

                    pdfDocument.Add(new Paragraph($" Name: {clientName}, " +
                                                  $"Total Order Value: ${customer.TotalValue:N2}, " +
                                                  $"Orders Count: {customer.OrderCount}", regularFont));
                }

                pdfDocument.Add(new Paragraph($"Generated on: {DateTime.Now:dd-MM-yyyy HH:mm:ss}", regularFont));

                pdfDocument.Close();
                MessageBox.Show($"The report has been saved to: {path}", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (pdfDocument.IsOpen())
                {
                    pdfDocument.Close();
                }
            }
        }

        private void btnGenerateSalesReport_Click_1(object sender, EventArgs e)
        {
            Document pdfDocument = new Document(PageSize.A4);
            try
            {
                string path = Path.Combine(Application.StartupPath, "sales_report.pdf");
                PdfWriter.GetInstance(pdfDocument, new FileStream(path, FileMode.Create));
                pdfDocument.Open();

                var titleFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));
                var regularFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));

                pdfDocument.Add(new Paragraph("Sales Report", titleFont));
                pdfDocument.Add(Chunk.NEWLINE);

                pdfDocument.Add(new Paragraph("1. Total number of sales by weapon type:", regularFont));
                var salesByType = coldWeaponStoreDataSet.WeaponDetails
                            .AsEnumerable()
                            .GroupBy(w => w.Field<string>("WeaponType"))
                            .Select(group => new
                            {
                                Type = group.Key,
                                Count = group.Count()
                            })
                            .ToList();

                foreach (var type in salesByType)
                {
                    pdfDocument.Add(new Paragraph($"{type.Type}: {type.Count}", regularFont));
                }

                pdfDocument.Add(new Paragraph("2. Total monthly revenue:", regularFont));
                var incomeByMonth = coldWeaponStoreDataSet.Orders
                             .AsEnumerable()
                             .GroupBy(o => new { Month = o.Field<DateTime>("Date").Month, Year = o.Field<DateTime>("Date").Year })
                             .Select(group => new
                             {
                                 MonthYear = $"{group.Key.Month}/{group.Key.Year}",
                                 TotalSum = group.Sum(o => o.Field<decimal>("OverallSum"))
                             })
                             .ToList();

                foreach (var month in incomeByMonth.OrderBy(m => m.MonthYear))
                {
                    pdfDocument.Add(new Paragraph($"{month.MonthYear}: ${month.TotalSum:N2}", regularFont));
                }
                pdfDocument.Add(new Paragraph($"Generated on: {DateTime.Now:dd-MM-yyyy HH:mm:ss}", regularFont));

                pdfDocument.Close();
                MessageBox.Show($"The report has been saved to: {path}", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (pdfDocument.IsOpen())
                {
                    pdfDocument.Close();
                }
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            var bindingSource = dataGridView1.DataSource as BindingSource;
            if (bindingSource != null)
            {
                bindingSource.Filter = string.Empty;
                dataGridView1.DataSource = bindingSource;

                textBox1.Text = string.Empty;
                comboBox1.Text = string.Empty;
            }
        }

        private void deleteToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }
            int weaponId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            DialogResult result = MessageBox.Show($"Do you want to delete weapon with id {weaponId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                weaponHistoryTableAdapter.DeleteByWeaponID(weaponId);
                weaponCertificateTableAdapter.DeleteByWeaponID(weaponId);
                weaponDetailsTableAdapter.DeleteByWeaponID(weaponId);
                orderDetailTableAdapter.DeleteByWeaponID(weaponId);
                weaponTableAdapter.DeleteQuery(weaponId);

                weaponTableAdapter.Fill(coldWeaponStoreDataSet.Weapon);
                weaponHistoryTableAdapter.Fill(coldWeaponStoreDataSet.WeaponHistory);
                weaponCertificateTableAdapter.Fill(coldWeaponStoreDataSet.WeaponCertificate);
                weaponDetailsTableAdapter.Fill(coldWeaponStoreDataSet.WeaponDetails);
                orderDetailTableAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);

                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void deleteToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to delete.");
                return;
            }
            int weaponDetailId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            DialogResult result = MessageBox.Show($"Do you want to delete weapon detail with id {weaponDetailId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                weaponDetailsTableAdapter.DeleteQuery(weaponDetailId);
                weaponDetailsTableAdapter.Fill(coldWeaponStoreDataSet.WeaponDetails);

                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = supplierBindingSource;
            bindingNavigator1.BindingSource = supplierBindingSource;
            SetSearchControlsVisibility(true);
            dataGridView1.Refresh();
            bindingNavigator1.Refresh();
            currentTable = "Supplier";
            label1.Text = "Please enter ID of supplier wich details you want to see";
            label2.Text = "Supplier";
            setValuesForSearchCombobox();

        }

        private void addToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            edit = false;
            var form = new Form9();
            form.ShowDialog();
            supplierTableAdapter.Fill(coldWeaponStoreDataSet.Supplier);
            coldWeaponStoreDataSet.AcceptChanges();
        }

        private void updateToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the row to edit.");
                return;
            }
            edit = true;
            var supplierRow = new ColdWeaponStoreDataSet.SupplierDataTable();
            supplierTableAdapter.FillBy(supplierRow, Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            if (supplierRow.Rows.Count > 0)
            {
                object[] row = supplierRow.Rows[0].ItemArray;
                var edt = new Form9(
                    Convert.ToInt32(row[0]), 
                    row[1].ToString(),
                    row[2].ToString(), 
                    row[3].ToString(),
                    row[4].ToString()
                );
                edt.ShowDialog();
                supplierTableAdapter.Fill(coldWeaponStoreDataSet.Supplier); 
                coldWeaponStoreDataSet.AcceptChanges();
            }
            else
            {
                MessageBox.Show("No data was found for the selected Weapon Certificate ID.");
            }
        }

        private void deleteToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            int supplierId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            DialogResult result = MessageBox.Show($"Do you want to delete supplier with id {supplierId}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                var weaponIds = new List<int>();
                string connectionString = "Data Source=DESKTOP-JGN4EB0;Initial Catalog=ColdWeaponStore;Integrated Security=True";
                string query = @"SELECT WeaponID FROM Weapon WHERE SupplierID = @supplierId";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@supplierId", SqlDbType.Int).Value = supplierId;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                weaponIds.Add((int)reader[0]);
                            }
                        }
                    }
                }

                foreach (var weaponId in weaponIds)
                {
                    weaponHistoryTableAdapter.DeleteByWeaponID(weaponId);
                    weaponCertificateTableAdapter.DeleteByWeaponID(weaponId);
                    weaponDetailsTableAdapter.DeleteByWeaponID(weaponId);
                    orderDetailTableAdapter.DeleteByWeaponID(weaponId);
                    weaponTableAdapter.DeleteQuery(weaponId);

                    weaponTableAdapter.Fill(coldWeaponStoreDataSet.Weapon);
                    weaponHistoryTableAdapter.Fill(coldWeaponStoreDataSet.WeaponHistory);
                    weaponCertificateTableAdapter.Fill(coldWeaponStoreDataSet.WeaponCertificate);
                    weaponDetailsTableAdapter.Fill(coldWeaponStoreDataSet.WeaponDetails);
                    orderDetailTableAdapter.Fill(coldWeaponStoreDataSet.OrderDetail);
                }

                supplierTableAdapter.DeleteQuery(supplierId);
                supplierTableAdapter.Fill(coldWeaponStoreDataSet.Supplier);
                coldWeaponStoreDataSet.AcceptChanges();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var column = comboBox1.SelectedItem.ToString();
            var value = textBox1.Text;

            var source = dataGridView1.DataSource as BindingSource;
            if(!string.IsNullOrEmpty(value) && value != null)
            {
                string filterExpression = "";

                if (int.TryParse(value, out _))
                {
                    filterExpression = $"{column} = {value}";
                }
                else
                {
                    filterExpression = $"{column} LIKE '%{value}%'";
                }

                try
                {
                    source.Filter = filterExpression;
                    dataGridView1.DataSource = source;
                }
                catch
                {
                    filterExpression = $"{column} LIKE '%{value}%'";
                    source.Filter = filterExpression;
                    dataGridView1.DataSource = source;
                }
            }
        }
    }
}

