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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PriceCheckerVGH
{
    public class Core
    {
        public string cost { get; set; }
        PriceResponse pResponse;
        static DateTime now = DateTime.Now;
        Boolean status=true;
        public string flag = null;

        static string fileName = now.ToShortDateString();
        static string finalFileName = fileName.Replace("/", "-");
        string filePath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/"+finalFileName+" Used Price Update List.csv");

        //
        public string gTitle;


        //

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

                    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString.Result));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PriceResponse));
                    pResponse = (PriceResponse)serializer.ReadObject(ms);
                    ms.Flush();
                    gTitle = pResponse.gameTitle;
                    if (pResponse.price == null)
                    {
                        cost = null;
                        flag = "OK";
                        Console.WriteLine(pResponse.gameTitle + " for console " + pResponse.consoleType + " found, but no price.");
                        return "OK";
                    }

                    if (pResponse.price.Length == 5)
                    {
                        var finalCostFormat = pResponse.price.Insert(3, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[4] = '9';
                        cost = "$" + new string(array);
                    }

                    else if (pResponse.price.Length == 4)
                    {
                        var finalCostFormat= pResponse.price.Insert(2, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[3] = '9';
                        cost = "$" + new string(array);
                    }

                    else if (pResponse.price.Length == 3)
                    {
                        var finalCostFormat = pResponse.price.Insert(1, ".");
                        char[] array = finalCostFormat.ToCharArray();
                        array[2] = '9';
                        cost = "$" + new string(array);
                    }
                    pResponse.price = cost;
                    flag = "OK";
                }
                else
                {
                    Console.WriteLine("No game found, possible dupe UPC.");
                    flag = "FAIL";
                    return "FAIL";
                }  
                return "OK";
            }

        }

        public int writeGame()
        {
            if (pResponse.price == null)
            {
                Console.WriteLine("No game found with this upc to be added to database.");
                return -1;
            }
            var csv = new StringBuilder();
            status = true;
            var newLine = string.Format("{0},{1},{2},{3}", pResponse.consoleType, pResponse.gameTitle,pResponse.price,pResponse.upc);
            csv.AppendLine(newLine);
            try
            {
                if (new FileInfo(filePath).Length == 0)
                {

                }
            }

            catch (FileNotFoundException ex1)
            {
                Console.WriteLine("No file found for data storage for day " + now.ToShortDateString() + ". Creating...");
                try
                {
                    status = false;
                    var title = new StringBuilder();
                    var content = "Console,Game,GS Price,UPC";
                    title.AppendLine(content);
                    File.AppendAllText(filePath, title.ToString());
                    File.AppendAllText(filePath, csv.ToString());
                    Console.WriteLine(pResponse.gameTitle + " was added to file for price updating.");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Please close the excel document to write games to file. The last game was not added.");
                    return -2;
                }

            }
            if(status)
            {
                try
                {
                    File.AppendAllText(filePath, csv.ToString());
                    Console.WriteLine(pResponse.gameTitle + " was added to file for price updating.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please close the excel document to write games to file. The last game was not added.");
                    return -2;
                }
               
            }
            return 1;
        }
    }
}
