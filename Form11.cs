using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K4os.Compression.LZ4.Streams;
using MySql.Data.MySqlClient;

namespace 农产品物流管理系统
{
    public partial class Form11 : Form
    {
        string Lno;
        MySqlConnection conn = null;
        string user = "";
        public Form11(MySqlConnection conn,string user)
        {
            InitializeComponent();
            this.conn = conn;
            this.user = user;
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
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form11_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            label5.BackColor = Color.FromArgb(0, 255, 255, 255);
            label6.BackColor = Color.FromArgb(0, 255, 255, 255);
            label7.BackColor = Color.FromArgb(0, 255, 255, 255);
            label8.BackColor = Color.FromArgb(0, 255, 255, 255);
            label9.BackColor = Color.FromArgb(0, 255, 255, 255);
            label10.BackColor = Color.FromArgb(0, 255, 255, 255);
            label11.BackColor = Color.FromArgb(0, 255, 255, 255);
            

            int f = 1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Lno = n.ToString();

            while (f == 1)
            {
                conn.Open();
                string sql_isdouble = $"select LNo from logistics where LNo= '{Lno}' ;";
                MySqlCommand cmd = new MySqlCommand(sql_isdouble, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ran = new Random();
                    n = ran.Next(100000000);
                    Lno = n.ToString();
                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            textBox2.Text = Lno;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int f = 1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Lno = n.ToString();

            while (f == 1)
            {
                conn.Open();
                string sql_isdouble = $"select LNo from logistics where LNo= '{Lno}' ;";
                MySqlCommand cmd = new MySqlCommand(sql_isdouble, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ran = new Random();
                    n = ran.Next(100000000);
                    Lno = n.ToString();
                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            textBox2.Text = Lno;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int count = 0;
            string Fno, Cno = "";
            string[] fno = new string[100];
            comboBox2.Items.Clear();
            foreach (Crops crop in Common.crops)
            {
                if (comboBox1.SelectedItem.ToString() == crop.cname)
                {
                    Cno = crop.cno;
                }
            }
            conn.Open();
            string sql = $"select FNo from plante where CNo= '{Cno}' ;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fno[count] = reader.GetString("FNo");
                count++;
            }//获取FNO
            reader.Dispose();
            for (int i = 0; i < count; i++)
            {
                string sql_getname = $"select FName from farmer where FNo= '{fno[i]}' ;";
                cmd = new MySqlCommand(sql_getname, conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                comboBox2.Items.Add($"{reader.GetString("FName")}");
                reader.Dispose();
            }
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                label8.Text = "请选择农产品！";
                return;
            }
            else
            {
                label8.Text = "";
            }
            if (comboBox2.SelectedItem==null)
            {
                label9.Text = "请选择农户！";
                return;
            }
            else
            {
                label9.Text = "";
            }


            int tmp;
            if (!int.TryParse(textBox1.Text, out tmp))
            {
                label10.Text = "请输入数字!";
                return;
            }
            else
            {
                int num = 0;
                num = int.Parse(textBox1.Text);
                if (num <= 0)
                {
                    label10.Text = "请输入正整数";
                    return;
                }
                else
                    label10.Text = "";
            }
            SubDate sub = new SubDate(dateTimePicker1.Value,dateTimePicker2.Value);
            if(sub.dateSub()<=0)
            {
                label11.Text = "请输入正确预计日期！";
                return;
            }
            else
            {
                label11.Text = "";
            }

        }
    }
}
