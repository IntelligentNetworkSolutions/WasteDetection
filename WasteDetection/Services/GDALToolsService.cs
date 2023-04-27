﻿using CliWrap;
using WasteDetection.Da;
using WasteDetection.Models.Entities;

namespace WasteDetection.Services
{
    public class GDALToolsService
    {
        private readonly SettingsService _settingsService;
        private readonly DataContext _dataContext;

        public GDALToolsService(SettingsService settingsService, DataContext dataContext)
        {
            _settingsService = settingsService;
            _dataContext = dataContext;
        }

        public async Task<string> TranslateLayer(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            if(!File.Exists(inpLayerPath))
                throw new Exception($"File not found {inpLayerPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("Translate");
            string outBasePath = _settingsService.GetOutBasePathByGDALToolName("Translate");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string relativeOutTranslationPath = Path.Join(outBasePath, "translate_" + outputGuid + ".tif");
            string absoluteOutTranslationPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", relativeOutTranslationPath);

            TranslateLayerRequest request = new TranslateLayerRequest
            {
                Id = Guid.NewGuid(),
                InpLayerPath = inpLayerPath,
                OutLayerPath = relativeOutTranslationPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.TranslateLayerRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ComputeImageStatisticsRequest Not Inserted");

            string outputFormatArg = "-of GTiff ";
            string compressionAllOptionsArg = 
                " -co COMPRESS=DEFLATE -co PREDICTOR=2 -co ZLEVEL=9 -co TILED=YES -co NUM_THREADS=ALL_CPUS ";
            string inpLayerPathArg = $" \"{inpLayerPath}\" ";
            string outputTranslationPathArg = $" \"{absoluteOutTranslationPath}\" ";
            //"-of GTiff -co COMPRESS=DEFLATE -co PREDICTOR=2 -co ZLEVEL=9 -co TILED=YES -co NUM_THREADS=ALL_CPUS \"C:/Work Projects/WasteDetection/data/1to10/rendered/gdal/gdal_rendered.tif\" \"C:/Work Projects/WasteDetection/data/1to10/rendered/gdal/final_render_from_code_exe.tif\"";

            string trainingCommandArguments =
                $"{outputFormatArg} {compressionAllOptionsArg} {inpLayerPathArg} {outputTranslationPathArg} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Translation has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutTranslationPath))
                throw new Exception($"Can't find Translated Layer");

            TranslateLayerRequest? requestFromDb =
                await _dataContext.TranslateLayerRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("TranslateLayerRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return absoluteOutTranslationPath;
        }

        public async Task<string> BuildPyramids(string inpLayerPath)
        {
            //gdaladdo "C:/Work Projects/WasteDetection/data/1to10/rendered/gdal/final_render.tif" -r average -clean

            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            if (!File.Exists(inpLayerPath))
                throw new Exception($"File not found {inpLayerPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("Pyramids");

            string outputGuid = Guid.NewGuid().ToString();

            BuildPyramidsRequest request = new BuildPyramidsRequest
            {
                Id = Guid.NewGuid(),
                InpLayerPath = inpLayerPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.BuildPyramidsRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ComputeImageStatisticsRequest Not Inserted");

            string inpLayerPathArg = $" \"{inpLayerPath}\" ";
            string resamplingMethodArg = " -r average ";
            string cleanArg = " ";

            string trainingCommandArguments =
                $"{inpLayerPathArg} {resamplingMethodArg} {cleanArg} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Translation has failed with exit code: {trainingCommandResult.ExitCode}");

            BuildPyramidsRequest? requestFromDb =
                await _dataContext.BuildPyramidsRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("BuildPyramidsRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return inpLayerPath;
        }

        public async Task<string> RasterCalculator(string inpLayerAPath, string inpLayerBPath)
        {
            //gdal_calc.bat --overwrite --calc "where(((B > 0.9) & (isin(A, [1, 7]))), 1, 0)" --format GTiff --type UInt16 -A "C:/Visual Studio Projects/WasteDetection/WasteDetection/wwwroot/detection/image_classifier/calculated/classification_326eab5a-54d5-4fec-acfa-add74619ed11.tif" --A_band 1 -B "C:/Visual Studio Projects/WasteDetection/WasteDetection/wwwroot/detection/image_classifier/calculated/confidence_326eab5a-54d5-4fec-acfa-add74619ed11.tif" --B_band 1 --co COMPRESS=DEFLATE --co PREDICTOR=2 --co ZLEVEL=9 --co TILED=YES --co NUM_THREADS=ALL_CPUS --outfile "C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/calculator/final_calculator.tif"
            if (string.IsNullOrEmpty(inpLayerAPath))
                throw new ArgumentNullException(nameof(inpLayerAPath));

            if (!File.Exists(inpLayerAPath))
                throw new Exception($"File not found {inpLayerAPath}");

            if (string.IsNullOrEmpty(inpLayerBPath))
                throw new ArgumentNullException(nameof(inpLayerBPath));

            if (!File.Exists(inpLayerBPath))
                throw new Exception($"File not found {inpLayerBPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("RasterCalculator");
            string outBasePath = _settingsService.GetOutBasePathByGDALToolName("RasterCalculator");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string relativeOutRasterPath = Path.Join(outBasePath, "raster_calculation_" + outputGuid + ".tif");
            string absoluteOutRasterPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", relativeOutRasterPath);

            RasterCalculatorRequest request = new RasterCalculatorRequest
            {
                Id = Guid.NewGuid(),
                InpLayerAPath = inpLayerAPath,
                InpLayerBPath = inpLayerBPath,
                OutLayerPath = relativeOutRasterPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.RasterCalculatorRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ComputeImageStatisticsRequest Not Inserted");

            string overwriteArg = " --overwrite ";
            string calculationArg = " --calc \"where(((B > 0.9) & (isin(A, [1, 7]))), 1, 0)\" ";
            string outRasterFormatArg = " --format GTiff ";
            string outRasterTypeArg = " --type Int16 ";
            string inpLayerAPathArg = $" -A \"{request.InpLayerAPath}\" --A_band 1 ";
            string inpLayerBPathArg = $" -B \"{request.InpLayerBPath}\" --B_band 1 ";
            string compressAllArg = " --co COMPRESS=DEFLATE --co PREDICTOR=2 --co ZLEVEL=9 --co TILED=YES --co NUM_THREADS=ALL_CPUS ";
            string outRasterPathArg = $" --outfile \"{absoluteOutRasterPath}\" ";

            string trainingCommandArguments =
                $"{overwriteArg} {calculationArg} {outRasterFormatArg} {outRasterTypeArg} {inpLayerAPathArg} {inpLayerBPathArg} {compressAllArg} {outRasterPathArg} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments, ".bat");

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Raster Calculation has failed with exit code: {trainingCommandResult.ExitCode}");

            RasterCalculatorRequest? requestFromDb =
                await _dataContext.RasterCalculatorRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("RasterCalculatorRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return requestFromDb.OutLayerPath;
        }

        public async Task<string> Sieve(string inpLayerPath)
        {
            // gdal_sieve.bat -st 3 -4 -nomask -of GTiff D:/DeponiiBodanRezultat/pajtonashi.tif D:/DeponiiBodanRezultat/sieve_3x3.tif
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            if (!File.Exists(inpLayerPath))
                throw new Exception($"File not found {inpLayerPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("Sieve");
            string outBasePath = _settingsService.GetOutBasePathByGDALToolName("Sieve");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string relativeOutSievedPath = Path.Join(outBasePath, "sieve_" + outputGuid + ".tif");
            string absoluteOutSievedPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", relativeOutSievedPath);

            SieveRequest request = new SieveRequest
            {
                Id = Guid.NewGuid(),
                InpLayerPath = inpLayerPath,
                OutSievedPath = relativeOutSievedPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.SieveRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("SieveRequest Not Inserted");

            string thresholdArg = " -st 3 ";
            string defaultParametersArg = " -4 ";
            string outputFormatArg = " -of GTiff ";
            string inpLayerPathArg = $" \"{inpLayerPath}\" ";
            string outputSievedPathArg = $" \"{absoluteOutSievedPath}\" ";

            string trainingCommandArguments =
                $"{thresholdArg} {defaultParametersArg} {outputFormatArg} {inpLayerPathArg} {outputSievedPathArg} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments, ".bat");

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Sieving has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutSievedPath))
                throw new Exception($"Can't find Sieved Layer");

            SieveRequest? requestFromDb =
                await _dataContext.SieveRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("SieveRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return absoluteOutSievedPath;
        }

        public async Task<string> Contour(string inpLayerPath)
        {
            //gdal_contour -b 1 -a ELEV -i 1.0 -f "ESRI Shapefile" D:/DeponiiBodanRezultat/sieve_3x3.tif D:/DeponiiBodanRezultat/Contours.shp

            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            if (!File.Exists(inpLayerPath))
                throw new Exception($"File not found {inpLayerPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("Contour");
            string outBasePath = _settingsService.GetOutBasePathByGDALToolName("Contour");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string relativeOutCountoursPath = Path.Join(outBasePath, "contour_" + outputGuid + ".shp");
            string absoluteOutCountoursPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", relativeOutCountoursPath);

            ContourRequest request = new ContourRequest
            {
                Id = Guid.NewGuid(),
                InpLayerPath = inpLayerPath,
                OutContoursPath = relativeOutCountoursPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.ContourRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ContourRequest Not Inserted");

            string defaultParametersArg = " -b 1 -a ELEV -i 1.0 ";
            string outputFormatArg = " -f \"ESRI Shapefil\" ";
            string inpLayerPathArg = $" \"{inpLayerPath}\" ";
            string outputCountoursPathArg = $" \"{absoluteOutCountoursPath}\" ";

            string trainingCommandArguments =
                $"{defaultParametersArg} {outputFormatArg} {inpLayerPathArg} {outputCountoursPathArg} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Contouring has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutCountoursPath))
                throw new Exception($"Can't find Contoured Layer");

            ContourRequest? requestFromDb =
                await _dataContext.ContourRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("ContourRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return absoluteOutCountoursPath;
        }

        public async Task<string> Polygonize(string inpLayerPath)
        {
            if (string.IsNullOrEmpty(inpLayerPath))
                throw new ArgumentNullException(nameof(inpLayerPath));

            if (!File.Exists(inpLayerPath))
                throw new Exception($"File not found {inpLayerPath}");

            string scriptName = _settingsService.GetScriptNameByGDALToolName("Polygonize");
            string outBasePath = _settingsService.GetOutBasePathByGDALToolName("Polygonize");
            if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath)))
                Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "wwwroot", outBasePath));

            string outputGuid = Guid.NewGuid().ToString();
            string relativeOutPolygonizedPath = Path.Join(outBasePath, "polygonize_" + outputGuid + ".json");
            string absoluteOutPolygonizedPath =
                Path.Join(Environment.CurrentDirectory, "wwwroot", relativeOutPolygonizedPath);

            PolygonizeRequest request = new PolygonizeRequest
            {
                Id = Guid.NewGuid(),
                InpLayerPath = inpLayerPath,
                OutVectorizedPath = relativeOutPolygonizedPath,
                CreateOn = DateTime.Now,
                Suceeded = false,
            };

            _dataContext.PolygonizeRequests.Add(request);
            int resultAdd = await _dataContext.SaveChangesAsync();
            if (resultAdd <= 0)
                throw new Exception("ContourRequest Not Inserted");

            string inpLayerPathArg = $" \"{inpLayerPath}\" ";
            string defaultParametersArg = " -b 1 ";
            string outputVectorizedPathArg = $" -f \"GeoJSON\" \"{absoluteOutPolygonizedPath}\" ";
            string outputFieldName = "test class";

            string trainingCommandArguments =
                $"{inpLayerPathArg} {defaultParametersArg} {outputVectorizedPathArg} {outputFieldName} ";

            CommandTask<CommandResult>? commandTask =
                await GetConfiguredGDALCommandTask(scriptName, trainingCommandArguments);

            CommandResult? trainingCommandResult = await commandTask;

            if (trainingCommandResult.ExitCode != 0)
                throw new Exception($"Polygonizing has failed with exit code: {trainingCommandResult.ExitCode}");

            if (!File.Exists(absoluteOutPolygonizedPath))
                throw new Exception($"Can't find Polygonized Layer");

            PolygonizeRequest? requestFromDb =
                await _dataContext.PolygonizeRequests.FindAsync(request.Id);
            if (requestFromDb == null)
                throw new Exception("PolygonizeRequest Not Found in Db");

            requestFromDb.Suceeded = true;
            int resultUpdate = await _dataContext.SaveChangesAsync();

            return absoluteOutPolygonizedPath;
        }

        private async Task<CommandTask<CommandResult>> GetConfiguredGDALCommandTask(string scriptName,
                                                                                    string commandArguments,
                                                                                    string scriptType = null)
        {
            if (string.IsNullOrEmpty(scriptName))
                throw new ArgumentNullException(nameof(scriptName));

            using CancellationTokenSource forcefulCts = new CancellationTokenSource();
            using CancellationTokenSource gracefulCts = new CancellationTokenSource();

            // Cancel forcefully after a timeout of 10 seconds
            forcefulCts.CancelAfter(TimeSpan.FromHours(6));

            // Cancel gracefully after a timeout of 180 seconds.
            // If the process takes too long to respond to graceful cancellation,
            // it will eventually get killed by forceful cancellation configured above.
            gracefulCts.CancelAfter(TimeSpan.FromHours(4));

            string? gdalToolsExesPath = _settingsService.GetGDALToolsExesPath();
            string workingDirectory = Environment.CurrentDirectory;

            if (!string.IsNullOrEmpty(scriptType) && scriptType == ".bat")
            {
                gdalToolsExesPath = _settingsService.GetGDALToolsBatsPath();
                workingDirectory = gdalToolsExesPath;
            }

            //if (!string.IsNullOrEmpty(scriptType) && scriptType == ".py")
            //{
            //gdalToolsExesPath = "C:\\Program Files\\GDAL";
                //workingDirectory = gdalToolsExesPath;
            //}

            string targetProgram = Path.Combine(gdalToolsExesPath, scriptName);

            CommandTask<CommandResult>? commandTask =
                Cli.Wrap(targetProgram)
                    //.WithWorkingDirectory(workingDirectory)
                    .WithWorkingDirectory(Environment.CurrentDirectory)
                    .WithEnvironmentVariables(env => env
                        //.Set("Path", "C:\\Python311;C:\\Python311\\Scripts;C:\\Program Files\\GDAL;")
                        //.Set("PROJ_LIB", "C:\\Program Files\\GDAL\\projlib")
                        //.Set("GDAL_DATA", "C:\\Program Files\\GDAL\\gdal-data")
                        //.Set("GDAL_DRIVER_PATH", "C:\\Program Files\\GDAL\\gdalplugins")
                        .Set("Path", "C:\\Program Files\\QGIS 3.28.5\\apps\\Python39;C:\\Program Files\\QGIS 3.28.5\\apps\\Python39\\Scripts")
                        .Set("PROJ_LIB", "C:\\Program Files\\QGIS 3.28.5\\share\\proj")
                        .Set("GDAL_DATA", "C:\\Program Files\\QGIS 3.28.5\\apps\\gdal\\share\\gdal")
                        .Set("GDAL_DRIVER_PATH", "C:\\Program Files\\QGIS 3.28.5\\apps\\gdal\\lib\\gdalplugins")
                        .Set("%OSGEO4W_ROOT%", "C:\\Program Files\\QGIS 3.28.5")
                        .Set("OSGEO4W_ROOT", "C:\\Program Files\\QGIS 3.28.5")
                    )
                    .WithArguments(commandArguments)
                    .WithValidation(CommandResultValidation.None)
                    .WithStandardErrorPipe(PipeTarget.ToFile("C:\\Logs\\WasteDetection\\ErrorLogCliWrap.txt"))
                    .ExecuteAsync(forcefulCts.Token, gracefulCts.Token);

            if (commandTask is null)
                throw new Exception("Unable to get Configured Cli Command Task");

            return await Task.FromResult(commandTask);
        }

        
    }
}