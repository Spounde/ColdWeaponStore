namespace ColdWeaponStore
{
    partial class EditForm3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.orderComboBox = new System.Windows.Forms.ComboBox();
            this.weaponComboBox = new System.Windows.Forms.ComboBox();
            this.coldWeaponStoreDataSet = new ColdWeaponStore.ColdWeaponStoreDataSet();
            this.ordersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ordersTableAdapter = new ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters.OrdersTableAdapter();
            this.weaponBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.weaponTableAdapter = new ColdWeaponStore.ColdWeaponStoreDataSetTableAdapters.WeaponTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.coldWeaponStoreDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(379, 196);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Order ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Weapon ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Amount";
            // 
            // orderComboBox
            // 
            this.orderComboBox.DataSource = this.ordersBindingSource;
            this.orderComboBox.DisplayMember = "OrderID";
            this.orderComboBox.FormattingEnabled = true;
            this.orderComboBox.Location = new System.Drawing.Point(379, 123);
            this.orderComboBox.Name = "orderComboBox";
            this.orderComboBox.Size = new System.Drawing.Size(100, 21);
            this.orderComboBox.TabIndex = 8;
            // 
            // weaponComboBox
            // 
            this.weaponComboBox.DataSource = this.weaponBindingSource;
            this.weaponComboBox.DisplayMember = "WeaponID";
            this.weaponComboBox.FormattingEnabled = true;
            this.weaponComboBox.Location = new System.Drawing.Point(379, 162);
            this.weaponComboBox.Name = "weaponComboBox";
            this.weaponComboBox.Size = new System.Drawing.Size(100, 21);
            this.weaponComboBox.TabIndex = 9;
            // 
            // coldWeaponStoreDataSet
            // 
            this.coldWeaponStoreDataSet.DataSetName = "ColdWeaponStoreDataSet";
            this.coldWeaponStoreDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ordersBindingSource
            // 
            this.ordersBindingSource.DataMember = "Orders";
            this.ordersBindingSource.DataSource = this.coldWeaponStoreDataSet;
            // 
            // ordersTableAdapter
            // 
            this.ordersTableAdapter.ClearBeforeFill = true;
            // 
            // weaponBindingSource
            // 
            this.weaponBindingSource.DataMember = "Weapon";
            this.weaponBindingSource.DataSource = this.coldWeaponStoreDataSet;
            // 
            // weaponTableAdapter
            // 
            this.weaponTableAdapter.ClearBeforeFill = true;
            // 
            // EditForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.weaponComboBox);
            this.Controls.Add(this.orderComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Name = "EditForm3";
            this.Text = "Form4";
            this.Load += new System.EventHandler(this.EditForm3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.coldWeaponStoreDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox orderComboBox;
        private System.Windows.Forms.ComboBox weaponComboBox;
        private ColdWeaponStoreDataSet coldWeaponStoreDataSet;
        private System.Windows.Forms.BindingSource ordersBindingSource;
        private ColdWeaponStoreDataSetTableAdapters.OrdersTableAdapter ordersTableAdapter;
        private System.Windows.Forms.BindingSource weaponBindingSource;
        private ColdWeaponStoreDataSetTableAdapters.WeaponTableAdapter weaponTableAdapter;
    }
}