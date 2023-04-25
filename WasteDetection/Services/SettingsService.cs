namespace WasteDetection.Services
{
    public class SettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetOrfeoToolboxPath()
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

        public string GetScriptNameByToolName(string toolName)
        {
            string? scriptName = _configuration.GetValue<string>($"OrfeoToolBoxTools:{toolName}:ScriptName");

            if (string.IsNullOrEmpty(scriptName))
                throw new Exception("ScriptName Not Found");

            return scriptName;
        }

        public string GetOutBasePathByToolName(string toolName)
        {
            string? outBasePath = _configuration.GetValue<string>($"OrfeoToolBoxTools:{toolName}:OutBasePath");

            if (string.IsNullOrEmpty(outBasePath))
                throw new Exception("OutBasePath Not Found");

            return outBasePath;
        }
    }   
}
