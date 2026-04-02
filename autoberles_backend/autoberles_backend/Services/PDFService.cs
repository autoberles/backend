using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using autoberles_backend.Models;

public class PDFService
{
    public byte[] Generate(Rental rental, User user, Car car)
    {
        var generatedAt = DateTime.Now;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header().Column(col =>
                {
                    col.Item().Image("wwwroot/logo.png", ImageScaling.FitWidth);
                    col.Item().PaddingTop(10);
                    col.Item().Text("Autóbérlés visszaigazolás")
                        .FontSize(22).Bold().AlignCenter();
                    col.Item().PaddingBottom(20);
                });

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    col.Item().Text("Felhasználó adatai").Bold().FontSize(14);
                    col.Item().Column(inner =>
                    {
                        inner.Spacing(5);
                        inner.Item().Text($"Bérlő neve: {user.FirstName} {user.LastName}");
                        inner.Item().Text($"Email címe: {user.Email}");
                        inner.Item().Text($"Telefonszáma: {user.PhoneNumber}");
                    });

                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text("Autó adatai").Bold().FontSize(14);
                    col.Item().Column(inner =>
                    {
                        inner.Spacing(5);
                        inner.Item().Text($"Kibérelt autó: {car.Brand} {car.Model}");
                        inner.Item().Text($"Rendszám: {car.LicensePlate}");
                    });

                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text("Bérlés adatai").Bold().FontSize(14);
                    col.Item().Column(inner =>
                    {
                        inner.Spacing(5);
                        inner.Item().Text($"Bérlés kezdete: {rental.StartDate:yyyy-MM-dd}");
                        inner.Item().Text($"Bérlés vége: {rental.EndDate:yyyy-MM-dd}");
                    });

                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text($"Bérlés teljes ára: {rental.FullPrice} Ft")
                        .FontSize(16)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    col.Item().PaddingTop(10)
                        .Text($"Generálva: {generatedAt:yyyy-MM-dd HH:mm}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Darken2);

                    col.Item().PaddingTop(20).Background(Colors.Grey.Lighten4).Padding(12).Column(legal =>
                    {
                        legal.Spacing(6);
                        legal.Item().Text("Fontos információ").Bold().FontSize(11);
                        legal.Item().Text(
                            "A bérelt jármű a bérbeadó kizárólagos tulajdonát képezi. " +
                            "A bérlő a bérleti időszak alatt a járműben keletkezett minden kárért teljes anyagi felelősséggel tartozik, " +
                            "és köteles annak teljes összegét megtéríteni."
                        )
                        .FontSize(9).FontColor(Colors.Grey.Darken3);
                    });
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Köszönjük, hogy minket választott!")
                    .FontSize(10)
                    .Italic();
            });
        }).GeneratePdf();
    }
}