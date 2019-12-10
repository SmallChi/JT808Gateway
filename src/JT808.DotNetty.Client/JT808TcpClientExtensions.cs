using JT808.DotNetty.Client.Metadata;
using JT808.Protocol;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;

namespace JT808.DotNetty.Client
{
    public static class JT808TcpClientExtensions
    {
        public static void Send(this JT808TcpClient client, JT808Package package, int minBufferSize = 4096)
        {
            package.Header.TerminalPhoneNo = client.DeviceConfig.TerminalPhoneNo;
            package.Header.MsgNum = client.DeviceConfig.MsgSNDistributed.Increment(); 
            JT808ClientRequest request = new JT808ClientRequest(package, minBufferSize);
            client.Send(request);
        }
    }

}
