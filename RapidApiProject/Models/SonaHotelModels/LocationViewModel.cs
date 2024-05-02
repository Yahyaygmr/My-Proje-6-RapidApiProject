namespace RapidApiProject.Models.SonaHotelModels
{
    public class LocationViewModel
    {
            public Datum[] data { get; set; }
       
        public class Datum
        {
            public string dest_id { get; set; }
            public string name { get; set; }
            public string search_type { get; set; }
        }

    }
}
