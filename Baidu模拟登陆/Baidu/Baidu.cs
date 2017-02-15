using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//有问题联系QQ：9997433
namespace Baidu
{
    public class Baidu
    {
        public CookieContainer cookies = new CookieContainer();
        private string token = string.Empty;
        private string verifyStr = string.Empty;
        private string pubksy = string.Empty;
        private string rsakey = string.Empty;
        private string valcode = string.Empty;

        public string GetLoginJs()
        {
            var url = "https://passport.baidu.com/v2/?login";
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem()
                {
                    URL = url,
                    Method = "GET",
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(httpResult.Html);
                var js1Src = doc.DocumentNode.SelectNodes("//script")
                    .Where(s => s.Attributes.FirstOrDefault(a => a.Name.Equals("src")) != null)
                    .FirstOrDefault(
                        s => s.Attributes["src"].Value.Contains("https://passport.baidu.com/passApi/js/wrapper.js"))
                    .Attributes["src"].Value;

                var js1Str = new WebClient().DownloadString(js1Src);
                var mc = new Regex(@"""https:"":""(?<url1>.*?)""}([\s\S]*)login_tangram:""(?<url2>.*?)""").Match(js1Str);
                return mc.Success
                    ? new WebClient().DownloadString(mc.Groups["url1"].Value + mc.Groups["url2"].Value)
                    : string.Empty;
            }

            return string.Empty;
        }

        public bool GetToken()
        {
            var url = "https://passport.baidu.com/v2/api/?getapi&";

            var nvc = new NameValueCollection
            {
                {"apiver", "v3"},
                {"callback", "customName"},
                {"class", ""},
                {"logintype", "basicLogin"},
                {"tpl", "pp"},
                {"tt", DateTime.Now.Ticks.ToString()},
            };
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem()
                {
                    URL = url + HttpHelper.DataToString(nvc),
                    Method = "GET",
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                var mc = new Regex(@"\((?<jsonStr>.*)\)").Match(httpResult.Html.Replace("\n", string.Empty));
                if (mc.Success)
                {
                    dynamic obj = JsonConvert.DeserializeObject<JObject>(mc.Groups["jsonStr"].Value);
                    var no = obj.errInfo.no.Value;
                    if (no.Equals("0"))
                    {
                        token = obj.data.token.Value;
                        return true;
                    }
                }

            }
            return false;
        }

        public bool GetPubksyAndRsakey()
        {
            var url = "https://passport.baidu.com/v2/getpublickey?";
            var nvc = new NameValueCollection
            {
                {"apiver", "v3"},
                {"callback", "customName"},
                {"token", token},
                {"tpl", "pp"},
                {"tt", DateTime.Now.Ticks.ToString()},
            };
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem()
                {
                    URL = url + HttpHelper.DataToString(nvc),
                    Method = "GET",
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                var mc = new Regex(@"\((?<jsonStr>.*)\)").Match(httpResult.Html);
                if (mc.Success)
                {
                    dynamic obj = JsonConvert.DeserializeObject<JObject>(mc.Groups["jsonStr"].Value);
                    var no = obj.errno.Value;
                    if (no.Equals("0"))
                    {
                        pubksy = obj.pubkey.Value;
                        rsakey = obj.key.Value;
                        return true;
                    }
                }
                return true;
            }
            return false;
        }

        public Image GetValCode()
        {
            var url = "https://passport.baidu.com/v2/?reggetcodestr&";
            var nvc = new NameValueCollection
            {
                {"apiver", "v3"},
                {"callback", "customName"},
                {"fr", "login"},
                {"token", token},
                {"tpl", "pp"},
                {"tt", DateTime.Now.Ticks.ToString()},
            };
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem()
                {
                    URL = url + HttpHelper.DataToString(nvc),
                    Method = "GET",
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                var mc = new Regex(@"\((?<jsonStr>.*)\)").Match(httpResult.Html.Replace("\n", string.Empty));
                if (mc.Success)
                {
                    dynamic obj = JsonConvert.DeserializeObject<JObject>(mc.Groups["jsonStr"].Value);
                    var no = obj.errInfo.no.Value;
                    if (no.Equals("0"))
                    {
                        verifyStr = obj.data.verifyStr.Value;
                        var verifySign = obj.data.verifySign.Value;
                        url = "https://passport.baidu.com/cgi-bin/genimage?";
                        httpResult = new HttpHelper().GetHtml(
                            new HttpItem()
                            {
                                URL = url + verifyStr,
                                Method = "GET",
                                CookieContainer = cookies,
                                ResultCookieType = ResultCookieType.CookieContainer,
                                ResultType = ResultType.Byte
                            });
                        if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            return Image.FromStream(new MemoryStream(httpResult.ResultByte));
                        }
                    }

                }
            }
            return null;
        }

        public bool CheckValCode(string code)
        {
            var url = "https://passport.baidu.com/v2/?checkvcode&";
            var nvc = new NameValueCollection
            {
                {"apiver", "v3"},
                {"token", token},
                {"tpl", "pp"},
                {"fr", "login"},
                {"token", token},
                {"tpl", "pp"},
                {"tt", DateTime.Now.Ticks.ToString()},
                {"verifycode", code},
                {"codestring", verifyStr},
                {"callback", "customName"},
            };
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem()
                {
                    URL = url + HttpHelper.DataToString(nvc),
                    Method = "GET",
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                var mc = new Regex(@"\((?<jsonStr>.*)\)").Match(httpResult.Html.Replace("\n", string.Empty));
                if (mc.Success)
                {
                    dynamic obj = JsonConvert.DeserializeObject<JObject>(mc.Groups["jsonStr"].Value);
                    var no = obj.errInfo.no.Value;
                    if (no.Equals("0"))
                    {
                        valcode = code;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Login(string userName, string pswd)
        {
            //pubksy = pubksy.Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
            //.Replace("-----END PUBLIC KEY-----", string.Empty)
            //.Replace("\n", string.Empty);

            //var paser = new Asn1Parser();
            //paser.LoadData(new MemoryStream(Convert.FromBase64String(pubksy)));
            //var modulus =byteToHexStr(paser.RootNode.GetChildNode(1).GetChildNode(0).GetChildNode(0).Data);
            //var exponent = byteToHexStr(paser.RootNode.GetChildNode(1).GetChildNode(0).GetChildNode(1).Data);
            //var rsa = new RSACryptoServiceProvider();
            //rsa.KeySize = 1024;
            //var rsaKeyInfo = new RSAParameters { Modulus = , Exponent = BitConverter.GetBytes(Int32.Parse(exponent, NumberStyles.HexNumber)) };
            //rsa.ImportParameters(rsaKeyInfo);//导入公钥
            //pswd = Convert.ToBase64String(rsa.Encrypt(Encoding.ASCII.GetBytes(pswd), true));//加密
            pswd = GetPswd(pswd, pubksy.Replace("\n", "\\n"));
            var url = "https://passport.baidu.com/v2/api/?login";
            var nvc = new NameValueCollection
            {
                {"apiver", "v3"},
                {"staticpage", "https://passport.baidu.com/static/passpc-account/html/v3Jump.html"},
                {"charset", "UTF-8"},
                {"token", token},
                {"tpl", "pp"},
                {"tt", DateTime.Now.Ticks.ToString()},
                {"codestring", verifyStr},
                {"safeflg", "0"},
                {"u", "https://passport.baidu.com/"},
                {"isPhone", "false"},
                {"detect", "1"},
                {"quick_user", "0"},
                {"logintype", "basicLogin"},
                {"logLoginType", "pc_loginBasic"},
                {"loginmerge", "true"},
                {"username", userName},
                {"password", pswd},
                {"verifycode", valcode},
                {"mem_pass", "on"},
                {"rsakey", rsakey},
                {"crypttype", "12"},
                {"ppui_logintime", "111111"},
                {"callback", "customName"},
            };
            var httpResult = new HttpHelper().GetHtml(
                new HttpItem
                {
                    URL = url,
                    Method = "POST",
                    ContentType = "application/x-www-form-urlencoded",
                    Postdata = HttpHelper.DataToString(nvc),
                    PostDataType = PostDataType.String,
                    CookieContainer = cookies,
                    ResultCookieType = ResultCookieType.CookieContainer
                });
            if (httpResult.StatusCode.Equals(HttpStatusCode.OK))
            {
                return HttpHelper.GetAllCookies(cookies).FirstOrDefault(c => c.Name.Equals("STOKEN")) != null;
            }
            return false;
        }

        public bool Init()
        {
            return GetToken() && GetPubksyAndRsakey();
        }


        public string GetPswd(string pswd, string pubkey)
        {
            var jsStr = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pswd.js"));
            var obj = Type.GetTypeFromProgID("ScriptControl");
            if (obj == null) return string.Empty;
            var scriptControl = Activator.CreateInstance(obj);
            obj.InvokeMember("Language", BindingFlags.SetProperty, null, scriptControl, new object[] {"JScript"});
            obj.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, scriptControl, new object[] {jsStr});
            return obj.InvokeMember("Eval", BindingFlags.InvokeMethod, null, scriptControl, new object[]
            {
                string.Format("PasswordEncrypt('{0}','{1}')", pswd, pubkey)
            }).ToString();
        }
    }
}
