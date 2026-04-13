using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.HttpClientService
{
    public interface IHttpApiClientFactory
    {
        Task<HttpResponseMessage> GetAsync(string baseUrl,string resource,string applicationName,long apiUserId,Dictionary<string, string>? headers = null);

        Task<HttpResponseMessage> PostAsync(string baseUrl,string resource,object body,string applicationName,long apiUserId,Dictionary<string, string>? headers = null,bool excludeNull = true);

        Task<HttpResponseMessage> PutAsync(string baseUrl,string resource,object body,string applicationName,long apiUserId,Dictionary<string, string>? headers = null);

        Task<HttpResponseMessage> DeleteAsync(string baseUrl,string resource,string applicationName,long apiUserId,Dictionary<string, string>? headers = null);
    }
}
