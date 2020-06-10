using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 农产品物流管理系统
{
    public class Tradesman
    {
        public string tno, tpassword, tname, tadress, tcontact, pno, cno, cname;
        public DateTime timePro;
        public int stock, freshness, leave,isFresh;
        public Tradesman()
        {
            tno = "";
            tpassword = "";
            tname = "";
            tadress = "";
            tcontact = "";
            pno = "";
            stock = 0;
            cno = "";
            cname = "";
            freshness = 0;
            leave = 0;
            isFresh = 0;
            timePro = new DateTime();
        }
    }
}
