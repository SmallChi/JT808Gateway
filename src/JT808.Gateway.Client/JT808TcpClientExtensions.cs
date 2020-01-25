using JT808.Protocol;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Gateway.Client.Metadata;

namespace JT808.Gateway.Client
{
    public static class JT808TcpClientExtensions
    {
        public static void Send(this JT808TcpClient client, JT808Package package, int minBufferSize = 4096)
        {
            package.Header.TerminalPhoneNo = client.DeviceConfig.TerminalPhoneNo;
            JT808ClientRequest request = new JT808ClientRequest(package, minBufferSize);
            client.Send(request);
        }
    }

}
