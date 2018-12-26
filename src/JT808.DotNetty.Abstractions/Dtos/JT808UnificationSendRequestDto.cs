namespace JT808.DotNetty.Abstractions.Dtos
{
    /// <summary>
    /// 统一下发请求参数
    /// </summary>
    public class JT808UnificationSendRequestDto
    {
        public string TerminalPhoneNo { get; set; }
        public byte[] Data { get; set; }
    }
}
