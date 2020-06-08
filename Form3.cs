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
    public partial class Form3 : Form
    {
        MySqlConnection conn;
        public Form3(MySqlConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            conn.Open();
            string sql = "select district_name from district where district_sqe LIKE '.1._.' or district_sqe LIKE '.1.__.'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add($"{reader.GetString("district_name")}");
            }
            reader.Dispose();
            conn.Close();
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            label6.BackColor = Color.FromArgb(0, 255, 255, 255);
            label7.BackColor = Color.FromArgb(0, 255, 255, 255);
            label8.BackColor = Color.FromArgb(0, 255, 255, 255);
            label9.BackColor = Color.FromArgb(0, 255, 255, 255);
            label10.BackColor = Color.FromArgb(0, 255, 255, 255);
            label11.BackColor = Color.FromArgb(0, 255, 255, 255);
            checkBox1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
                textBox3.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string address="";
            string user="";
            string name="";
            string phone="";
            string password="";
            conn.Open();//验重
            string sql_repeat = $"select FNo from farmer where FNo = '{textBox1.Text}'";
            MySqlCommand cmd = new MySqlCommand(sql_repeat, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                label6.Text="账号已被注册！";
            }
            else
            {
                label6.Text = "";
                user = textBox1.Text;
            }
            reader.Dispose();
            conn.Close();

            if(textBox6.Text.Length!=11||textBox6.Text.Substring(0,1)!="1")//验手机号
            {
                label11.Text = "格式错误！";
            }
            else
            {
                label11.Text = "";
                phone = textBox6.Text;
            }

            if (textBox2.Text!=textBox3.Text)//验密码
            {
                label7.Text = "两次密码不一致！";
            }
            else
            {
                label7.Text = "";
                password = textBox2.Text;
            }
            if(comboBox1.SelectedItem==null|| comboBox2.SelectedItem==null|| textBox5.Text=="")
            {
                label13.Text = "请输入地址";
            }
            else 
            {
                label13.Text = "";
                address = comboBox1.SelectedItem.ToString() +"省"+ comboBox2.SelectedItem.ToString() +"市"+ textBox5.Text;
            }
            
            name = textBox4.Text;

            if (label6.Text == "" && label7.Text == "" && label11.Text == ""&&label13.Text=="")//无注册提示错误信息
            {
                conn.Open();
                string sql_regi = $"insert into farmer(FNo,FPassword,FName,FAdress,FContact) values('{user}','{password}','{name}','{address}','{phone}');";
                cmd = new MySqlCommand(sql_regi, conn);
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("注册成功！", "注册提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                this.Close();
            }
            else
            {
                MessageBox.Show("注册失败，请输入正确的信息！", "注册提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string selected;
            string point;
            conn.Open();
            selected = comboBox1.SelectedItem.ToString();
            string sql = $"select district_sqe from district where district_name ='{selected}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            point = reader.GetString("district_sqe");
            reader.Dispose();
            sql = $"select district_name from district where district_sqe like '{point}_.' OR district_sqe like '{point}__.'OR district_sqe like '{point}___.'";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add($"{reader.GetString("district_name")}");
            }
            reader.Dispose();
            conn.Close();
        }
    }
}
