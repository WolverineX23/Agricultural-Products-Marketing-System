using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 农产品物流管理系统
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label1.BackColor = Color.FromArgb(0, 255, 255, 255);

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
        }

    }
}
