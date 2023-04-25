using WasteDetection.Models.Entities;

namespace WasteDetection.Da
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            bool resultEnsureCreated = context.Database.EnsureCreated();

            if (context.TrainImageClassificatierRequests.Any())
                return;

            string preparedInputsBasePath = "\\detection\\prepared_inputs\\";

            string computeImageStatisticsBasePath = "\\detection\\compute_image_statistics\\prepared\\";
            ComputeImageStatisticsRequest[] computeImageStatisticsRequest = new ComputeImageStatisticsRequest[]
            {
                new ComputeImageStatisticsRequest
                {
                    Id = Guid.NewGuid(),
                    CreateOn = DateTime.Now,
                    InpImgPath = preparedInputsBasePath + "1to10.tif",
                    OutXmlPath = computeImageStatisticsBasePath + "1to10.xml",
                    Suceeded = true
                },
            };
            foreach (ComputeImageStatisticsRequest cisr in computeImageStatisticsRequest)
            {
                context.ComputeImageStatisticsRequests.Add(cisr);
            }
            var resultStatisticsAdd = context.SaveChanges();

            string trainImageClassificafierOutBasePath = "\\detection\\train_image_classifier\\prepared\\";
            TrainImageClassificatierRequest[] trainImageClassificatierRequests = new TrainImageClassificatierRequest[]
            {
                new TrainImageClassificatierRequest 
                { 
                    Id = Guid.NewGuid(),
                    CreateOn = DateTime.Now,
                    InpImgPath = preparedInputsBasePath + "1to10.tif",
                    InpVectorPath = preparedInputsBasePath + "statistics\\1to10.xml",
                    ValidationVectorPath = preparedInputsBasePath + "\\training_layers\\training_classes.shp",
                    InpXmlStatisticsPath = preparedInputsBasePath + "\\control_layers\\control_classes.shp",
                    LabelField = "class",
                    OutModelPath = trainImageClassificafierOutBasePath + "model_1to10.mdl",
                    OutConfusionMatrixPath = trainImageClassificafierOutBasePath + "confm_1to10.xml",
                    Suceeded = true,
                    TrainingClassifierName = "rf"
                },
            };
            foreach (TrainImageClassificatierRequest ticr in trainImageClassificatierRequests)
            {
                context.TrainImageClassificatierRequests.Add(ticr);
            }
            var resultTrainClassificationAdd = context.SaveChanges();

            string imageClassifierOutBasePath = "\\detection\\image_classifier\\prepared\\";
            ImageClassificationRequest[] imageClassificationRequests = new ImageClassificationRequest[]
            {
                new ImageClassificationRequest
                {
                    Id = Guid.NewGuid(),
                    CreateOn = DateTime.Now,
                    InpImgPath = preparedInputsBasePath + "1to10.tif",
                    InpModelPath = preparedInputsBasePath + "model_1to10.mdl",
                    InpXmlStatisticsPath = preparedInputsBasePath + "\\control_layers\\control_classes.shp",
                    OutRasterPath = imageClassifierOutBasePath + "raster_1to10.tif",
                    OutConfidenceMapPath = imageClassifierOutBasePath + "confidence_map_1to10.tif",
                    Suceeded = true
                }
            };
            foreach (ImageClassificationRequest icr in imageClassificationRequests)
            {
                context.ImageClassificationRequests.Add(icr);
            }
            var resultClassificationAdd = context.SaveChanges();
        }
    }
}
