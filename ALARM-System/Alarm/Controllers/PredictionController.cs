using Alarm.Models;
using Microsoft.AspNetCore.Mvc;

// Razor page
namespace Alarm.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PredictionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
