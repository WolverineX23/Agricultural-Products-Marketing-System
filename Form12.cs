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

            conn.Open();
            string sql = $"SELECT CNo FROM deal,plante WHERE deal.PNo = plante.PNo AND TNo = '{user}' AND TStock > 0 AND plante.IsFresh = 0;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            string[] cno = new string[10];
            string CNo = "";
            int count = 0;
            Boolean isExist = false;
            while (reader.Read())
            {
                isExist = false;
                CNo = reader.GetString("CNo");

                int temp = 0;
                while(cno[temp] != null)
                {
                    if(cno[temp] == CNo)//判断cno是否已存在
                    {
                        isExist = true;
                        break;
                    }
                    temp++;
                }

                if(isExist == false)
                {
                    cno[count++] = CNo;//写至cno中，用于下个循环

                    int i = 0;
                    while (Common.crops[i] != null)
                    {
                        if (Common.crops[i].cno == CNo)
                        {
                            comboBox2.Items.Add(Common.crops[i].cname);//写入comboBox中
                            break;
                        }
                        i++;
                    }
                }
            }
            conn.Close();
            reader.Close();
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
                label2.Text = "请选择农产品！";
            else
                label2.Text = "";

            int tmp;
            if (!int.TryParse(textBox1.Text, out tmp))
                label6.Text = "请输入数字!";
            else
            {
                int num = 0;
                num = int.Parse(textBox1.Text);
                if (num <= 0)
                    label6.Text = "请输入正整数";
                else
                    label6.Text = "";
            }

            if(label2.Text == "" && label6.Text == "")
            {
                string connetStr = "server=127.0.0.1;port=3306;user=wx;password=wuxiao.04092313; database=cls;";
                DateTime timeConsume = dateTimePicker1.Value;
                //先获取所选取农产品的CNo
                string cno = "";
                int i = 0;
                while (Common.crops[i] != null)
                {
                    if (Common.crops[i].cname.Equals(comboBox2.SelectedItem.ToString()))
                    {
                        cno = Common.crops[i].cno;
                        break;
                    }
                    i++;
                }

                //判断输入的weight
                int weight = Convert.ToInt32(textBox1.Text);
                int temp = weight;
                int total_tstock = 0;//商户该农产品总库存
                string pno = "";
                conn.Open();
                string sql = $"SELECT TStock,plante.PNo FROM deal,plante WHERE deal.PNo = plante.PNo AND TNo = '{user}' AND CNo = '{cno}' AND IsFresh = 0";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    total_tstock += reader.GetInt32("TStock");
                }//获得总库存
                reader.Dispose();

                if(total_tstock < weight)
                {
                    MessageBox.Show($"数据保存失败：该商户库存：{total_tstock.ToString()}kg，无法提供您所输入的重量：{weight}kg!", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MySqlConnection conn1 = new MySqlConnection(connetStr);
                        MySqlCommand cmd1 = null;
                        pno = reader.GetString("PNo");

                        if(temp <= reader.GetInt32("TStock"))
                        {
                            conn1.Open();
                            //更新deal表中TStock值
                            sql = $"UPDATE deal SET TStock = '{reader.GetInt32("TStock") - temp}' WHERE PNo = '{pno}' AND TNo = '{user}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            //向consume表中插入信息
                            sql = $"INSERT INTO consume(TNo,Crop,Weight,TimeConsume) VALUES('{user}','{comboBox2.SelectedItem}',{weight},'{timeConsume}')";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            MessageBox.Show("消费信息添加成功!", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                            conn1.Close();
                            break;
                        }
                        else
                        {
                            conn1.Open();
                            temp -= reader.GetInt32("TStock");

                            //更新deal表中TStock值
                            sql = $"UPDATE deal SET TStock = 0 WHERE PNo = '{pno}' AND TNo = '{user}'";
                            cmd1 = new MySqlCommand(sql, conn1);
                            cmd1.ExecuteNonQuery();

                            conn1.Close();
                        }
                    }
                }
                conn.Close();
                reader.Close();
            }
        }
    }
}
