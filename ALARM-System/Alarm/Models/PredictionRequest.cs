using Alarm.Models.Da;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Alarm.Models
{
    public class PredictionRequest
    {     
         public string prim_okved { get; set; }
         public int zsk { get; set; }
         public int inn_type { get; set; }
         public string company_age { get; set; }

        [Required]
        public string phone_mask { get; set; }
      

        [Required]
        [MinLength(10)]
        public string inn { get; set; }

    }
}
