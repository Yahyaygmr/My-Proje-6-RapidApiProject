using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RapidApiProject.Models;
using RapidApiProject.Models.SonaHotelModels;
using System.Diagnostics;

namespace RapidApiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetLocations(PostLocationViewModel postLocationViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=" + postLocationViewModel.LocationName),
                Headers =
    {
        { "X-RapidAPI-Key", "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var locationData = JsonConvert.DeserializeObject<LocationViewModel>(body);
                int? destId = Convert.ToInt32(locationData.data[0].dest_id);
                var postLocationWithId = new PostLocationViewModel
                {
                    LocationName = postLocationViewModel.LocationName,
                    dest_id = destId,
                    arrival_date = postLocationViewModel.arrival_date,
                    departure_date = postLocationViewModel.departure_date
                };

                return RedirectToAction("HotelList", "Home", postLocationWithId);
            }

        }
      
        public async Task<IActionResult> HotelList(PostLocationViewModel loc)
        {
            var arrv_date = loc.arrival_date.ToString("yyyy-MM-dd");
            var dept_date = loc.departure_date.ToString("yyyy-MM-dd");

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id=" + loc.dest_id + "&search_type=CITY&arrival_date=" + arrv_date + "&departure_date=" + dept_date + "&adults=1&children_age=0%2C17&room_qty=1&page_number=1&languagecode=en-us&currency_code=EUR"),
                Headers =
    {
        { "X-RapidAPI-Key", "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<SearchHotelsViewModel>(body);
                if (value.data.hotels != null)
                {
                    return View(value.data.hotels.ToList());
                }
                return View(null);
               

            }

        }
        public async Task<IActionResult> PropertyDetail(string hotel_id,string arrival_date,string departure_date)
        {
            var arrv_date = arrival_date;
            var dept_date = departure_date;

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id="+hotel_id+"&arrival_date="+arrv_date+"&departure_date="+dept_date+"&adults=1&children_age=1%2C17&room_qty=1&languagecode=en-us&currency_code=EUR"),
                Headers =
    {
        { "X-RapidAPI-Key", "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<ListHotelDetailViewModel>(body);
                if(value.data != null)
                {
                    return View(value.data);
                }
            }
            return View();
        }
    }
}
