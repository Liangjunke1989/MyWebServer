using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public class HttpResponse
    {
        private string Content_Type { set; get; }
        private byte[] buffer = null;
        public HttpResponse(byte[] buffer,string filePath)
        {
            this.buffer = buffer;
            string fileExtension = Path.GetExtension(filePath);
            switch (fileExtension)
            {
                case ".html":
                case ".htm":
                    Content_Type = "text/html";
                    break;
            }
        }
        /// <summary>
        ///  获取响应报文的响应头
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeaderResponse()
        {
            StringBuilder builder=new StringBuilder();
            builder.Append("HTTP/1.1 200 ok\r\n");
            builder.Append("Content-Type:"+ Content_Type + ";charset=utf-8\r\n");
            builder.Append("Content-Length:" + buffer.Length + "\r\n\r\n");//在相应报文头的最后一行下面有一个空行
            //所以在里面加两组"\r\n\r\n"
            return System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            //text/html
            //text/plain
        }
    }
}
