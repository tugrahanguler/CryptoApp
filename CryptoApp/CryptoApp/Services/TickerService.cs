using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoApp.Services
{
    public class TickerService
    {

        public List<TickerProperties> GetData()
        {
            string apiUrl = "https://api.btcturk.com/api/v2/ticker";

            var data = TickerService.GetDataFromApi<List<TickerProperties>>(apiUrl).Result;

            return data;
        }

        public static async Task<T> GetDataFromApi<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                JObject data = (JObject)JObject.Parse(resultContentString);
                IList<JToken> DataForList = data["data"].Children().ToList();


                IList<TickerProperties> TickerPropertiesList = new List<TickerProperties>();

                foreach (JToken results in DataForList)
                {
                    TickerProperties ConvertedInfos = results.ToObject<TickerProperties>();
                    TickerPropertiesList.Add(ConvertedInfos);

                }
                return (T)TickerPropertiesList;

            }
        }

    }
}
