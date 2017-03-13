using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace PriceCheckerVGH
{
    [DataContract]
    public class PriceResponse
    {
        [DataMember(Name = "gamestop-price")]
        public string price { get; set; }
    }
}
