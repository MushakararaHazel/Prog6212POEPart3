using CMCS.Models;
using CMCS.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace CMCS.Reports
{
    public class ReportDocument : IDocument
    {
        private readonly ReportVm _vm;

        public ReportDocument(ReportVm vm)
        {
            _vm = vm;
        }

        public DocumentMetadata GetMetadata() => new DocumentMetadata
        {
            Title = "User Report",
            Author = "CMCS System",
            Subject = "User Listing",
            Keywords = "Users, CMCS, Report"
        };

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Header()
                    .Text("CMCS - User Report")
                    .FontSize(20)
                    .SemiBold()
                    .FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(10)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);   
                            columns.RelativeColumn();     
                            columns.RelativeColumn();     
                            columns.RelativeColumn();     
                            columns.RelativeColumn();     
                            columns.ConstantColumn(70);  
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("ID").Bold();
                            header.Cell().Text("Username").Bold();
                            header.Cell().Text("First Name").Bold();
                            header.Cell().Text("Last Name").Bold();
                            header.Cell().Text("Email").Bold();
                            header.Cell().Text("Rate").Bold();
                        });

                        // ROWS
                        foreach (var u in _vm.Users)
                        {
                            table.Cell().Text(u.Id.ToString());
                            table.Cell().Text(u.Username);
                            table.Cell().Text(u.FirstName ?? "-");
                            table.Cell().Text(u.LastName ?? "-");
                            table.Cell().Text(u.Email ?? "-");
                            table.Cell().Text(u.HourlyRate.ToString("N2"));
                        }

                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm}");
            });
        }
    }
}
