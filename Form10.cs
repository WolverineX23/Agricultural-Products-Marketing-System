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
    public partial class Form10 : Form
    {
        MySqlConnection conn = null;
        string user = "";
        DateTime timePro = new DateTime();//获取产品生产日期
        string pno = "";//生产编号
        string cno;//农产品编号
        string cname = "";
        int yeild = 0;//产量
        int stock = 0;//库存
        int freshday = 0;//农产品保鲜期
        int leave;//剩余保鲜天数
        SubDate sd;
        public Form10(MySqlConnection conn, string user)
        {
            InitializeComponent();
            richTextBox1.AppendText("保鲜期到期提醒(低于原保质期天数的0.2倍):\n");
            richTextBox1.AppendText("农产品\t\t\t库存(kg)\t\t\t剩余保质期(day)\n");

            this.conn = conn;
            this.user = user;
            string name = "";

            //设置label1的值
            conn.Open();
            string sql_tip = $"SELECT TName FROM tradesman WHERE TNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_tip, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            name = reader.GetString("TName");
            reader.Dispose();
            conn.Close();
            label1.Text = "欢迎" + name + "~";

        }

        private void Form10_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form11(conn,user).ShowDialog();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("\t\t\t\t\t   仓库\n");
            richTextBox1.AppendText("生产编号\t农产品\t\t产量(kg)\t\t库存(kg)\t\t生产日期\n");
            conn.Open();
            string sql_storehouse = $"SELECT PNo,CNo,ProdDate,Yeild,FStock FROM plante WHERE FNo = '{user}'";
            MySqlCommand cmd1 = new MySqlCommand(sql_storehouse, conn);
            MySqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                pno = reader.GetString("PNo");
                cno = reader.GetString("CNo");
                timePro = reader.GetDateTime("ProdDate");
                yeild = reader.GetInt32("Yeild");
                stock = reader.GetInt32("FStock");
                int i = 0;
                while (Common.crops[i] != null)
                {
                    if (Common.crops[i].cno.Equals(cno))
                    {
                        freshday = Common.crops[i].freshness;
                        cname = Common.crops[i].cname;
                        break;
                    }
                    i++;
                }
                richTextBox1.AppendText(pno + "\t" + cname + "\t\t" + yeild + "\t\t\t" + stock + "\t\t\t" + timePro.ToShortDateString() + "\n");
            }
            conn.Close();
            reader.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form12(conn, user).ShowDialog();
        }
    }
}
