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
    public partial class Form11 : Form
    {
        MySqlConnection conn = null;
        string user = "";
        public Form11(MySqlConnection conn,string user)
        {
            InitializeComponent();
            this.conn = conn;
            this.user = user;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form11_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            label5.BackColor = Color.FromArgb(0, 255, 255, 255);
            label6.BackColor = Color.FromArgb(0, 255, 255, 255);
        }
    }
}
