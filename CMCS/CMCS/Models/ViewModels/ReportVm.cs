namespace CMCS.Models.ViewModels
{
    public class ReportVm
    {
        public List<CMCS.Models.AppUser> Users { get; set; } = new();
        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }
}
