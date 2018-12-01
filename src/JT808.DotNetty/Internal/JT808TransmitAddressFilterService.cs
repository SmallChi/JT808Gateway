using JT808.DotNetty.Configurations;
using JT808.DotNetty.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// JT808转发地址过滤服务
    /// 按照808的消息，有些请求必须要应答，但是转发可以不需要有应答可以节省部分资源包括：
    //  1.消息的序列化
    //  2.消息的下发
    //  都有一定的性能损耗，那么不需要判断写超时 IdleState.WriterIdle 
    //  就跟神兽貔貅一样。。。
    /// </summary>
    public class JT808TransmitAddressFilterService : IDisposable
    {
        private readonly IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor;

        private ConcurrentDictionary<string,int> ForwardingRemoteAddresssDict;

        private IDisposable jT808ConfigurationOptionsMonitorDisposable;

        public JT808TransmitAddressFilterService(
            IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor)
        {
            this.jT808ConfigurationOptionsMonitor = jT808ConfigurationOptionsMonitor;
            ForwardingRemoteAddresssDict = new ConcurrentDictionary<string, int>();
            InitForwardingRemoteAddress(jT808ConfigurationOptionsMonitor.CurrentValue.ForwardingRemoteAddress);
            //OnChange 源码多播委托
            jT808ConfigurationOptionsMonitorDisposable = this.jT808ConfigurationOptionsMonitor.OnChange(options =>
            {
                InitForwardingRemoteAddress(options.ForwardingRemoteAddress);
            });
        }

        private void InitForwardingRemoteAddress(List<JT808ClientConfiguration> jT808ClientConfigurations)
        {
            if (jT808ClientConfigurations != null && jT808ClientConfigurations.Count > 0)
            {
                foreach (var item in jT808ClientConfigurations)
                {
                    string host = item.EndPoint.ToString();
                    ForwardingRemoteAddresssDict.TryAdd(host, 0);
                }
            }
        }

        public bool ContainsKey(EndPoint endPoint)
        {
            return ForwardingRemoteAddresssDict.ContainsKey(endPoint.ToString());
        }

        public JT808ResultDto<bool> Add(JT808IPAddressDto jT808IPAddressDto)
        {
            string host = jT808IPAddressDto.EndPoint.ToString();
            return new JT808ResultDto<bool>() { Code = JT808ResultCode.Ok, Data = ForwardingRemoteAddresssDict.TryAdd(host,0) };
        }

        public JT808ResultDto<bool> Remove(JT808IPAddressDto jT808IPAddressDto)
        {
            string host = jT808IPAddressDto.EndPoint.ToString();
            if(jT808ConfigurationOptionsMonitor.CurrentValue.ForwardingRemoteAddress!=null &&
                jT808ConfigurationOptionsMonitor.CurrentValue.ForwardingRemoteAddress.Any(w=>w.EndPoint.ToString()== host))
            {
                return new JT808ResultDto<bool>() { Code = JT808ResultCode.Ok, Data = false,Message="不能删除服务器配置的地址" };
            }
            else
            {
                return new JT808ResultDto<bool>() { Code = JT808ResultCode.Ok, Data = ForwardingRemoteAddresssDict.TryRemove(host,out var temp) };
            }
        }

        public JT808ResultDto<List<string>> GetAll()
        {
            return new JT808ResultDto<List<string>>(){ Code = JT808ResultCode.Ok, Data = ForwardingRemoteAddresssDict.Select(s=>s.Key).ToList() };
        }

        public void Dispose()
        {
            jT808ConfigurationOptionsMonitorDisposable.Dispose();
        }
    }
}
