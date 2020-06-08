using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 农产品物流管理系统
{
    class LogInformation
    {
        public string lno, goods, fname, fcontact, fadress, tname, tcontact, tadress;
        public int weight;
        public DateTime timeDeal, timeArrive;

        public LogInformation()
        {
            lno = "";
            goods = "";
            fname = "";
            fcontact = "";
            fadress = "";
            tname = "";
            tcontact = "";
            tadress = "";
            weight = 0;
            timeDeal = new DateTime();
            timeArrive = new DateTime();
        }
    }
}
