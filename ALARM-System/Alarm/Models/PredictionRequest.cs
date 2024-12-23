using System.ComponentModel.DataAnnotations;

namespace Alarm.Models
{
    public class PredictionRequest
    {
        public float prim_okved { get; set; }
        public float zsk { get; set; }
        public float inn_type { get; set; }
        public string company_age { get; set; }
        public float phone_mask { get; set; }
        [Required]
        [MinLength(10)]
        public string inn { get; set; }

    }
}
