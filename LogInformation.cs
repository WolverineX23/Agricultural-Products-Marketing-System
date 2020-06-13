using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 农产品物流管理系统
{
    class LogInformation
    {
        public string lno, goods, fno, tno, fname, fcontact, faddress, tname, tcontact, taddress, cno;
        public int weight;
        public DateTime timeDeal, timeArrive;

        public LogInformation()
        {
            lno = "";
            fno = "";
            tno = "";
            cno = "";

            goods = "";
            fname = "";
            fcontact = "";
            faddress = "";
            tname = "";
            tcontact = "";
            taddress = "";
            weight = 0;
            timeDeal = new DateTime();
            timeArrive = new DateTime();
        }
    }
}
