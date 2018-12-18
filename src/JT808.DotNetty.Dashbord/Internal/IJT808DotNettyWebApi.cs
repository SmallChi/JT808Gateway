using JT808.DotNetty.Dtos;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace JT808.DotNetty.Dashbord.Internal
{
    public interface IJT808DotNettyWebApi : IHttpApi
    {
        [HttpPost]
        ITask<JT808ResultDto<bool>> UnificationSend([Uri]string uri, [FormContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
        [HttpGet]
        ITask<JT808ResultDto<List<JT808SessionInfoDto>>> SessionGetAll([Uri]string uri);
        [HttpPost]
        ITask<JT808ResultDto<bool>> SessionRemoveByChannelId([Uri]string uri,[FormContent]string channelId);
        [HttpPost]
        ITask<JT808ResultDto<bool>> SessionRemoveByTerminalPhoneNo([Uri]string uri, [FormContent]string terminalPhoneNo);
        [HttpPost]
        ITask<JT808ResultDto<bool>> SourcePackageAdd([Uri]string uri, [FormContent]JT808IPAddressDto jT808IPAddressDto);
        [HttpPost]
        ITask<JT808ResultDto<bool>> SourcePackageRemove([Uri]string uri, [FormContent]JT808IPAddressDto jT808IPAddressDto);
        [HttpGet]
        ITask<JT808ResultDto<List<JT808SourcePackageChannelInfoDto>>> SourcePackageGetAll([Uri]string uri);
        [HttpPost]
        ITask<JT808ResultDto<bool>> TransmitAdd([Uri]string uri, [FormContent]JT808IPAddressDto jT808IPAddressDto);
        [HttpPost]
        ITask<JT808ResultDto<bool>> TransmitRemove([Uri]string uri, [FormContent]JT808IPAddressDto jT808IPAddressDto);
        [HttpGet]
        ITask<JT808ResultDto<List<string>>> TransmitGetAll([Uri]string uri);
        [HttpGet]
        ITask<JT808ResultDto<JT808AtomicCounterDto>> GetAtomicCounter([Uri]string uri);
    }
}
