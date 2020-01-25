using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JT808.Gateway.Client;
using JT808.Gateway.Client.Metadata;
using JT808.Gateway.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace JT808.Gateway.CleintBenchmark
{
    /// <summary>
    /// 车辆控制器
    /// </summary>
    [Route("JT808WebApi")]
    [ApiController]
    [EnableCors("Domain")]
    public class ReportController : ControllerBase
    {
        private readonly IJT808TcpClientFactory clientFactory;
        private readonly JT808ReceiveAtomicCounterService ReceiveAtomicCounterService;
        private readonly JT808SendAtomicCounterService SendAtomicCounterService;

        /// <summary>
        /// 
        /// </summary>
        public ReportController(
            IJT808TcpClientFactory factory,
            JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService,
            JT808SendAtomicCounterService jT808SendAtomicCounterService)
        {
            clientFactory = factory;
            ReceiveAtomicCounterService = jT808ReceiveAtomicCounterService;
            SendAtomicCounterService = jT808SendAtomicCounterService;
        }

        [HttpPost]
        [HttpGet]
        [Route("QueryReport")]
        public ActionResult<JT808Report> QueryReport()
        {
            var clients = clientFactory.GetAll();
            JT808Report report = new JT808Report()
            {
                SendTotalCount = SendAtomicCounterService.MsgSuccessCount,
                ReceiveTotalCount = ReceiveAtomicCounterService.MsgSuccessCount,
                CurrentDate = DateTime.Now,
                Connections = clients.Count,
                OnlineConnections = clients.Where(w => w.IsOpen).Count(),
                OfflineConnections = clients.Where(w => !w.IsOpen).Count(),
            };
            return report;
        }
    }
}