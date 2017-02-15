using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    //驾驶违章的实体
    public class DrivePeccancyEntity
    {
        // 所扣分数
        public string point { get; set; }
        // 所扣金额
        public string money { get; set; }
        // 违章发生地点
        public string adr { get; set; }
        // 违章发生时间
        public string time { get; set; }
        // 违章行为
        public string behavior { get; set; }


    }
}
