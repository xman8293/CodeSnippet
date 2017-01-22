using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace BLL
{
    public class RadomAgentBLL
    {


        /// <summary>
        /// *Author：cuip ,Data：2017/1/22
        /// 随机从代理Ip池子内 捞取数据
        /// </summary>
        /// <returns></returns>
        public AgentEntity GetEntityFromList()
        {

            List<AgentEntity> list = new List<AgentEntity>();
            AgentEntity entity = new AgentEntity
            {
                AgentIp = "127.0.0.1",
                Port=80,
                AgentUserName="User",
                AgentPwd="12345",
            };
            var count = list.Count;
            Random ran = new Random();
            int n = ran.Next(0, count);
            return list[n];

        }

    }
}
