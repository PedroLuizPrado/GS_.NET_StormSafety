using Microsoft.ML;
using Microsoft.ML.Data;

namespace StormSafety.API.Services
{
    public class MLModelService
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<OcorrenciaInput, OcorrenciaPrediction> _predictionEngine;

        public MLModelService()
        {
            _mlContext = new MLContext();

            var data = _mlContext.Data.LoadFromEnumerable(new[]
            {
                new OcorrenciaInput { Descricao = "Rua alagada", Tipo = "Enchente" },
                new OcorrenciaInput { Descricao = "Árvore caiu na pista", Tipo = "Queda de árvore" },
                new OcorrenciaInput { Descricao = "Ficamos sem luz depois da tempestade", Tipo = "Falta de energia" }
            });

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(OcorrenciaInput.Descricao))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(OcorrenciaInput.Tipo)))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(data);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<OcorrenciaInput, OcorrenciaPrediction>(model);
        }

        public string PreverTipo(string descricao)
        {
            var input = new OcorrenciaInput { Descricao = descricao };
            var prediction = _predictionEngine.Predict(input);
            return prediction.TipoPrevisto;
        }
    }

    public class OcorrenciaInput
    {
        public string Descricao { get; set; }
        public string Tipo { get; set; }
    }

    public class OcorrenciaPrediction
    {
        [ColumnName("PredictedLabel")]
        public string TipoPrevisto { get; set; }
    }
}
