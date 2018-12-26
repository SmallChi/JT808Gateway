using System.Net;

namespace JT808.DotNetty.Configurations
{
    public class JT808ClientConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        private EndPoint endPoint;

        public EndPoint EndPoint
        {
            get
            {
                if (endPoint == null)
                {
                    if (IPAddress.TryParse(Host, out IPAddress ip))
                    {
                        endPoint = new IPEndPoint(ip, Port);
                    }
                    else
                    {
                        endPoint = new DnsEndPoint(Host, Port);
                    }
                }
                return endPoint;
            }
        }
    }
}
