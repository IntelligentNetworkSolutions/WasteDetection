using CliWrap;
using Microsoft.AspNetCore.Mvc;
using WasteDetection.Models.Entities;
using WasteDetection.Services;

namespace WasteDetection.Controllers
{
    public class DetectionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SettingsService _settingsService;
        private OrfeoToolboxToolsService orfeoToolboxToolsService;

        public DetectionController(IConfiguration configuration, SettingsService settingsService, OrfeoToolboxToolsService orfeoToolboxToolsService)
        {
            _configuration = configuration;
            _settingsService = settingsService;
            this.orfeoToolboxToolsService = orfeoToolboxToolsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ComputeImageStatistics()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> ComputeImageStatisticsRequest(string inpImgPath)
        {
            if(string.IsNullOrEmpty(inpImgPath))
                throw new ArgumentNullException(nameof(inpImgPath));

            string resultFilePath = await orfeoToolboxToolsService.ComputeImageStatistics(inpImgPath);

            return resultFilePath;
        }

        [HttpGet]
        public IActionResult TrainImageClassifier()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> TrainImageClassifier(string inpImgPath, string inpTrainingVectorPath, 
            string inpValidationVectorPath, string inpXmlStatisticsPath, string labelField = "class")
        {
            if (string.IsNullOrEmpty(inpImgPath))
                throw new ArgumentNullException(nameof(inpImgPath));

            if (string.IsNullOrEmpty(inpTrainingVectorPath))
                throw new ArgumentNullException(nameof(inpTrainingVectorPath));

            if (string.IsNullOrEmpty(inpValidationVectorPath))
                throw new ArgumentNullException(nameof(inpValidationVectorPath));

            if (string.IsNullOrEmpty(inpXmlStatisticsPath))
                throw new ArgumentNullException(nameof(inpXmlStatisticsPath));

            TrainImageClassificatierRequest request = new TrainImageClassificatierRequest() {
                Id = Guid.NewGuid(),
                InpImgPath = inpImgPath,
                InpVectorPath = inpTrainingVectorPath,
                InpXmlStatisticsPath = inpXmlStatisticsPath,
                ValidationVectorPath = inpValidationVectorPath,
            };

            (string modelRelativePath, string modelAbsolutePath) = await orfeoToolboxToolsService.TrainImageClassifier(request);

            return modelRelativePath + "---" + modelAbsolutePath;
        }

        [HttpGet]
        public IActionResult ImageClassifier()
        {
            return View();
        }

        [HttpPost]
        public async Task<(string, string)> ImageClassifier(string inpImgPath, string inpModelPath, string inpXmlStatisticsPath)
        {
            if (string.IsNullOrEmpty(inpImgPath))
                throw new ArgumentNullException(nameof(inpImgPath));

            if (string.IsNullOrEmpty(inpModelPath))
                throw new ArgumentNullException(nameof(inpModelPath));

            if (string.IsNullOrEmpty(inpXmlStatisticsPath))
                throw new ArgumentNullException(nameof(inpXmlStatisticsPath));

            ImageClassificationRequest request = new ImageClassificationRequest()
            {
                Id = Guid.NewGuid(),
                InpImgPath = inpImgPath,
                InpModelPath = inpModelPath,
                InpXmlStatisticsPath = inpXmlStatisticsPath,
            };

            (string raster, string confidenceMap) = await orfeoToolboxToolsService.ImageClassification(request);

            return (raster, confidenceMap);
        }

        //public IActionResult PreTrainedDetection()
        //{
        //    List<PreTrainedDetectionExample> examples = new List<PreTrainedDetectionExample>()
        //    {
        //        new PreTrainedDetectionExample(),
        //        new PreTrainedDetectionExample()
        //    };
        //    return View();
        //}
    }
}
