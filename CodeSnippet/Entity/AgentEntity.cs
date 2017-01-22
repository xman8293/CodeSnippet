using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class AgentEntity
    {
     
        // 代理IP地址
        public string AgentIp { get; set; }
        // 代理端口号
        public int Port { get; set; }
        // 代理服务器登录名
        public string AgentUserName { get; set; }
        // 代理服务器密码
        public string AgentPwd { get; set; }
    }
}
