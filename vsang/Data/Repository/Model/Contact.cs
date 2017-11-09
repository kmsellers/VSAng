using System;
using System.Collections.Generic;
using System.Linq;
namespace VSAng.Data.Repository.Model
{

    using Newtonsoft.Json;
    [Collection(CollectionDatabases.VSAng)]
    public class Contact : IHasId
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid id { get ; set; }

        [JsonProperty(PropertyName = "first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string first_name { get; set; }

        [JsonProperty(PropertyName = "last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string last_name { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }

        [JsonProperty(PropertyName = "mobile_phone",NullValueHandling = NullValueHandling.Ignore)]
        public string mobile_phone { get; set; }

        [JsonProperty(PropertyName = "work_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string work_phone { get; set; }

        [JsonProperty(PropertyName = "address_line_1", NullValueHandling = NullValueHandling.Ignore)]
        public string address_line_1 { get; set; }

        //[JsonProperty(PropertyName = "address_line_2")]
        //public string address_line_2 { get; set; }

        //[JsonProperty(PropertyName = "address_line_3")]
        //public string address_line_3 { get; set; }

        //[JsonProperty(PropertyName = "address_line_4")]
        //public string address_line_4 { get; set; }


        [JsonProperty(PropertyName = "city", NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string state { get; set; }

        [JsonProperty(PropertyName = "postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code { get; set; }

        [JsonProperty(PropertyName = "country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }
    }
}