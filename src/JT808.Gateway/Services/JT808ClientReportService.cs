using JT808.Gateway.Client;
using JT808.Gateway.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JT808.Gateway.Services
{
    public class JT808ClientReportService
    {
        private readonly JT808ClientReceiveAtomicCounterService jT808ReceiveAtomicCounterService;
        private readonly JT808ClientSendAtomicCounterService jT808SendAtomicCounterService;
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        public List<JT808ClientReport> JT808Reports { get; private set; }

        public JT808ClientReportService(
            JT808ClientReceiveAtomicCounterService jT808ReceiveAtomicCounterService,
            JT808ClientSendAtomicCounterService jT808SendAtomicCounterService,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            this.jT808ReceiveAtomicCounterService = jT808ReceiveAtomicCounterService;
            this.jT808SendAtomicCounterService = jT808SendAtomicCounterService;
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            JT808Reports = new List<JT808ClientReport>();
        }

        public void Create()
        {
            var clients = jT808TcpClientFactory.GetAll();
            JT808Reports.Add(new JT808ClientReport()
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
