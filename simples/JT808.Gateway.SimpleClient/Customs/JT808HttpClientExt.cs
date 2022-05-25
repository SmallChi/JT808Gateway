using JT808.Gateway.Abstractions.Dtos;
using JT808.Gateway.WebApiClientTool;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.Gateway.SimpleClient.Customs
{
    public class JT808HttpClientExt : JT808HttpClient
    {
        public static string index1 = $"jt808apiext/index1";
        public JT808HttpClientExt(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// ext
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<string>> GetIndex1()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, index1);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<string>>();
            return value;
        }
    }
}
