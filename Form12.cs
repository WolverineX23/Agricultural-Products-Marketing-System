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
    public partial class Form12 : Form
    {

        MySqlConnection conn = null;
        string user = "";
        public Form12(MySqlConnection conn, string user)
        {
            InitializeComponent();
            this.conn = conn;
            this.user = user;
        }


        private void Form12_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            label5.BackColor = Color.FromArgb(0, 255, 255, 255);
            label6.BackColor = Color.FromArgb(0, 255, 255, 255);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                label2.Text = "请选择农产品！";
                return;
            }
            else
            {
                label2.Text = "";
            }

            int tmp;
            if (!int.TryParse(textBox1.Text, out tmp))
            {
                label6.Text = "请输入数字!";
                return;
            }
            else
            {
                int num = 0;
                num = int.Parse(textBox1.Text);
                if (num <= 0)
                {
                    label6.Text = "请输入正整数";
                    return;
                }
                else
                    label6.Text = "";
            }
        }
    }
}
