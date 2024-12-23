namespace Alarm.Models
{
    public class PredictionResponse
    {
        public bool PredictedLabel { get; set; } // Binary prediction
        public float Probability { get; set; } // Confidence probability
        public float Score { get; set; } // Raw score

    }
}
