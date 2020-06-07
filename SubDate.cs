using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 农产品物流管理系统
{
    class SubDate
    {
        DateTime dt1, dt2;
        public SubDate() { }
        public SubDate(DateTime dt1,DateTime dt2)
        {
            this.dt1 = dt1;
            this.dt2 = dt2;
        }

        public int dateSub()
        {
            DateTime start = Convert.ToDateTime(dt1.ToShortDateString());
            DateTime end = Convert.ToDateTime(dt2.ToShortDateString());
            TimeSpan sp = end.Subtract(start);

            return sp.Days;
        }
    }
}
