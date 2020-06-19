using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyWebServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Socket listenSocket;
        private void btnStart_Click(object sender, EventArgs e)
        {
            //在服务端创建了一个负责监听的Socket
            listenSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);//Tcp
            IPAddress ipAddress = IPAddress.Parse(this.txtIpAddress.Text);//获取IP地址
            IPEndPoint ipEndPoint=new IPEndPoint(ipAddress,Convert.ToInt32(this.txtPort.Text));//获取端口号
            listenSocket.Bind(ipEndPoint); //监听的Socket绑定端口号

            listenSocket.Listen(10);//查看一下数据有没有发送过来！listenSocket可以同时处理10个请求
            Thread mThread = new Thread(BeginAccept) {IsBackground = true};
            mThread.Start();
        }

        private void BeginAccept()
        {
            //死循环：负责监听的Socket一致要等待客户端数据
            while (true)//死循环的用处：该方法不能执行一次就结束，每个客户端发送的请求都创建一个socket，负责与客户端通信！
            {
                Socket newSocket = listenSocket.Accept();  //容易阻塞主线程，通过开启线程的方式解决这个问题"Accept 接受/同意/接收"
                HttpApplication httpApplication=new HttpApplication(newSocket,ShowMsg);//将socket交给Http处理器
            }
        }
        private void ShowMsg(String msg)
        {
            this.txtContent.AppendText(msg+"\r\n");
        }
    }
}
