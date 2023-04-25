using System.ComponentModel.DataAnnotations;

namespace WasteDetection.Models.Entities
{
    public class ComputeImageStatisticsRequest
    {
        /*
         * otbcli_ComputeImagesStatistics.bat -il C:/Work Projects/WasteDetection/data/1to10/1to10.tif 
         * -out.xml C:/Work Projects/WasteDetection/data/1to10/compute_image_statistics/latest.xml -ram 256 
         * */
        [Key]
        public Guid Id { get; set; }
        public string InpImgPath { get; init; }
        public string OutXmlPath { get; init; }

        public bool Suceeded { get; set; } = false;
        public DateTime CreateOn { get; set; } = DateTime.Now;
    }
}
