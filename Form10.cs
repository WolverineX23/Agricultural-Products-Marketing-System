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
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("仓库：\n");
            richTextBox1.AppendText("生产编号\t农产品\t\t库存(kg)\t\t保质期(天)\t\t生产日期\n");
            /*conn.Open();
            string sql_tradeshouse = $"SELECT PNo,CNo,ProdDate,Yeild,FStock FROM plante WHERE FNo = '{user}'";
            MySqlCommand cmd1 = new MySqlCommand(sql_tradeshouse, conn);
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
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form12(conn, user).ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Form11(conn, user).ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox1.AppendText("历史物流信息：\n");
            richTextBox1.AppendText("物流单号\t\t\t物品\t\t\t重量(kg)\t\t\t发货人\t\t联系电话\t\t\t\t地址\t\t\t\t\t\t\t收货人\t\t联系电话\t\t\t\t地址\t\t\t\t\t\t\t\t交易日期\t\t\t\t签收日期\n");
            LogInformation[] ligroup = new LogInformation[20];
            LogInformation li = null;
            int i = -1;

            conn.Open();
            string sql_log = $"SELECT LNo,FNo,LGoods,Weight,DateTime,ArrivalTime FROM logistics WHERE TNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_log, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            //添加基本信息
            while (reader.Read())
            {
                li = new LogInformation();
                li.lno = reader.GetString("LNo");
                li.fno = reader.GetString("FNo");
                li.goods = reader.GetString("LGoods");
                li.weight = reader.GetInt32("Weight");
                li.timeDeal = reader.GetDateTime("DateTime");
                li.timeArrive = reader.GetDateTime("ArrivalTime");

                ligroup[++i] = li;//往数组中添加该对象
            }
            reader.Close();

            //添加商户信息
            string tname, taddress, tcontact;
            string sql_tradesman = $"SELECT TName,TAddress,TContact FROM tradesman WHERE TNo = '{user}'";
            cmd = new MySqlCommand(sql_tradesman, conn);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            reader1.Read();
            tname = reader1.GetString("TName");
            taddress = reader1.GetString("TAddress");
            tcontact = reader1.GetString("TContact");
            reader1.Close();

            //添加农户信息
            i = 0;
            string sql_farmer = "";
            MySqlDataReader reader2 = null;
            while (ligroup[i] != null)
            {
                sql_farmer = $"SELECT FName,FAddress,FContact FROM farmer WHERE FNo = '{ligroup[i].fno}'";
                cmd = new MySqlCommand(sql_farmer, conn);
                reader2 = cmd.ExecuteReader();
                reader2.Read();
                ligroup[i].fname = reader2.GetString("FName");
                ligroup[i].faddress = reader2.GetString("FAddress");
                ligroup[i].fcontact = reader2.GetString("FContact");

                richTextBox1.AppendText(ligroup[i].lno + "\t\t\t" + ligroup[i].goods + "\t\t" + ligroup[i].weight + "\t\t\t\t" +
                    tname + "\t\t" + tcontact + "\t\t\t" + taddress + "\t\t" +
                    ligroup[i].fname + "\t\t" + ligroup[i].fcontact + "\t\t\t" + ligroup[i].faddress + "\t\t\t" +
                    ligroup[i].timeDeal.ToLongDateString() + "\t\t\t" + ligroup[i].timeArrive.ToShortDateString() + "\n");
                i++;
                reader2.Close();
            }

            conn.Close();
        }
    }
}
