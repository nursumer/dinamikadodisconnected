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

namespace dinamikadodisconnected
{
    public partial class Form1 : Form
    {
        //nesneleri tanımladık
        private DataGridView dataGrid1 = new DataGridView();
        private BindingSource bindingSource1= new BindingSource(); // veri data ile baglantı kurmak için
        private SqlDataAdapter dataAdapter= new SqlDataAdapter(); // disconnected kullanırken open ve close yok
        private Button reloadButton= new Button();
        private Button submitButton= new Button();
        public Form1()
        {//butonları ekrana getirme 
            InitializeComponent();
            reloadButton.Text = "Yenileme";
            submitButton.Text = "Kaydet";
                //event ekleme
                reloadButton.Click += new System.EventHandler(reloadButton_Click);
                submitButton.Click += new System.EventHandler(submitButton_Click);
            FlowLayoutPanel panel = new FlowLayoutPanel();  //paneli kullanırsan sıralar lokasyon belirtmene gerek yok 
            panel.Dock = DockStyle.Top;
            panel.AutoSize= true;   //panel büyürse listede büyüsün
            dataGrid1.Dock= DockStyle.Fill; //fullemek
            panel.Controls.AddRange(new Control[] {reloadButton, submitButton}); // panele reloadbutton ve submitbutton ekledik
            this.Controls.AddRange(new Control[] { panel, dataGrid1 }); // forma panel ve datagrid ekledik
            this.Load += new System.EventHandler(Form1_Load);
            this.Text = "Listelenen verilerin yenilenmesi ve kaydedilmei";
        }
        private void reloadButton_Click(object sender, EventArgs e)
        {
            GetData(dataAdapter.SelectCommand.CommandText);

        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            dataAdapter.Update((DataTable)bindingSource1.DataSource); 
            // binding Source datakaynağını datatable ceviriyor. dataadapter ile güncelleme butonuna aktarmış oluyor

        }
        private void GetData(string selectCommand)
        {
            try
            {
                string connectionstring = "Data Source=216-02\\SQLEXPRESS;Initial Catalog=Northwind;User ID=sa; Password=Fbu123456";
                //dataadpterin nereye nasıl bağlanacağını bildirmemiz gerek
                //veritabanındaki tüm bilgi dataadapterde tutulur.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionstring); //veriyi çekiyoruz data adapterden
                SqlCommandBuilder sqlCommandBuilder= new SqlCommandBuilder(dataAdapter);
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table); // dataadapterdekileri table doldur
                bindingSource1.DataSource= table;
                dataGrid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGrid1.RowTemplate.Height = 60; // satırların genişliği
            dataGrid1.DataSource = bindingSource1;
           GetData("Select* from Employees"); // çalışanların listesini getir. içerisinde sql komutu datatadapter de
        }

        //tablo sayısı küçük olana entity framework
        //dopper en çok kullanılan

    }
}
