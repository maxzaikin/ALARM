using Alarm.Models;
using Alarm.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Alarm.Services;

// Razor page
namespace Alarm.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DaDataService _daDataService;

        public PredictionController(IHttpClientFactory httpClientFactory, DaDataService daDataService)
        {
            _httpClientFactory = httpClientFactory;
            _daDataService = daDataService;
        }

        // Display pred form
        public IActionResult Index()
        {
            return View();
        }

        // display form fields and call WebAPI
        [HttpPost]
        public async Task<IActionResult> Predict(PredictionRequest request)
        {
            var info = await _daDataService.GetCompanyInfo(request.inn);
            if (info == null)
            {
                ModelState.AddModelError("", "Company not found");
                return View("Index");
            }
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("PredictionAPI");

                // send data to WebAPI
                var response = await client.PostAsJsonAsync("FraudDetection/predict", request);
                if (response.IsSuccessStatusCode)
                {
                    var prediction = await response.Content.ReadFromJsonAsync<PredictionResponse>();
                    return View("PredictionResult", prediction);
                }
                else
                {
                    // Error, send user back with error
                    ModelState.AddModelError("", "Error receive predicion data");
                    return View("Index");
                }

            }
            else
            {
                // Error, send user back with error
                ModelState.AddModelError("", "Input data is invalid");
                return View("Index");
            }

        }

    }
}
