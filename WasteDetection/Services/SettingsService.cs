namespace WasteDetection.Services
{
    public class SettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Orfeo Toolbox
        public string GetOrfeoToolboxToolsPath()
        {
            string? orfeoToolboxPath = _configuration.GetValue<string>("OrfeoToolboxPath");

            if (string.IsNullOrEmpty(orfeoToolboxPath))
                throw new Exception("OrfeoToolboxPath Not Found");

            return orfeoToolboxPath;
        }

        public string GetRamArgument()
        {
            string? ramArgument = _configuration.GetValue<string>("OrfeoToolBoxTools:RamArgument");

            if (string.IsNullOrEmpty(ramArgument))
                throw new Exception("Ram Argument Not Found");

            return ramArgument;
        }

        public string GetScriptNameByOrfeoToolboxToolName(string toolName)
        {
            string? scriptName = _configuration.GetValue<string>($"OrfeoToolBoxTools:{toolName}:ScriptName");

            if (string.IsNullOrEmpty(scriptName))
                throw new Exception("ScriptName Not Found");

            return scriptName;
        }

        public string GetOutBasePathByOrfeoToolboxToolName(string toolName)
        {
            string? outBasePath = _configuration.GetValue<string>($"OrfeoToolBoxTools:{toolName}:OutBasePath");

            if (string.IsNullOrEmpty(outBasePath))
                throw new Exception("OutBasePath Not Found");

            return outBasePath;
        }
        #endregion


        #region GDAL
        public string GetGDALToolsExesPath()
        {
            string? gdalToolsPath = _configuration.GetValue<string>("GDALToolsExesPath");

            if (string.IsNullOrEmpty(gdalToolsPath))
                throw new Exception("GDAL Tools .exe's Path Not Found");

            return gdalToolsPath;
        }

        public string GetGDALToolsBatsPath()
        {
            string? gdalToolsPath = _configuration.GetValue<string>("GDALToolsBatsPath");

            if (string.IsNullOrEmpty(gdalToolsPath))
                throw new Exception("GDAL Tools .bat's Path Not Found");

            return gdalToolsPath;
        }

        public string GetScriptNameByGDALToolName(string toolName)
        {
            string? scriptName = _configuration.GetValue<string>($"GDALTools:{toolName}:ScriptName");

            if (string.IsNullOrEmpty(scriptName))
                throw new Exception("ScriptName Not Found");

            return scriptName;
        }

        public string GetOutBasePathByGDALToolName(string toolName)
        {
            string? outBasePath = _configuration.GetValue<string>($"GDALTools:{toolName}:OutBasePath");

            if (string.IsNullOrEmpty(outBasePath))
                throw new Exception("OutBasePath Not Found");

            return outBasePath;
        }
        #endregion
    }
}
