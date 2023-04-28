using CliWrap;
using Microsoft.AspNetCore.Mvc;
using WasteDetection.Models.Entities;
using WasteDetection.Services;

namespace WasteDetection.Controllers
{
    public class DetectionController : Controller
    {
        private readonly SettingsService _settingsService;
        private OrfeoToolboxToolsService _orfeoToolboxToolsService;
        private GDALToolsService _GDALToolsService;

        public DetectionController(SettingsService settingsService, 
            OrfeoToolboxToolsService orfeoToolboxToolsService, GDALToolsService gDALToolsService)
        {
            _settingsService = settingsService;
            this._orfeoToolboxToolsService = orfeoToolboxToolsService;
            _GDALToolsService = gDALToolsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Steps()
        {
            return View();
        }

        #region TranslateLayer
        [HttpGet]
        public IActionResult TranslateLayer()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> TranslateLayer(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            string resultFilePath = await _GDALToolsService.TranslateLayer(inpLayerPath);

            return resultFilePath;
        }
        #endregion

        #region BuildPyramids
        [HttpGet]
        public IActionResult BuildPyramids()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> BuildPyramids(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            string resultFilePath = await _GDALToolsService.BuildPyramids(inpLayerPath);

            return resultFilePath;
        }
        #endregion

        #region ComputeImageStatistics
        [HttpGet]
        public IActionResult ComputeImageStatistics()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> ComputeImageStatisticsRequest(string inpImgPath)
        {
            if(string.IsNullOrEmpty(inpImgPath))
                throw new ArgumentNullException(nameof(inpImgPath));

            string resultFilePath = await _orfeoToolboxToolsService.ComputeImageStatistics(inpImgPath);

            return resultFilePath;
        }
        #endregion

        #region TrainImageClassifier
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

            (string modelRelativePath, string modelAbsolutePath) = await _orfeoToolboxToolsService.TrainImageClassifier(request);

            return modelRelativePath + "---" + modelAbsolutePath;
        }
        #endregion

        #region ImageClassifier
        [HttpGet]
        public IActionResult ImageClassifier()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> ImageClassifier(string inpImgPath, string inpModelPath, string inpXmlStatisticsPath)
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

            (string rasterRelPath, string rasterAbsPath, string confidenceMapRelPath, string confidenceMapAbsPath) = 
                await _orfeoToolboxToolsService.ImageClassification(request);

            return rasterRelPath + "---" + rasterAbsPath + "---" + confidenceMapRelPath + "---" + confidenceMapAbsPath;
        }
        #endregion

        #region RasterCalculator
        [HttpGet]
        public IActionResult RasterCalculator()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> RasterCalculator(string inpLayerAPath, string inpLayerBPath)
        {
            if (string.IsNullOrEmpty(inpLayerAPath))
                throw new ArgumentNullException(nameof(inpLayerAPath));

            if (string.IsNullOrEmpty(inpLayerBPath))
                throw new ArgumentNullException(nameof(inpLayerBPath));

            (string resultFileRelPath, string resultFileAbsPath) = await _GDALToolsService.RasterCalculator(inpLayerAPath, inpLayerBPath);

            return resultFileRelPath + "---" + resultFileAbsPath;
        }
        #endregion

        #region Sieve
        [HttpGet]
        public IActionResult Sieve()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Sieve(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            (string resultFileRelativePath, string resultFileAbsolutePath) = await _GDALToolsService.Sieve(inpLayerPath);

            return resultFileRelativePath + "---" + resultFileAbsolutePath;
        }
        #endregion

        #region Contour
        [HttpGet]
        public IActionResult Contour()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Contour(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            string resultFilePath = await _GDALToolsService.Contour(inpLayerPath);

            return resultFilePath;
        }
        #endregion

        #region Polygonize
        [HttpGet]
        public IActionResult Polygonize()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Polygonize(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            string resultFilePath = await _GDALToolsService.Polygonize(inpLayerPath);

            return resultFilePath;
        }
        #endregion

        public async Task<string> OneForAllForOne(string inpImgPath)
        {
            //inpImgPath = "C:\\Visual Studio Projects\\WasteDetection\\WasteDetection\\wwwroot\\detection\\prepared_inputs\\1to10.tif";

            //string transaltedInpImgAbsPath = await _GDALToolsService.TranslateLayer(inpImgPath);

            //await _GDALToolsService.BuildPyramids(transaltedInpImgAbsPath);

            string xmlStatisticsRelPath = await _orfeoToolboxToolsService.ComputeImageStatistics(inpImgPath);
            string xmlStatisticsAbsPath = Path.Join(Environment.CurrentDirectory, "wwwroot", xmlStatisticsRelPath);

            string trainingVectorAbsPath = Path.Join(Environment.CurrentDirectory, "wwwroot", "\\detection\\prepared_inputs\\training_layers\\training_classes.shp");
            string validationVectorAbsPath = Path.Join(Environment.CurrentDirectory, "wwwroot", "\\detection\\prepared_inputs\\control_layers\\control_classes.shp");
            TrainImageClassificatierRequest trainImageClassificatierRequest = new TrainImageClassificatierRequest()
            {
                Id = Guid.NewGuid(),
                CreateOn = DateTime.Now,
                InpImgPath = inpImgPath,
                InpVectorPath = trainingVectorAbsPath,
                ValidationVectorPath = validationVectorAbsPath,
                InpXmlStatisticsPath = xmlStatisticsAbsPath,
            };
            (string modelRelPath, string modelAbsPath) =
                await _orfeoToolboxToolsService.TrainImageClassifier(trainImageClassificatierRequest);

            ImageClassificationRequest imageClassificationRequest = new ImageClassificationRequest()
            {
                Id = Guid.NewGuid(),
                CreateOn = DateTime.Now,
                InpImgPath = inpImgPath,
                InpModelPath = modelAbsPath,
                InpXmlStatisticsPath= xmlStatisticsAbsPath,
                Suceeded= false,
            };

            (string outRasterRelPath, string outRasterAbsPath, string outConfidenceMapRelPath, string outConfidenceMapAbsPath) =
                await _orfeoToolboxToolsService.ImageClassification(imageClassificationRequest);

            (string outRasterCalculatorRelPath, string outRasterCalculatorAbsPath) =
                await _GDALToolsService.RasterCalculator(outRasterAbsPath, outConfidenceMapAbsPath);

            (string outSieverelPath, string outSieveAbsPath) = await _GDALToolsService.Sieve(outRasterCalculatorAbsPath);

            string outPolygonizeRelPath = await _GDALToolsService.Polygonize(outSieveAbsPath);
            return outPolygonizeRelPath;
        }
    }
}
