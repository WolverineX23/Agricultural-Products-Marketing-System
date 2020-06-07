using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 农产品物流管理系统
{
    class Crops
    {
        public string cno, cname;
        public int freshness;
        public Crops() { }
        public Crops(string cno,string cname,int freshness)
        {
            this.cno = cno;
            this.cname = cname;
            this.freshness = freshness;
        }
    }
}
