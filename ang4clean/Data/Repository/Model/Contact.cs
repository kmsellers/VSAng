using System;
using System.Collections.Generic;
using System.Linq;
namespace VSAng.Data.Repository.Model
{

    using Newtonsoft.Json;
    [Collection(CollectionDatabases.VSAng)]
    public class Contact : IHasId
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get ; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string first_name { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string last_name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }

        //[JsonProperty(PropertyName = "mobile_phone")]
        //public string mobile_phone { get; set; }

        //[JsonProperty(PropertyName = "work_phone")]
        //public string work_phone { get; set; }

        [JsonProperty(PropertyName = "address_line_1")]
        public string address_line_1 { get; set; }

        //[JsonProperty(PropertyName = "address_line_2")]
        //public string address_line_2 { get; set; }

        //[JsonProperty(PropertyName = "address_line_3")]
        //public string address_line_3 { get; set; }

        //[JsonProperty(PropertyName = "address_line_4")]
        //public string address_line_4 { get; set; }


        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string state { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        public string postal_code { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string country { get; set; }
    }
}