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
    public partial class Form5 : Form
    {
        string Pno;
        string user;
        string Cno;
        MySqlConnection conn;
        public Form5(MySqlConnection conn,string user)
        {
            
            this.conn = conn;
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);

            conn.Open();
            string sql = "select CName from crops ;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add($"{reader.GetString("CName")}");
            }
            reader.Dispose();
            conn.Close();
            

            int f=1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Pno = n.ToString();

            while(f==1)
            {
                conn.Open();
                string sql_isdouble = $"select PNo from plante where PNo= '{Pno}' ;";
                cmd = new MySqlCommand(sql_isdouble, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            label6.Text = Pno;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int f = 1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Pno = n.ToString();

            while (f == 1)
            {
                conn.Open();
                string sql_isdouble = $"select PNo from plante where PNo= '{Pno}' ;";
                MySqlCommand cmd = new MySqlCommand(sql_isdouble, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            label6.Text = Pno;
        }
        
        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql_tosto = $"insert into plante (PNo,FNo,CNo,ProdDate,Yeild(kg),FStock(kg),isFresh) values('{label6.Text}','{user}','{}','{dateTimePicker1.Value.ToString()}','{textBox1.Text}','{textBox1.Text}','0');";
            MySqlCommand cmd = new MySqlCommand(sql_tosto, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
