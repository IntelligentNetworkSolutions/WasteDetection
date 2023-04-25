namespace WasteDetection.Models.Entities
{
    public class PreTrainedDetectionRequest
    {
        public Guid Id { get; set; }
        public string TifName { get; set; }
        public string TifPath { get; set; }

        public PreTrainedDetectionRequest(string tifName, string tifPath)
        {
            Id = Guid.NewGuid();
            TifName = tifName;
            TifPath = tifPath;
        }
    }
}
