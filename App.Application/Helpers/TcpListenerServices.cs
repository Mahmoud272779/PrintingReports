using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace App.Application.Helpers
{
    public class TcpListenerServices
    {
        public static string PrintExporting(string filePath,IConfiguration _configuration)
        {

            try
            {
                Encoding iso = Encoding.GetEncoding("ISO-8859-6");
                TcpClient clientSocket = new TcpClient();
                string ip = _configuration["ApplicationSetting:ExportingToolIp"];
                int port = int.Parse(_configuration["ApplicationSetting:ExportingToolPort"]);
                clientSocket.Connect(ip, port);
                NetworkStream serverStream = clientSocket.GetStream();

                // Request
                byte[] outStream = iso.GetBytes(filePath);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                //Response
                byte[] bytesToRead = new byte[clientSocket.ReceiveBufferSize];
                int bytesRead = serverStream.Read(bytesToRead, 0, clientSocket.ReceiveBufferSize);
                string resp = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                return resp;

            }
            catch (Exception EXC)
            {
                return EXC.Message;
            }
        }
    }
}
