using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyWebServer
{
    /// <summary>
    /// 完成客户端发送过来的数据的处理！
    /// </summary>
    public class HttpApplication
    {
        private Socket socket = null;
        private DGShowMsg dgShowMsg = null;

        //构造方法，在该类创建的时候，执行
        public HttpApplication(Socket socket,DGShowMsg dgShowMsg)
        {
            //01_获取到socket和委托(签名一样的方法)
            this.socket = socket;
            this.dgShowMsg = dgShowMsg;

            //02_接收客户端发送过来的数据
            byte[] buffer=new byte[1024*1024*2];       //定义一个字节数组
            int receiveLength = socket.Receive(buffer);//接收客户端发送过来的数据，返回的是实际接收的数据的长度。
           
            //03_判断接收到的数据是否有值
            if (receiveLength>0)
            {
                //此时拿到了发送的socket请求信息
                string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveLength);//将二进制信息解析成string字符串
                //对请求报文进行处理，此时可以得到要访问文件的名称
                HttpRequst requst = new HttpRequst(msg);
                //此时会返回响应头  和  响应体
                ProcessRequest(requst);

                dgShowMsg(msg);
            }
           
        }

        public void ProcessRequest(HttpRequst requst)
        {
            //01_得到要访问文件的名称
            string filePath = requst.FilePath;

            //02_01获取当前服务器程序的运行目录，获取物理路径
            //     记得将要处理的文件放在bin运行目录下，运行时复制，可访问到
            string dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (dataDirectory.EndsWith(@"\bin\Debug\") || dataDirectory.EndsWith(@"\bin\Release\"))
            {
                dataDirectory = System.IO.Directory.GetParent(dataDirectory).Parent.Parent.FullName;
            }
            //03_此时拿到文件完整路径
            string fullDirectory = dataDirectory + filePath; 


            //04_通过文件流解析文件内容
            using (FileStream fileStream=new FileStream(fullDirectory,FileMode.Open,FileAccess.Read))
            {
                //将解析的内容转成二进制
                byte[] buffer=new byte[fileStream.Length];
                int readbuffer = fileStream.Read(buffer, 0, buffer.Length);

                //构建响应报文
                HttpResponse response=new HttpResponse(buffer,filePath);
                socket.Send(response.GetHeaderResponse());//返回响应头，此时是重点
                //将二进制数据返回！
                socket.Send(buffer);                            //返回响应体，此时是重点
            }
        }
    }
}
