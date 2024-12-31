using Alarm.Models;
using Alarm.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Alarm.Services;

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
            // Call DaData service to get company information
            var info = await _daDataService.GetCompanyInfo(request.inn);
            if (info == null)
            {
                ModelState.AddModelError("", "Provided INN not found in gov data. Please check and try again");
                return View("Index");
            }

            if ( info.Suggestions[0].Data.State.Status == "ACTIVE")
            {
                // Enrich the request object with additional data
                request.inn_type = info.Suggestions[0].Data.InnType;
                request.prim_okved = info.Suggestions[0].Data.Okved;
                request.company_age = info.Suggestions[0].Data.CompanyAge;
                request.phone_mask = request.phone_mask.Substring(0, request.phone_mask.Length - 4);
                //TODO: Remove from the protocol and model zsk
                request.zsk = 1;

                // Reset and revalidate ModelState
                ModelState.Clear(); // Clear the existing state
                TryValidateModel(request); // Revalidate the updated model

                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient("PredictionAPI");

                    // send data to WebAPI
                    var response = await client.PostAsJsonAsync("FraudDetection/predict", request);

                    if (response.IsSuccessStatusCode)
                    {
                        var prediction = await response.Content.ReadFromJsonAsync<PredictionResponse>();

                        // Combine DaData info with prediction result
                        var result = new PredictionResponse
                        {
                            inn = info.Suggestions[0].Data.Inn,
                            prim_okved = info.Suggestions[0].Data.Okved,
                            inn_type = info.Suggestions[0].Data.Opf.Short,
                            company_name = info.Suggestions[0].Value,
                            company_origin_date = info.Suggestions[0].Data.OriginDate,
                            Probability = prediction.Probability,
                            Score = prediction.Score
                        };

                    return View("PredictionResult", result);
                }
                else
                {
                    // Error, send user back with error
                    ModelState.AddModelError("", "Prediction error");
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
            else
            {
                ModelState.AddModelError("", "Company: " + info.Suggestions[0].Data.Opf.Short + " " + info.Suggestions[0].Value + " is, " + info.Suggestions[0].Data.State.Status);
                return View("Index");               
            }           

        }

    }
}
