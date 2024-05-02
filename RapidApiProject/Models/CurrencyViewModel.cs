namespace RapidApiProject.Models
{
    public class CurrencyViewModel
    {
            public Datum[] data { get; set; }
        public class Datum
        {
            public string name { get; set; }
            public string code { get; set; }
            public string encodedSymbol { get; set; }
            public string symbol { get; set; }
        }
    }
}
