using Alarm.Models;
using Alarm.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Alarm.Controllers
{
    public class DaDataController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly DaDataService _daDataService;

        public DaDataController(DaDataService daDataService)
        {
            _httpClient = new HttpClient();
            _daDataService = daDataService;
        }

         public async Task<IActionResult> GetCompanyInfo(string inn)
        {
            var result = await _daDataService.GetCompanyInfo(inn);
            return Json(result);
        }
    }
}
