using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Data;
using Microsoft.ML;

namespace AlarmWebAPI.Controllers.ML
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelTrainingController : ControllerBase
    {
        private readonly MLContext _mlContext;

        public ModelTrainingController()
        {
            _mlContext = new MLContext();
        }

        [HttpPost("train")]
        public IActionResult TrainModel()
        {
            string dataPath = "Dataset/ds_final.csv";

            // Load data
            IDataView data = _mlContext.Data.LoadFromTextFile<ModelInput>(
                dataPath,
                hasHeader: true,
                separatorChar: ',');

            // Split data
            var trainTestData = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

            // Указать "is_fraud" как Label
            var pipeline = _mlContext.Transforms.CopyColumns("Label", "is_fraud")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("inn_type"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("prim_okved"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("company_age"))
                .Append(_mlContext.Transforms.NormalizeMinMax("zsk"))
                 .Append(_mlContext.Transforms.NormalizeMinMax("phone_mask"))
                .Append(_mlContext.Transforms.Concatenate(
                    "Features",
                    "prim_okved",
                    "zsk",
                    "inn_type",
                    "company_age",
                    "phone_mask"))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

            // Model train
            var model = pipeline.Fit(trainTestData.TrainSet);

            // Model test
            var predictions = model.Transform(trainTestData.TestSet);
            var metrics = _mlContext.BinaryClassification.Evaluate(predictions, "Label");
            
            // Save model
            _mlContext.Model.Save(model, data.Schema, "Models/fraudDetectionModel.zip");

            return Ok(new
            {
                metrics.Accuracy,
                AUC = metrics.AreaUnderRocCurve
            });
        }

        public class ModelInput
        {
            [LoadColumn(0)]
            public float prim_okved { get; set; }

            [LoadColumn(1)]
            public float zsk { get; set; }

            [LoadColumn(2)]
            public float inn_type { get; set; }

            [LoadColumn(3)]
            public string company_age { get; set; }

            [LoadColumn(4)]
            public float phone_mask { get; set; }

            [LoadColumn(5)]
            public bool is_fraud { get; set; }
        }
    }
}
