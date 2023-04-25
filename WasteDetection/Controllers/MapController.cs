using CliWrap;
using Microsoft.AspNetCore.Mvc;

namespace WasteDetection.Controllers
{
    public class MapController : Controller
    {
        private readonly IConfiguration _configuration;
        public MapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> TrainImageClassifier()
        {
            string? orfeoToolboxPath = _configuration.GetValue<string>("OrfeoToolboxPath");

            string commandArguments = "-io.il \"C:/Work Projects/WasteDetection/Data/1to10/1to10.tif\" -io.vd \"C:/Work Projects/WasteDetection/deponii_test/Trening/trening_klasi.shp\" \"C:/Work Projects/WasteDetection/deponii_test/Trening/trening_klasi_samo_deponii.shp\" -io.valid \"C:/Work Projects/WasteDetection/deponii_test/Trening/kontrolni_klasi.shp\" \"C:/Work Projects/WasteDetection/deponii_test/Trening/kontrolni_klasi_samo_deponii.shp\" -io.imstat \"C:/Work Projects/WasteDetection/Data/1to10/compute_image_statistics/1to10.xml\" -io.out \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/model_cli.mdl\" -io.confmatout \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/confusion_matrix/confusion_matrix_cli.xml\" -sample.vfn class -ram 256 -classifier rf ";

            using CancellationTokenSource forcefulCts = new CancellationTokenSource();
            using CancellationTokenSource gracefulCts = new CancellationTokenSource();

            // Cancel forcefully after a timeout of 10 seconds
            forcefulCts.CancelAfter(TimeSpan.FromSeconds(360));

            // Cancel gracefully after a timeout of 180 seconds.
            // If the process takes too long to respond to graceful cancellation,
            // it will eventually get killed by forceful cancellation configured above.
            gracefulCts.CancelAfter(TimeSpan.FromSeconds(180));

            string targetProgram = orfeoToolboxPath + "\\otbcli_TrainImagesClassifier.bat";

            CommandTask<CommandResult>? commandTask =
                Cli.Wrap(targetProgram)
                    .WithWorkingDirectory(Environment.CurrentDirectory)
                    .WithArguments(commandArguments)
                    .WithValidation(CommandResultValidation.None)
                    .WithStandardErrorPipe(PipeTarget.ToFile("C:\\Logs\\WasteDetection\\ErrorLogCliWrap.txt"))
                    .ExecuteAsync(forcefulCts.Token, gracefulCts.Token);

            if (commandTask is null)
                return"Unable to get Configured Cli Command Task";

            CommandResult? commandResult = await commandTask;

            if (commandResult.ExitCode != 0)
                return $"Training has failed with exit code: {commandResult.ExitCode}";

            return "OK";
        }

        public async Task<string> ClassifyImage()
        {
            string? orfeoToolboxPath = _configuration.GetValue<string>("OrfeoToolboxPath");

            string commandArguments = "-in \"C:/Work Projects/WasteDetection/Data/1to10/1to10.tif\" -model \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/model.mdl\" -imstat \"C:/Work Projects/WasteDetection/Data/1to10/compute_image_statistics/1to10.xml\" -out \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/out_raster/ImageClassifier_e13afc97-9535-43ef-bcfd-f491d9c3aad9_cli.tif\" uint8 -confmap \"C:/Work Projects/WasteDetection/output/train_image_classifier_single_img/1to10/igorche_traning/confidence_map/confidence_map_cli.tif\" double -ram 256 ";

            using CancellationTokenSource forcefulCts = new CancellationTokenSource();
            using CancellationTokenSource gracefulCts = new CancellationTokenSource();

            // Cancel forcefully after a timeout of 10 seconds
            forcefulCts.CancelAfter(TimeSpan.FromSeconds(360));

            // Cancel gracefully after a timeout of 180 seconds.
            // If the process takes too long to respond to graceful cancellation,
            // it will eventually get killed by forceful cancellation configured above.
            gracefulCts.CancelAfter(TimeSpan.FromSeconds(180));

            string targetProgram = orfeoToolboxPath + "\\otbcli_ImageClassifier.bat";

            CommandTask<CommandResult>? commandTask =
                Cli.Wrap(targetProgram)
                    .WithWorkingDirectory(Environment.CurrentDirectory)
                    .WithArguments(commandArguments)
                    .WithValidation(CommandResultValidation.None)
                    .WithStandardErrorPipe(PipeTarget.ToFile("C:\\Logs\\WasteDetection\\ErrorLogCliWrapClassification.txt"))
                    .ExecuteAsync();
            //.ExecuteAsync(forcefulCts.Token, gracefulCts.Token);

            if (commandTask is null)
                return "Unable to get Configured Cli Command Task";

            CommandResult? commandResult = await commandTask;

            if (commandResult.ExitCode != 0)
                return $"Classification has failed with exit code: {commandResult.ExitCode}";

            return "OK";
        }
    }
}
