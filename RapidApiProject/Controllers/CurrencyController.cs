using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RapidApiProject.Models;

namespace RapidApiProject.Controllers
{

    public class CurrencyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.currecies = await GetCurrencyCodes();

            return View();
        }
        public async Task<List<CurrencyViewModel.Datum>> GetCurrencyAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/meta/getCurrency"),
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
                var values = JsonConvert.DeserializeObject<CurrencyViewModel>(body);
                return values.data.ToList();
            }
        }

        internal async Task<List<SelectListItem>> GetCurrencyCodes()
        {
            List<SelectListItem> currecyCodes = (from x in await GetCurrencyAsync()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.code,
                                                     Value = x.code
                                                 }).ToList();
            return currecyCodes;
        }

        public async Task<IActionResult> ExchangeRates(string CurrencyCode = "TRY")
        {
            ViewBag.currencyCode = CurrencyCode;
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/meta/getExchangeRates?base_currency=" + CurrencyCode),
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
                var values = JsonConvert.DeserializeObject<ExchangeRatesViewModel>(body);
                return View(values.data.exchange_rates.ToList());
            }
        }
    }
}
