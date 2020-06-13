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
            string Fno = "";
            string Cno = "";
            int isFresh = 0;
            int stock = 0;
            Boolean isExit = false;
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
            string sql = $"select FNo,IsFresh,FStock from plante where CNo= '{Cno}' ;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                isExit = false;
                isFresh = reader.GetInt32("IsFresh");
                Fno = reader.GetString("FNo");
                stock = reader.GetInt32("FStock");
                if (isFresh == -1 || stock == 0)   //若不新鲜或者无库存，则跳过
                    continue;
                else
                {
                    int tmp = 0;
                    while(fno[tmp] != null)
                    {
                        if(Fno == fno[tmp])
                        {
                            isExit = true;   //若该农户编号已存在在fno[]中，则跳过
                            break;
                        }
                        tmp++;
                    }
                    if (!isExit)
                        fno[count++] = Fno;
                }
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
            reader.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string connetStr = "server=127.0.0.1;port=3306;user=wx;password=wuxiao.04092313; database=cls;";
            string sql = "";
            LogInformation buy_infor = new LogInformation();
            if (comboBox1.SelectedItem == null)
                label8.Text = "请选择农产品！";
            else
                label8.Text = "";
            if (comboBox2.SelectedItem==null)
                label9.Text = "请选择农户！";
            else
                label9.Text = "";

            int tmp;
            if (!int.TryParse(textBox1.Text, out tmp))
                label10.Text = "请输入数字!";
            else
            {
                int num = 0;
                num = int.Parse(textBox1.Text);
                if (num <= 0)
                    label10.Text = "请输入正整数";
                else
                    label10.Text = "";
            }

            SubDate sub = new SubDate(dateTimePicker1.Value,dateTimePicker2.Value);
            if(sub.dateSub()<=0)
                label11.Text = "请输入正确预计日期！";
            else
                label11.Text = "";

            if(label8.Text == "" && label9.Text == "" && label10.Text =="" && label11.Text == "")
            {
                buy_infor.tno = user;
                buy_infor.timeDeal = dateTimePicker1.Value;
                buy_infor.timeArrive = dateTimePicker2.Value;
                buy_infor.lno = textBox2.Text;

                conn.Open();
                //获取FNo
                string sql_fno = $"SELECT FNo FROM farmer WHERE FName = '{comboBox2.SelectedItem}'";
                MySqlCommand cmd = new MySqlCommand(sql_fno,conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                buy_infor.fno = reader.GetString("FNo");
                reader.Dispose();

                //获取cno
                string sql_cno = $"SELECT CNo FROM crops WHERE CName = '{comboBox1.SelectedItem}'";
                cmd = new MySqlCommand(sql_cno, conn);
                reader = cmd.ExecuteReader();
                reader.Read();
                buy_infor.cno = reader.GetString("CNo");
                reader.Dispose();

                //获取农户该农产品总库存
                int total_stock = 0;
                string pno = "";
                string sql_stock = $"SELECT PNo,FStock FROM plante WHERE FNo = '{buy_infor.fno}' AND CNo = '{buy_infor.cno}' AND IsFresh = 0";
                cmd = new MySqlCommand(sql_stock, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    total_stock += reader.GetInt32("FStock");
                }
                reader.Dispose();

                //判断输入重量是否超值，以及影响到的生产批次pno
                buy_infor.weight = Convert.ToInt32(textBox1.Text);
                int weight = buy_infor.weight;
                if (buy_infor.weight > total_stock)
                    MessageBox.Show($"收购失败：该农户库存({total_stock.ToString()}kg)不足，无法提供您所需重量的{comboBox1.SelectedItem.ToString()}!", "收购提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MySqlConnection conn1 = new MySqlConnection(connetStr);
                        MySqlCommand cmd1 = null;
                        pno = reader.GetString("PNo");
                        if (reader.GetInt32("FStock") >= weight)
                        {
                            conn1.Open();
                            //更新plante表中数据
                            sql = $"UPDATE plante SET FStock = {reader.GetInt32("FStock") - weight} WHERE PNo = '{pno}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            //deal表中插入/更新数据
                            sql = $"SELECT TStock FROM deal WHERE TNo = '{buy_infor.tno}' AND PNo = '{pno}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = cmd1.ExecuteReader();
                            if (reader1.Read())
                            {
                                int tstock = reader1.GetInt32("TStock");
                                reader1.Close();
                                sql = $"UPDATE deal SET TStock = {tstock + weight} WHERE PNo = '{pno}' AND TNo = '{buy_infor.tno}'";
                                cmd1 = new MySqlCommand(sql, conn1);
                                cmd1.ExecuteNonQuery();
                            }
                            else
                            {
                                reader1.Close();
                                sql = $"INSERT INTO deal(PNo,TNo,TStock) VALUES('{pno}','{user}',{weight})";
                                cmd1 = new MySqlCommand(sql, conn1);
                                cmd1.ExecuteNonQuery();
                            }

                            //logistics表中插入数据
                            sql = $"INSERT INTO logistics(LNo,FNo,TNo,LGoods,Weight,DateTime,ArrivalTime) VALUES('{buy_infor.lno}','{buy_infor.fno}','{buy_infor.tno}','{comboBox1.SelectedItem}',{buy_infor.weight},'{buy_infor.timeDeal}','{buy_infor.timeArrive}')";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            MessageBox.Show("收购成功!", "收购提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
     
                            conn1.Close();
                            break;
                        }
                        else
                        {
                            conn1.Open();
                            //更新plante表中数据
                            sql = $"UPDATE plante SET FStock = 0 WHERE PNo = '{pno}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            //deal表中插入/更新数据
                            sql = $"SELECT TStock FROM deal WHERE TNo = '{buy_infor.tno}' AND PNo = '{pno}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            MySqlDataReader reader1 = cmd1.ExecuteReader();
                            if (reader1.Read())
                            {
                                int tstock = reader1.GetInt32("TStock");
                                reader1.Close();
                                sql = $"UPDATE deal SET TStock = {tstock + reader.GetInt32("FStock")} WHERE PNo = '{pno}' AND TNo = '{buy_infor.tno}'";
                                cmd1 = new MySqlCommand(sql, conn1);
                                cmd1.ExecuteNonQuery();
                            }
                            else
                            {
                                reader1.Close();
                                sql = $"INSERT INTO deal(PNo,TNo,TStock) VALUES('{pno}','{user}',{reader.GetInt32("FStock")})";
                                cmd1 = new MySqlCommand(sql, conn1);
                                cmd1.ExecuteNonQuery();
                            }
                            conn1.Close();

                            weight -= reader.GetInt32("FStock");
                        }
                    }

                }
                conn.Close();
                reader.Close();
            }
        }
    }
}
