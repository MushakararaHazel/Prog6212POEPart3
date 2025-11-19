using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using CMCS.Models.ViewModels;

namespace CMCS.Services
{
    public class ReportDocument : IDocument
    {
        private readonly ReportVm _model;

        public ReportDocument(ReportVm model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header().Text("CMCS User Report")
                    .FontSize(24).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Username").Bold();
                        header.Cell().Text("Name").Bold();
                        header.Cell().Text("Email").Bold();
                        header.Cell().Text("Rate").Bold();
                        header.Cell().Text("Role").Bold();
                    });

                    foreach (var u in _model.Users)
                    {
                        table.Cell().Text(u.Username);
                        table.Cell().Text($"{u.FirstName} {u.LastName}");
                        table.Cell().Text(u.Email);
                        table.Cell().Text(u.HourlyRate.ToString("N2"));
                        table.Cell().Text(u.Role.ToString());
                    }
                });

                page.Footer().AlignRight().Text($"Generated: {_model.GeneratedOn}");
            });
        }
    }
}
