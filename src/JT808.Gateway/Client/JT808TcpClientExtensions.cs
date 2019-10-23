using JT808.Protocol;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Gateway.Metadata;

namespace JT808.Gateway.Client
{
    public static class JT808TcpClientExtensions
    {
        public static void Send(this JT808TcpClient client, JT808Header header, JT808Bodies bodies, int minBufferSize = 1024)
        {
            JT808Package package = new JT808Package();
            package.Header = header;
            package.Bodies = bodies;
            package.Header.TerminalPhoneNo = client.DeviceConfig.TerminalPhoneNo;
            package.Header.MsgNum = client.DeviceConfig.MsgSNDistributed.Increment(); 
            JT808ClientRequest request = new JT808ClientRequest(package, minBufferSize);
            client.Send(request);
        }

        /// <summary>
        /// 终端通用应答
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0001 bodies, int minBufferSize = 100)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.终端通用应答.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }

        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0002 bodies, int minBufferSize = 100)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.终端心跳.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }

        /// <summary>
        /// 终端注销
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0003 bodies, int minBufferSize = 100)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.终端注销.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }

        /// <summary>
        /// 终端鉴权
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0102 bodies, int minBufferSize = 100)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.终端鉴权.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }

        /// <summary>
        /// 终端注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0100 bodies, int minBufferSize = 100)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.终端注册.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }

        /// <summary>
        /// 位置信息汇报
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bodies"></param>
        /// <param name="minBufferSize"></param>
        public static void Send(this JT808TcpClient client, JT808_0x0200 bodies, int minBufferSize = 200)
        {
            JT808Header header = new JT808Header();
            header.MsgId = JT808MsgId.位置信息汇报.ToUInt16Value();
            client.Send(header, bodies, minBufferSize);
        }
    }
}
