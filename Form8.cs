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
    public partial class Form8 : Form
    {
        MySqlConnection conn;
        String user = "";
        public Form8(MySqlConnection conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            checkBox1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form9(conn).ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string user = textBox1.Text;
            string password;
            string sql_pas = $"select TPassword from tradesman where TNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_pas, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                label5.Text = "该用户名未注册！";
                reader.Dispose();
                conn.Close();
                return;
            }
            label5.Text = "";
            password = reader.GetString("TPassword");
            reader.Dispose();
            conn.Close();
            if (password != textBox2.Text)
            {
                label4.Text = "密码错误！";
                return;
            }
            label4.Text = "";//到此成功登陆
            this.Hide();
            new Form4(conn, user).ShowDialog();
        }

    }
}
