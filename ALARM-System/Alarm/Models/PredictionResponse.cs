namespace Alarm.Models
{
    public class PredictionResponse
    {
        public string inn { get; set; }
        public string prim_okved { get; set; }
        public string inn_type { get; set; }
        public string company_name { get; set; }
        public DateTime company_origin_date { get; set; }
        public bool PredictedLabel { get; set; } // Binary prediction
        public float Probability { get; set; } // Confidence probability
        public float Score { get; set; } // Raw score

    }
}
