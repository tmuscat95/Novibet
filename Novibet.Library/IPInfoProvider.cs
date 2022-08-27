using Newtonsoft.Json;
using Novibet.Library.Exceptions;
using Novibet.Library.Interfaces;
using Novibet.Library.Models;
using System.Net.Http.Headers;

namespace Novibet.Library
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private const string APIKey = "93c89dae5649b9af24a8119b6c91ecae";
        private readonly HttpClient httpClient;
        public IPInfoProvider()
        {
            httpClient = new HttpClient();
        }
        public IPDetails GetDetails(string ip)
        {
            var requestURL = $"http://api.ipstack.com/{ip}?access_key={APIKey}";

            try
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = httpClient.GetStringAsync(requestURL);
                task.Wait();
                var res = task.Result;
                if (res == null)
                    throw new IPServiceNotAvailableException("API Request Error");
                res = res.Replace("continent_name", "continent").Replace("country_name", "country");
                IPDetails? details = JsonConvert.DeserializeObject<IPDetails>(res);
                if (details == null)
                    throw new IPServiceNotAvailableException("Deserialization Error");
                return details;
            }
            catch (IPServiceNotAvailableException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }


        }

    }
}
