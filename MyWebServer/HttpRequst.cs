using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public class HttpRequst
    {
        public string FilePath { get; set; }
        //拿到请求报文的数据
        public HttpRequst(string msg)
        {  
            //将请求报文的数据处理，拿到要访问的文件路径
            string[] msgs = msg.Split(new[] { '\r', '\n' }, 
                StringSplitOptions.RemoveEmptyEntries);     //按照请求报文中的回车换行符进行分割。
            FilePath = msgs[0].Split(' ')[1];//从请求报文中获取了用户请求的文件的名称

            
        }
      
    }
}
