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
            ViewBag.locationList = await LocationsList();
            return View();
        }
        public async Task<List<LocationViewModel.Datum>> GetLocationsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=Istanbul"),
                Headers =
    {
        { "X-RapidAPI-Key", "87df6af458msh956312b5472f837p1c4bfbjsne049073a2de2" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<LocationViewModel>(body);
                return value.data.ToList();
            }

        }
        internal async Task<List<SelectListItem>> LocationsList()
        {
            List<SelectListItem> locationsList = (from x in await GetLocationsAsync()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.name,
                                                      Value = x.dest_id
                                                  }).ToList();
            return locationsList;
        }

        public async Task<IActionResult> HotelList(PostLocationViewModel loc)
        {

            var client = _httpClientFactory.CreateClient();
            var baseUri = "https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels";
            // Construct the query string with dynamic dates
            var queryString = $"?dest_id={loc.dest_id}&search_type={loc.search_type}&arrival_date={loc.arrival_date.ToString("yyyy-MM-dd")}&departure_date={loc.departure_date.ToString("yyyy-MM-dd")}";

            // Combine base URI and query string
            var requestUri = new Uri(baseUri + queryString);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers =
{
        { "X-RapidAPI-Key", "38c011d3demsh877633ef4ae234ap1fae70jsn5f63793d31a2" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return View();
            }
        }

    }
}
