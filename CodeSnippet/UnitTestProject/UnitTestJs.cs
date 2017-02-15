using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using MSScriptControl;
namespace UnitTestProject
{
    [TestClass]
    public class UnitTestJs
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
         [TestMethod]
        public void execJS()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("retrun 1;");
            ScriptControlClass js = new ScriptControlClass();
            js.Language = "javascript";
            var r=js.Eval("var w=10;var t=2; p=w/t;").ToString();

        }
    }
}
