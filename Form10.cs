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
        SubDate sd;
        public Form10(MySqlConnection conn, string user)
        {
            InitializeComponent();

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
            reader.Close();
            label1.Text = "欢迎" + name + "~";

            //保鲜期到期/库存提醒
            richTextBox1.AppendText("保鲜期到期/库存不足提醒(低于原保质期天数的0.2倍/库存少于100kg):\n");
            richTextBox1.AppendText("农产品\t\t\t库存(kg)\t\t库存提醒\t\t保质期(day)\t\t剩余保质期(day)\n");
            DateTime timeNow = System.DateTime.Now;
            Tradesman[] freshGroup = new Tradesman[30];
            Tradesman tradesFre = null;
            int i = -1;
            //获取pno数组
            string sql_deal = $"SELECT PNo,TStock FROM deal WHERE TNo = '{user}'";
            cmd = new MySqlCommand(sql_deal, conn);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            while (reader1.Read())
            {
                tradesFre = new Tradesman();
                tradesFre.pno = reader1.GetString("PNo");
                tradesFre.stock = reader1.GetInt32("TStock");

                freshGroup[++i] = tradesFre;
            }
            reader1.Close();

            //获取cno在通过全局常量crops获得cname和fresh信息
            i = 0;
            int j;
            MySqlDataReader reader2 = null;
            string sql_getcno = "";
            while (freshGroup[i] != null)
            {
                sql_getcno = $"SELECT CNo,ProdDate FROM plante WHERE PNo = '{freshGroup[i].pno}'";
                cmd = new MySqlCommand(sql_getcno, conn);
                reader2 = cmd.ExecuteReader();
                reader2.Read();
                freshGroup[i].cno = reader2.GetString("CNo");
                freshGroup[i].timePro = reader2.GetDateTime("ProdDate");

                //获取commom.crops中值
                j = 0;
                while (Common.crops[j] != null)
                {
                    if (Common.crops[j].cno.Equals(freshGroup[i].cno))
                    {
                        freshGroup[i].cname = Common.crops[j].cname;//get cname
                        freshGroup[i].freshness = Common.crops[j].freshness;// get freshness
                        sd = new SubDate(freshGroup[i].timePro, timeNow);
                        freshGroup[i].leave = freshGroup[i].freshness-sd.dateSub();//get剩余保质期
                        break;
                    }
                    j++;
                }
                if (freshGroup[i].leave <= 0)
                {
                    if (freshGroup[i].stock < 100)
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t不足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t已过期\n");
                    }
                    else
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t充足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t已过期\n");
                    }
                    freshGroup[i].isFresh = 1;//过期
                }
                else if (freshGroup[i].leave < freshGroup[i].freshness * 0.2)
                {
                    if (freshGroup[i].stock < 100)
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t不足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t" + freshGroup[i].leave + "\n");
                    }
                    else
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t充足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t" + freshGroup[i].leave + "\n");
                    }
                }
                else { }
                i++;
                reader2.Dispose();
            }
            reader2.Close();

            //UPDATE plante - IsFresh
            string sql_update = "";
            i = 0;
            while(freshGroup[i] != null)
            {
                sql_update = $"UPDATE plante SET IsFresh = '{freshGroup[i].isFresh}' WHERE PNo = '{freshGroup[i].pno}'";
                cmd = new MySqlCommand(sql_update, conn);
                cmd.ExecuteNonQuery();
                i++;
            }

            conn.Close();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            conn.Open();
            MySqlCommand cmd = null;
            //保鲜期到期/库存提醒
            richTextBox1.AppendText("保鲜期到期/库存不足提醒(低于原保质期天数的0.2倍/库存少于100kg):\n");
            richTextBox1.AppendText("农产品\t\t\t库存(kg)\t\t库存提醒\t\t保质期(day)\t\t剩余保质期(day)\n");
            DateTime timeNow = System.DateTime.Now;
            Tradesman[] freshGroup = new Tradesman[30];
            Tradesman tradesFre = null;
            int i = -1;
            //获取pno数组
            string sql_deal = $"SELECT PNo,TStock FROM deal WHERE TNo = '{user}'";
            cmd = new MySqlCommand(sql_deal, conn);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            while (reader1.Read())
            {
                tradesFre = new Tradesman();
                tradesFre.pno = reader1.GetString("PNo");
                tradesFre.stock = reader1.GetInt32("TStock");

                freshGroup[++i] = tradesFre;
            }
            reader1.Close();

            //获取cno在通过全局常量crops获得cname和fresh信息
            i = 0;
            int j;
            MySqlDataReader reader2 = null;
            string sql_getcno = "";
            while (freshGroup[i] != null)
            {
                sql_getcno = $"SELECT CNo,ProdDate FROM plante WHERE PNo = '{freshGroup[i].pno}'";
                cmd = new MySqlCommand(sql_getcno, conn);
                reader2 = cmd.ExecuteReader();
                reader2.Read();
                freshGroup[i].cno = reader2.GetString("CNo");
                freshGroup[i].timePro = reader2.GetDateTime("ProdDate");

                //获取commom.crops中值
                j = 0;
                while (Common.crops[j] != null)
                {
                    if (Common.crops[j].cno.Equals(freshGroup[i].cno))
                    {
                        freshGroup[i].cname = Common.crops[j].cname;//get cname
                        freshGroup[i].freshness = Common.crops[j].freshness;// get freshness
                        sd = new SubDate(freshGroup[i].timePro, timeNow);
                        freshGroup[i].leave = freshGroup[i].freshness - sd.dateSub();//get剩余保质期
                        break;
                    }
                    j++;
                }
                if (freshGroup[i].leave <= 0)
                {
                    if (freshGroup[i].stock < 100)
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t不足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t已过期\n");
                    }
                    else
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t充足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t已过期\n");
                    }
                    freshGroup[i].isFresh = 1;//过期
                }
                else if (freshGroup[i].leave < freshGroup[i].freshness * 0.2)
                {
                    if (freshGroup[i].stock < 100)
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t不足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t" + freshGroup[i].leave + "\n");
                    }
                    else
                    {
                        richTextBox1.AppendText(freshGroup[i].cname + "\t\t\t" + freshGroup[i].stock + "\t\t\t充足\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t" + freshGroup[i].leave + "\n");
                    }
                }
                else { }
                i++;
                reader2.Dispose();
            }
            reader2.Close();
            conn.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
            richTextBox1.AppendText("仓库：\n");
            richTextBox1.AppendText("生产编号\t\t农产品\t\t库存(kg)\t\t保质期(天)\t\t生产日期\n");

            Tradesman[] freshGroup = new Tradesman[30];
            Tradesman tradesFre = null;
            MySqlCommand cmd = null;
            int i = -1;
            conn.Open();
            //获取pno数组
            string sql_deal = $"SELECT PNo,TStock FROM deal WHERE TNo = '{user}'";
            cmd = new MySqlCommand(sql_deal, conn);
            MySqlDataReader reader1 = cmd.ExecuteReader();
            while (reader1.Read())
            {
                tradesFre = new Tradesman();
                tradesFre.pno = reader1.GetString("PNo");
                tradesFre.stock = reader1.GetInt32("TStock");

                freshGroup[++i] = tradesFre;
            }
            reader1.Close();

            //获取cno在通过全局常量crops获得cname和fresh信息
            i = 0;
            int j;
            MySqlDataReader reader2 = null;
            string sql_getcno = "";
            while (freshGroup[i] != null)
            {
                sql_getcno = $"SELECT CNo,ProdDate FROM plante WHERE PNo = '{freshGroup[i].pno}'";
                cmd = new MySqlCommand(sql_getcno, conn);
                reader2 = cmd.ExecuteReader();
                reader2.Read();
                freshGroup[i].cno = reader2.GetString("CNo");
                freshGroup[i].timePro = reader2.GetDateTime("ProdDate");

                //获取commom.crops中值
                j = 0;
                while (Common.crops[j] != null)
                {
                    if (Common.crops[j].cno.Equals(freshGroup[i].cno))
                    {
                        freshGroup[i].cname = Common.crops[j].cname;//get cname
                        freshGroup[i].freshness = Common.crops[j].freshness;// get freshness
                        richTextBox1.AppendText(freshGroup[i].pno + "\t\t" + freshGroup[i].cname + "\t\t" + freshGroup[i].stock + "\t\t\t" +
                            freshGroup[i].freshness + "\t\t\t\t" + freshGroup[i].timePro.ToLongDateString() + "\n");
                        break;
                    }
                    j++;
                }
                i++;
                reader2.Dispose();
            }
            conn.Close();
            reader2.Close();
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
                    ligroup[i].fname + "\t\t" + ligroup[i].fcontact + "\t\t\t" + ligroup[i].faddress + "\t\t" +
                     tname+ "\t\t" + tcontact + "\t\t\t" + taddress + "\t\t\t" +
                    ligroup[i].timeDeal.ToLongDateString() + "\t\t\t" + ligroup[i].timeArrive.ToShortDateString() + "\n");
                i++;
                reader2.Close();
            }

            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string lno = "";
            int id = 0;
            string fno = "";
            string goods = "";
            int weight = 0;
            DateTime timeDeal = new DateTime();
            DateTime timeArrive = new DateTime();
            richTextBox1.Text = "";
            richTextBox1.AppendText("（农户-商户）批发入库记录：\n");
            richTextBox1.AppendText("批发单号\t\t农户账号\t\t农产品\t\t重量(kg)\t\t交易日期\t\t到达日期\n");

            conn.Open();
            string sql_f_t = $"SELECT LNo,FNo,LGoods,Weight,DateTime,ArrivalTime FROM logistics WHERE TNo = '{user}'";
            MySqlCommand cmd = new MySqlCommand(sql_f_t, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lno = reader.GetString("LNo");
                fno = reader.GetString("FNo");
                goods = reader.GetString("LGoods");
                weight = reader.GetInt32("Weight");
                timeDeal = reader.GetDateTime("DateTime");
                timeArrive = reader.GetDateTime("ArrivalTime");
                richTextBox1.AppendText(lno+"\t\t"+fno + "\t\t\t" + goods + "\t\t" + weight + "\t\t\t" + 
                    timeDeal.ToShortDateString() + "\t\t" + timeArrive.ToLongDateString() + "\n");
            }
            reader.Dispose();

            richTextBox1.AppendText("\n(商户-消费者)销售记录：\n");
            richTextBox1.AppendText("消费单号\t\t购买产品\t\t重量(kg)\t\t销售时间\n");
            string sql_t_c = $"SELECT ConNo,Crop,Weight,TimeConsume FROM consume WHERE TNo = '{user}'";
            cmd = new MySqlCommand(sql_t_c, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32("ConNo");
                goods = reader.GetString("Crop");
                weight = reader.GetInt32("Weight");
                timeDeal = reader.GetDateTime("TimeConsume");

                richTextBox1.AppendText(id + "\t\t\t" + goods + "\t\t" + weight + "\t\t\t" + timeDeal.ToLongDateString() + "\n");
            }
            conn.Close();
            reader.Close();
        }
    }
}
