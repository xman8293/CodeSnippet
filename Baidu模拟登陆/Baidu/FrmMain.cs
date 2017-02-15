using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Baidu
{
    //有问题联系QQ：9997433
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        Baidu baidu = new Baidu();
        private void button1_Click_1(object sender, EventArgs e)
        {
            label1.Text = baidu.Init() ? "初始化成功！" : "初始化失败！";

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var img = baidu.GetValCode();
            label1.Text = img == null ? "获取验证码失败！" : "获取验证码成功！";
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ret = string.IsNullOrWhiteSpace(textBox1.Text.Trim());
            label1.Text = ret ? "请输入验证码！" : string.Empty;
            if (ret) return;
            ret = baidu.CheckValCode(textBox1.Text.Trim());
            label1.Text = ret ? string.Empty : "验证码输入不正确";
            if (!ret)
            {
                button2_Click_1(null, null);
                return;
            }
            ret = baidu.Login(textBox2.Text.Trim(), textBox3.Text.Trim());
            label1.Text = ret ? "登录成功！" : "登录失败！";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HttpHelper.StartIe("http://pan.baidu.com/",baidu.cookies);
        }

    }
}
