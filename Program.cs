using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace 农产品物流管理系统
{
    class Common {
        public static Crops[] crops = new Crops[9] {
                new Crops("C001","西兰花",15),
                new Crops("C002","娃娃菜",30),
                new Crops("C003","大  米",365),
                new Crops("C004","西红柿",10),
                new Crops("C005","竹  笋",150),
                new Crops("C006","茭  白",60),
                new Crops("C007","土  豆",180),
                new Crops("C008","生  菜",5),
                new Crops("C009","四季豆",7)
     };
    }


    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
