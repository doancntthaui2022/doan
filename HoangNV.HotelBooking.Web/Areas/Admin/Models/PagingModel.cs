using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Models
{
    public class PagingModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("recordsTotal")]
        public int Total { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("allIds")]
        public List<string> AllIds { get; set; }

        public PagingModel(int page, int total, object data)
        {
            Page = page;
            Total = total;
            RecordsFiltered = total;
            Data = data;
        }
    }

    public class DataTableAjaxPostModel
    {
        public int page { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
