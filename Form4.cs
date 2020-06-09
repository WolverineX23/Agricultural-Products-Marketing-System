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
        string user = "";//农户编号--fno
        DateTime timePro = new DateTime();//获取产品生产日期
        string pno="";//生产编号
        string cno;//农产品编号
        string cname = "";
        int yeild = 0;//产量
        int stock = 0;//库存
        int freshday = 0;//农产品保鲜期
        int leave;//剩余保鲜天数

        //交易物流
        string tno = "";//商户编号
        string tname = "";//商户姓名
        string goods = "";//交易物资
        int weight=0;//交易重量
        DateTime timeDeal = new DateTime();//交易时间

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

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("历史物流信息：\n");
            richTextBox1.AppendText("物流单号\t\t\t物品\t\t\t重量(kg)\t\t\t发货人\t\t联系电话\t\t\t\t地址\t\t\t\t\t\t\t收货人\t\t联系电话\t\t\t\t地址\t\t\t\t\t\t\t\t交易日期\t\t\t\t签收日期\n");
            LogInformation[] ligroup = new LogInformation[20];
            LogInformation li = null;
            int i = -1;

            conn.Open();
            string sql_log = $"SELECT LNo,TNo,LGoods,Weight,DateTime,ArrivalTime FROM logistics WHERE FNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_log,conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            //添加基本信息
            while (reader.Read())
            {
                li = new LogInformation();
                li.lno = reader.GetString("LNo");
                li.tno = reader.GetString("TNo");
                li.goods = reader.GetString("LGoods");
                li.weight = reader.GetInt32("Weight");
                li.timeDeal = reader.GetDateTime("DateTime");
                li.timeArrive = reader.GetDateTime("ArrivalTime");

                ligroup[++i] = li;//往数组中添加该对象
            }
            reader.Close();

            //添加农户信息
            string fname, faddress, fcontact;
            string sql_farmer = $"SELECT FName,FAddress,FContact FROM farmer WHERE FNo = '{user}'";
            cmd = new MySqlCommand(sql_farmer, conn);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            reader1.Read();
            fname = reader1.GetString("FName");
            faddress = reader1.GetString("FAddress");
            fcontact = reader1.GetString("FContact");
            reader1.Close();

            //添加商户信息
            i = 0;
            string sql_tradesman = "";
            MySqlDataReader reader2 = null;
            while (ligroup[i] != null)
            {
                sql_tradesman = $"SELECT TName,TAddress,TContact FROM tradesman WHERE TNo = '{ligroup[i].tno}'";
                cmd = new MySqlCommand(sql_tradesman, conn);
                reader2 = cmd.ExecuteReader();
                reader2.Read();
                ligroup[i].tname = reader2.GetString("TName");
                ligroup[i].taddress = reader2.GetString("TAddress");
                ligroup[i].tcontact = reader2.GetString("TContact");

                richTextBox1.AppendText(ligroup[i].lno + "\t\t\t" + ligroup[i].goods + "\t\t" + ligroup[i].weight + "\t\t\t\t" + 
                    fname + "\t\t" + fcontact + "\t\t\t" + faddress + "\t\t" + 
                    ligroup[i].tname + "\t\t" + ligroup[i].tcontact + "\t\t\t" + ligroup[i].taddress + "\t\t\t" +
                    ligroup[i].timeDeal.ToLongDateString() + "\t\t\t" + ligroup[i].timeArrive.ToShortDateString() + "\n");
                i++;
                reader2.Close();
            }

            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("保鲜期到期提醒(低于原保质期天数的0.2倍):\n");
            richTextBox1.AppendText("农产品\t\t\t库存(kg)\t\t\t剩余保质期(day)\n");

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

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("交易记录：\n");
            richTextBox1.AppendText("批发商\t\t农产品\t\t重量(kg)\t\t交易日期\n");

            conn.Open();
            string sql_log = $"SELECT TNo,LGoods,Weight,DateTime FROM logistics WHERE FNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_log, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tno = reader.GetString("TNo");
                goods = reader.GetString("LGoods");
                weight = reader.GetInt32("Weight");
                timeDeal = reader.GetDateTime("DateTime");

                /*string sql_tname = $"SELECT TName FROM tradesman WHERE TNo = '{tno}'";
                MySqlCommand cmd1 = new MySqlCommand(sql_tname, conn);
                MySqlDataReader reader1 = cmd1.ExecuteReader();
                tname = reader1.GetString("TName");*/

                richTextBox1.AppendText(tno + "\t\t\t" + goods + "\t\t" + weight + "\t\t\t" + timeDeal.ToShortDateString() + "\n");
            }
            conn.Close();
            reader.Close();
        }
    }
}
