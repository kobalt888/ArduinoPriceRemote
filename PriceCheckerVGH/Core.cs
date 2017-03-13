using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PriceCheckerVGH
{
    public class Core
    {
        public string cost { get; set; }

        public async Task<String> getPrice(string upc)
        {

            HttpClient gamePricer = new HttpClient();
            Dictionary<string, string> jsonKeyValues = new Dictionary<string, string>();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["upc"] = upc;
            var uri = "https://ae.pricecharting.com/api/product?t=7b3a8558f2514753ddabd35a40f711c857c42910&" + queryString;
            using (var content = new StringContent(""))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await gamePricer.GetAsync(uri);

                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got response for UPC:" + upc);
                    Console.WriteLine(jsonString.Result);

                    PriceResponse pResponse;

                    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString.Result));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PriceResponse));
                    pResponse = (PriceResponse)serializer.ReadObject(ms);
                    ms.Flush();

                    if (pResponse.price.Length == 5)
                    {
                        var finalCostFormat = pResponse.price.Insert(3, ".");
                        cost = "$" + finalCostFormat;
                    }

                    if (pResponse.price.Length == 4)
                    {
                        var finalCostFormat= pResponse.price.Insert(2, ".");
                        cost = "$" + finalCostFormat;
                    }

                    if (pResponse.price.Length == 3)
                    {
                        var finalCostFormat = pResponse.price.Insert(1, ".");
                        cost = "$" + finalCostFormat;
                    }

                }
                return "OK";
            }

        }
    }
}
