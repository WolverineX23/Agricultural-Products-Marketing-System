using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace 农产品物流管理系统
{
    public partial class Form4 : Form
    {
        MySqlConnection conn = null;
        string user = "";
        public Form4(MySqlConnection conn,string user)
        {
            InitializeComponent();
            this.conn = conn;
            this.user = user;
            string name = "";

            conn.Open();
            string sql_tip = $"SELECT FName FROM farmer WHERE FNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_tip, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            name = reader.GetString("FName");
            reader.Dispose();
            conn.Close();
            label1.Text = "欢迎" + name + "~";
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form5().ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
