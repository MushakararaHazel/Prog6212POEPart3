namespace CMCS.Models.ViewModels
{
    public class ReportVm
    {
        public List<AppUser> Users { get; set; } = new();
         
        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }
}
