using CliWrap;
using WasteDetection.Da;
using WasteDetection.Models.Entities;

namespace WasteDetection.Services
{
    public class OrfeoToolboxToolsService
    {
        private readonly SettingsService _settingsService;
        private readonly DataContext _dataContext;

        public OrfeoToolboxToolsService(SettingsService settingsService, DataContext dataContext)
        {
            _settingsService = settingsService;
            _dataContext = dataContext;
        }

        public async Task<string> ComputeImageStatistics(string inpImgPath)
        {
            if (string.IsNullOrEmpty(inpImgPath))
                throw new ArgumentNullException(nameof(inpImgPath));

            string originalFileName = inpImgPath;
            string originalExtension = System.IO.Path.GetExtension(inpImgPath);

            string outXmlPathBase = _settingsService.GetOutBasePathByOrfeoToolboxToolName("ComputeImageStatistics");
            string outXmlPath = Path.Combine(outXmlPathBase, Guid.NewGuid() + ".xml");

            // Not for now, pointless
            //string guidFilename = Guid.NewGuid() + originalExtension;
            //string inpImgPathToSave = Path.Combine(outXmlPathBase, guidFilename);
            //using (FileStream output = System.IO.File.Create(inpImgPath))
            //await file.CopyToAsync(output);

            ComputeImageStatisticsRequest request = new ComputeImageStatisticsRequest()
            {
                Id = Guid.NewGuid(),
                InpImgPath = inpImgPath,
                OutXmlPath = outXmlPath,
                Suceeded = false,
                CreateOn = DateTime.Now,
            };

            _dataContext.ComputeImageStatisticsRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ComputeImageStatisticsRequest Not Inserted");

            string scriptName = _settingsService.GetScriptNameByOrfeoToolboxToolName("ComputeImageStatistics");
            string absoluteOutXmlPath = Path.Join(Environment.CurrentDirectory, "wwwroot", request.OutXmlPath);
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outXmlPathBase)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outXmlPathBase));

            string inputImagePathArg = $"-il \"{request.InpImgPath}\" ";
            string outputXmlPathArg = $"-out.xml \"{absoluteOutXmlPath}\" ";
            string ramArg = _settingsService.GetRamArgument();

            string commandArguments = $" {inputImagePathArg} {outputXmlPathArg} {ramArg}";

            CommandTask<CommandResult>? commandTask = 
                await GetConfiguredOrfeoToolboxCommandTask(scriptName, commandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Computing Image Statistics has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutXmlPath))
                throw new Exception($"Xml Statistics not created at path {absoluteOutXmlPath}");

            ComputeImageStatisticsRequest? requestFromDb =
                await _dataContext.ComputeImageStatisticsRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("ComputeImageStatisticsRequest Not Found in Db");
            
            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();
            //if(resultUpdate <= 0)

            return outXmlPath;
        }

        public async Task<(string, string)> TrainImageClassifier(TrainImageClassificatierRequest request)
        {
            //string commandArguments = "-io.il \"C:/Work Projects/WasteDetection/Data/1to10/1to10.tif\" -io.vd \"C:/Work Projects/WasteDetection/deponii_test/Trening/trening_klasi.shp\" \"C:/Work Projects/WasteDetection/deponii_test/Trening/trening_klasi_samo_deponii.shp\" -io.valid \"C:/Work Projects/WasteDetection/deponii_test/Trening/kontrolni_klasi.shp\" \"C:/Work Projects/WasteDetection/deponii_test/Trening/kontrolni_klasi_samo_deponii.shp\" -io.imstat \"C:/Work Projects/WasteDetection/Data/1to10/compute_image_statistics/1to10.xml\" -io.out \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/model_cli.mdl\" -io.confmatout \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/confusion_matrix/confusion_matrix_cli.xml\" -sample.vfn class -ram 256 -classifier rf ";
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            
            string scriptName = _settingsService.GetScriptNameByOrfeoToolboxToolName("TrainImageClassifier");
            string outBasePath = _settingsService.GetOutBasePathByOrfeoToolboxToolName("TrainImageClassifier");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string absoluteOutModelPath = 
                Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath, "model_" + outputGuid + ".mdl");
            string absoluteOutConfusionMatrixPath = 
                Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath, "confm_" + outputGuid + ".xml");

            request.OutModelPath = Path.Join(outBasePath, "model_" + outputGuid + ".mdl"); ;
            request.OutConfusionMatrixPath = Path.Join(outBasePath, "confm_" + outputGuid + ".xml");
            request.TrainingClassifierName = request.TrainingClassifierName ?? "rf";
            request.LabelField = request.LabelField ?? "class";

            _dataContext.TrainImageClassificatierRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("TrainImageClassificatierRequest Not Inserted");

            string inpRasterArg = $"-io.il \"{request.InpImgPath}\" ";
            string inpVectorArg = $"-io.vd \"{request.InpVectorPath}\" ";
            string validationVectorArg = $"-io.valid \"{request.ValidationVectorPath}\" ";
            string inputXmlStatisticsArg = $"-io.imstat \"{request.InpXmlStatisticsPath}\" ";
            string outputModelArg = $"-io.out \"{absoluteOutModelPath}\" ";
            string outputConfusionMatrixArg = $"-io.confmatout \"{absoluteOutConfusionMatrixPath}\" ";
            string labelFieldArg = $"-sample.vfn {request.LabelField}";
            string ramArg = "-ram 256 ";
            string classifierNameArg = $"-classifier {request.TrainingClassifierName}";

            string trainingCommandArguments = 
                $"{inpRasterArg} {inpVectorArg} {validationVectorArg} {inputXmlStatisticsArg} {outputModelArg} {outputConfusionMatrixArg} {labelFieldArg} {ramArg} {classifierNameArg} ";
            CommandTask<CommandResult>? commandTask =
                await GetConfiguredOrfeoToolboxCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Training has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutModelPath))
                throw new Exception($"Model not created at path {absoluteOutModelPath}");

            TrainImageClassificatierRequest? requestFromDb =
                await _dataContext.TrainImageClassificatierRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("TrainImageClassificatierRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();
            //if(resultUpdate <= 0)

            return await Task.FromResult((request.OutModelPath, absoluteOutModelPath));
        }

        public async Task<(string, string)> ImageClassification(ImageClassificationRequest request)
        {
            /*
             * string commandArguments = "-in \"C:/Work Projects/WasteDetection/Data/1to10/1to10.tif\" 
             * -model \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/model.mdl\" 
             * -imstat \"C:/Work Projects/WasteDetection/Data/1to10/compute_image_statistics/1to10.xml\" 
             * -out \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/out_raster/ImageClassifier_e13afc97-9535-43ef-bcfd-f491d9c3aad9_cli.tif\" 
             * uint8 -confmap \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/confidence_map/confidence_map_cli.tif\" double -ram 256 ";
             */
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            string scriptName = _settingsService.GetScriptNameByOrfeoToolboxToolName("ImageClassifier");
            string outBasePath = _settingsService.GetOutBasePathByOrfeoToolboxToolName("ImageClassifier");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string absoluteOutRasterPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath, "classification_" + outputGuid + ".tif");
            string absoluteOutConfidenceMapPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath, "confidence_" + outputGuid + ".tif");

            request.OutRasterPath = absoluteOutRasterPath;
            request.OutConfidenceMapPath = absoluteOutConfidenceMapPath;

            string inpImageArg = $"-in \"{request.InpImgPath}\" ";
            string inpModelArg = $"-model \"{request.InpModelPath}\" ";
            string inputXmlStatisticsArg = $"-imstat \"{request.InpXmlStatisticsPath}\" ";
            string outputRasterArg = $"-out \"{request.OutRasterPath}\" ";
            string outputConfidenceMapArg = $"uint8 -confmap  \"{request.OutConfidenceMapPath}\" ";
            string ramArg = "double -ram 256 ";

            string trainingCommandArguments =
                $"{inpImageArg} {inpModelArg} {inputXmlStatisticsArg} {outputRasterArg} {outputConfidenceMapArg} {ramArg} ";
            CommandTask<CommandResult>? commandTask =
                await GetConfiguredOrfeoToolboxCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Training has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutRasterPath))
                throw new Exception($"Can't find Classified Image Raster");

            if (!File.Exists(absoluteOutConfidenceMapPath))
                throw new Exception($"Can't find Classified Image Confidence Map");

            return (absoluteOutRasterPath, absoluteOutConfidenceMapPath);
        }

        private async Task<CommandTask<CommandResult>> GetConfiguredOrfeoToolboxCommandTask(string scriptName, 
                                                                                            string commandArguments)
        {
            if (string.IsNullOrEmpty(scriptName))
                throw new ArgumentNullException(nameof(scriptName));

            using CancellationTokenSource forcefulCts = new CancellationTokenSource();
            using CancellationTokenSource gracefulCts = new CancellationTokenSource();

            // Cancel forcefully after a timeout of 10 seconds
            forcefulCts.CancelAfter(TimeSpan.FromSeconds(360));

            // Cancel gracefully after a timeout of 180 seconds.
            // If the process takes too long to respond to graceful cancellation,
            // it will eventually get killed by forceful cancellation configured above.
            gracefulCts.CancelAfter(TimeSpan.FromSeconds(180));

            string? orfeoToolboxPath = _settingsService.GetOrfeoToolboxToolsPath();

            string targetProgram = Path.Combine(orfeoToolboxPath, scriptName);

            CommandTask<CommandResult>? commandTask =
                Cli.Wrap(targetProgram)
                    .WithWorkingDirectory(Environment.CurrentDirectory)
                    .WithArguments(commandArguments)
                    .WithValidation(CommandResultValidation.None)
                    .WithStandardOutputPipe(PipeTarget.ToFile("\\logs\\stdoutOrfeo.txt"))
                    .WithStandardErrorPipe(PipeTarget.ToFile("\\logs\\ErrorLogCliWrapOrfeo.txt"))
                    .ExecuteAsync();
            //.ExecuteAsync(forcefulCts.Token, gracefulCts.Token);

            if (commandTask is null)
                throw new Exception("Unable to get Configured Cli Command Task");

            return await Task.FromResult(commandTask);
        }
    }
}
