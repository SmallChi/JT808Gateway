using JT808.DotNetty.Client.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JT808.DotNetty.Client.Services
{
    public class JT808ReportService
    {
        private readonly JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService;
        private readonly JT808SendAtomicCounterService jT808SendAtomicCounterService;
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        public List<JT808Report> JT808Reports { get; private set; }

        public JT808ReportService(
            JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService,
            JT808SendAtomicCounterService jT808SendAtomicCounterService,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            this.jT808ReceiveAtomicCounterService = jT808ReceiveAtomicCounterService;
            this.jT808SendAtomicCounterService = jT808SendAtomicCounterService;
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            JT808Reports = new List<JT808Report>();
        }

        public void Create()
        {
            var clients = jT808TcpClientFactory.GetAll();
            JT808Reports.Add(new JT808Report()
            {
                  SendTotalCount= jT808SendAtomicCounterService.MsgSuccessCount,
                  ReceiveTotalCount= jT808ReceiveAtomicCounterService.MsgSuccessCount,
                  CurrentDate=DateTime.Now,
                  Connections= clients.Count,
                  OnlineConnections= clients.Where(w => w.IsOpen).Count(),
                  OfflineConnections= clients.Where(w => !w.IsOpen).Count(),
            });
        }
    }
}
