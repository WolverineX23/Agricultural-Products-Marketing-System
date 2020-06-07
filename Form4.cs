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
        DateTime timePro = new DateTime();//获取产品生产日期
        string pno="";//生产编号
        string cno;//农产品编号
        string cname = "";
        int yeild = 0;//产量
        int stock = 0;//库存
        int freshday = 0;//农产品保鲜期
        int leave;//剩余保鲜天数
        SubDate sd;
        public Form4(MySqlConnection conn,string user)
        {
            InitializeComponent();
            richTextBox1.AppendText("保鲜期到期提醒(低于原保质期天数的0.2倍):\n");
            richTextBox1.AppendText("农产品\t\t\t库存(kg)\t\t\t剩余保质期(day)\n");

            this.conn = conn;
            this.user = user;
            string name = "";

            //设置label1的值
            conn.Open();
            string sql_tip = $"SELECT FName FROM farmer WHERE FNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_tip, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            name = reader.GetString("FName");
            reader.Dispose();
            conn.Close();
            label1.Text = "欢迎" + name + "~";

            //过期智能提醒
            DateTime timeNow = System.DateTime.Now;//获取当前时间

            conn.Open();
            string sql_date = $"SELECT CNo,ProdDate,FStock FROM plante WHERE FNo = '{user}'";
            MySqlCommand cmd1 = new MySqlCommand(sql_date, conn);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                cno = reader1.GetString("CNo");
                timePro = reader1.GetDateTime("ProdDate");
                stock = reader1.GetInt32("FStock");
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
                sd = new SubDate(timePro, timeNow);
                leave = freshday - sd.dateSub();//计算剩余天数
                if (leave > freshday * 0.2) { }
                else if (leave <= 0)
                    richTextBox1.AppendText(cname + "\t\t\t" + stock + "\t\t\t\t已过期\n");
                else
                    richTextBox1.AppendText(cname + "\t\t\t" + stock + "\t\t\t\t" + leave + "\n");
            }
            conn.Close();
            reader1.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form5(conn,user).ShowDialog();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("\t\t\t\t\t   仓库\n");
            richTextBox1.AppendText("生产编号\t农产品\t产量(kg)\t\t库存(kg)\t\t生产日期\n");
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
                richTextBox1.AppendText(pno + "\t" + cname + "\t" + yeild + "\t\t\t" + stock + "\t\t\t" + timePro.ToShortDateString() + "\n");
            }
            conn.Close();
            reader.Close();
        }
    }
}
