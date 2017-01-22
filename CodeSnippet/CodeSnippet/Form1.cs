using Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeSnippet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpHelper http = new HttpHelper();       
            HttpItem item = new HttpItem()
            {
                URL = "http://wap.chewu8.com/wap/wapAction!editCar.action",//URL     必需项
                //Encoding = null,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                Encoding = Encoding.GetEncoding("utf-8"),
                Method = "POST",//URL     可选项 默认为Get
                Timeout = 100000,//连接超时时间     可选项默认为100000
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
               
                Cookie =this.txtCookie.Text,//字符串Cookie     可选项
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",//    可选项有默认值
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                Referer = "http://wap.chewu8.com/wap/carEdit2.jsp",//来源URL     可选项
                Allowautoredirect = true,//是否根据３０１跳转     可选项
                //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
                Connectionlimit = 1024,//最大连接数     可选项 默认为1024
                Postdata = "param.id=0&param.province=%E4%BA%AC&param.carNumber="+this.txtCarNumber.Text+"&param.carType=02&param.frameNumber=&param.engineNumber="+this.txtEngineNumber.Text+"&param.carpid=14&param.carcid=189&param.cxpid=14&param.cxcid=189",//Post数据     可选项GET时不需要写
                //Postdata = "param.id=0&param.province=%E4%BA%AC&param.carNumber=QZ3V27&param.carType=02&param.frameNumber=&param.engineNumber=vq35749228c&param.carpid=14&param.carcid=189&param.cxpid=14&param.cxcid=189",//Post数据     可选项GET时不需要写
                //PostDataType = PostDataType.FilePath,//默认为传入String类型，也可以设置PostDataType.Byte传入Byte类型数据

                //ProxyIp = "106.46.136.90:808",//代理服务器ID 端口可以直接加到后面以：分开就行了    可选项 不需要代理 时可以不设置这三个参数
               // ProxyPwd = "QH8DDMbC9TPL",//代理服务器密码     可选项
                //ProxyUserName = "hello.01@163.com",//代理服务器账户名     可选项
                ResultType = ResultType.Byte,//返回数据类型，是Byte还是String
                //PostdataByte = System.Text.Encoding.Default.GetBytes("测试一下"),//如果PostDataType为Byte时要设置本属性的值
                //CookieCollection = new System.Net.CookieCollection(),//可以直接传一个Cookie集合进来
                Host = "wap.chewu8.com",
                
            };

            
            item.Header.Add("Origin", "http://wap.chewu8.com");
            item.Header.Add("Upgrade-Insecure-Requests", "1");
           
            //得到HTML代码
            HttpResult result = http.GetHtml(item);
            //取出返回的Cookie
            string cookie = result.Cookie;
            //返回的Html内容
            string html = result.Html;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //表示访问成功，具体的大家就参考HttpStatusCode类
            }
            //表示StatusCode的文字说明与描述
            string statusCodeDescription = result.StatusDescription;
            //把得到的Byte转成图片
           // Image img = byteArrayToImage(result.ResultByte);

            this.textBox1.Text = html;
            
        }
        /// <summary>
        /// 字节数组生成图片
        /// </summary>
        /// <param name="Bytes">字节数组</param>
        /// <returns>图片</returns>
   
        
        private Image byteArrayToImage(byte[] Bytes)
        {
                MemoryStream ms = new MemoryStream(Bytes);
                Image outputImg = Image.FromStream(ms);
                return outputImg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HttpHelper http = new HttpHelper();
          
            HttpItem item = new HttpItem()
            {
                URL = "http://wap.chewu8.com/m/",//URL   
                Method = "get",//URL     可选项 默认为Get  
                //ProxyIp = "106.46.136.90:808",//代理服务器ID 端口可以直接加到后面以：分开就行了    可选项 不需要代理 时可以不设置这三个参数
                //ProxyPwd = "",//代理服务器密码     可选项
                //ProxyUserName = "",//代理服务器账户名     可选项
            };
            //得到HTML代码
            HttpResult result = http.GetHtml(item);
            string cookie = result.Cookie;
            this.txtCookie.Text = cookie;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";

        }

        private void txtAnalysis_Click(object sender, EventArgs e)
        {
            var c = this.textBox1.Text.Trim();
            StringBuilder sb = new StringBuilder();
            if (Lib.RegexHelper.IsMatch(c, "<h2>查询结果</h2>"))
            {
                sb.AppendFormat("查询目标页：OK");



            }
            else
            {
                sb.AppendFormat("查询目标页：Error");
            }
                 
        }

        private void xx()
        {
          
            //html在成功抓取后的后续动作  提取自己想要的数据
            //1，把html里的空格和一些特殊字符全部替换掉
            //2. 准备合适的正则表达式
            //3. 正则准备好后 先把循环体搞出来
            //4，在循环体内 把想要的数据搞出来


            string s = @"<你是tom>，<我是jerry>，<他是韩梅梅，哈哈>";
            string pattern = @"(?<=<tronclick=trClick([1-9]*))([\s\S]*?)(?<adr>())(?=<tdstyle=border-bottom:0px;></td></tr>)";
            var html = this.textBox1.Text.Trim();
            string result = Regex.Replace(html, @"\s", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace("：","");
            MatchCollection matches = Regex.Matches(result, pattern);
            foreach (Match m in matches)
            {               
                var point = "";  //分数
                var money = "";  //钱
                var adr = "";  //发生地址
                var time = ""; //时间
                var behavior = ""; //行为
                string pointreg = @"(?<=id=tdkf1>)([\s\S]*?)(?=</span></td>)";
                Regex regex = new Regex(pointreg);
                if (regex.IsMatch(m.Value))
                {
                    point = regex.Match(m.Value).Value;
                }
                else
                {
                    point = "-1";
                }
                string moneyreg = @"(?<=id=tdfk1>)([\s\S]*?)(?=</span></td>)";
                 regex = new Regex(moneyreg);
              
                if (regex.IsMatch(m.Value))
                {
                    money = regex.Match(m.Value).Value;
                }
                else
                {
                    money = "-1";
                }
                string adrreg = @"(?<=<span>地址)([\s\S]*?)(?=</span>)";
                regex = new Regex(adrreg);
              
                if (regex.IsMatch(m.Value))
                {
                    adr = regex.Match(m.Value).Value;
                }
                else
                {
                    adr = "";
                }
                string timereg = @"(?<=<span>时间)([\s\S]*?)(?=</span>)";
                regex = new Regex(timereg);
             
                if (regex.IsMatch(m.Value))
                {
                    time = regex.Match(m.Value).Value;
                }
                else
                {
                    time = "";
                }
                string behaviorreg = @"(?<=<span>行为)([\s\S]*?)(?=</span>)";
                regex = new Regex(behaviorreg);
              
                if (regex.IsMatch(m.Value))
                {
                    behavior = regex.Match(m.Value).Value;
                }
                else
                {
                    behavior = "";
                }

             
                
            }
           
          
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            xx();
        }



    }
}
