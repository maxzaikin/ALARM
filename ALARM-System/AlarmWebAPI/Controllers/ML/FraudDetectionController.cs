using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace AlarmWebAPI.Controllers.ML
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudDetectionController : ControllerBase
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public FraudDetectionController()
        {
            _mlContext = new MLContext();

            // Load model
            _model = _mlContext.Model.Load("Models/fraudDetectionModel.zip", out var modelInputSchema);
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] ModelInput input)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);
            var prediction = predictionEngine.Predict(input);

            return Ok(new
            {
                FraudProbability = prediction.Probability,
                IsFraud = prediction.Prediction
            });
        }

        public class ModelInput
        {
            public float prim_okved { get; set; }
            public float zsk { get; set; }
            public float inn_type { get; set; }
            public string company_age { get; set; }
            public float phone_mask { get; set; }
        }

        public class ModelOutput
        {
            public bool Prediction { get; set; }
            public float Probability { get; set; }
            public float Score { get; set; }
        }
    }
}
