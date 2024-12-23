using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AlarmWebAPI.Controllers.ML
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
            // Create prediction engine
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);

            // Get prediction
            var prediction = predictionEngine.Predict(input);
                        
            // return
            return Ok(new
            {
                isFraud = prediction.PredictedLabel,
                probability = prediction.Probability,
                score= prediction.Score
            });
        }

        public class ModelInput
        {
            public float prim_okved { get; set; }
            public float zsk { get; set; }
            public float inn_type { get; set; }
            public string company_age { get; set; }
            public float phone_mask { get; set; }
           
            public bool is_fraud { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public bool PredictedLabel { get; set; } // Binary prediction

            public float Probability { get; set; } // Confidence probability
            public float Score { get; set; } // Raw score
        }
    }
}
