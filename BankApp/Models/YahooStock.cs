using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBank.Models
{
    public class stockInfo
    {       
        public string symbol { get; set; }

        public Price price { get; set; }
       
    }

    public class Price
    {
        public string shortName { get; set; }

        public MarketPrice regularMarketPrice { get; set; }
    }

    public class MarketPrice 
    {
        public string fmt { get; set; }
    }


}