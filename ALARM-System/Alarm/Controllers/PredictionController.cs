using Alarm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alarm.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PredictionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Для отображения формы предсказания
        public IActionResult Index()
        {
            return View(); // Отображение начальной страницы с формой
        }

        // Для обработки данных формы и вызова WebAPI
        [HttpPost]
        public async Task<IActionResult> Predict(PredictionRequest request)
        {
            var client = _httpClientFactory.CreateClient("PredictionAPI");

            // Отправка данных в WebAPI
            var response = await client.PostAsJsonAsync("FraudDetection/predict", request);
            if (response.IsSuccessStatusCode)
            {
                var prediction = await response.Content.ReadFromJsonAsync<PredictionResponse>();
                return View("PredictionResult", prediction); // Показ результата
            }
            else
            {
                // Возврат на начальную страницу с ошибкой
                ModelState.AddModelError("", "Ошибка при получении предсказания.");
                return View("Index");
            }
        }
    }
}
