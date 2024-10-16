namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
    using UglyToad.PdfPig;

    public class PdfAnalysisRepository : IDocumentAnalysisRepository
    {
        public DocumentType DocumentType => DocumentType.Pdf;

        public List<DocumentPage> GetPagesContent(Stream stream)
        {
            var documentPages = new List<DocumentPage>();

            using var document = PdfDocument.Open(stream);
            foreach (var page in document.GetPages())
            {
                var letters = page.Letters;
                var wordExtractor = NearestNeighbourWordExtractor.Instance;
                var documentWords = wordExtractor.GetWords(letters)
                    .Select(word => new DocumentWord
                    {
                        Text = word.Text,
                        PositionX = word.BoundingBox.Left,
                        PositionY = word.BoundingBox.Bottom,
                        Rotation = word.BoundingBox.Rotation,
                        Height = word.BoundingBox.Height,
                        Orientation = GetTextOrientation(word.TextOrientation),
                    });

                documentPages.Add(new DocumentPage(page.Number, documentWords));
            }

            return documentPages;
        }

        private static TextOrientation GetTextOrientation(UglyToad.PdfPig.Content.TextOrientation orientation)
        {
            return orientation switch
            {
                UglyToad.PdfPig.Content.TextOrientation.Other => TextOrientation.Rotated,
                UglyToad.PdfPig.Content.TextOrientation.Horizontal => TextOrientation.Horizontal,
                UglyToad.PdfPig.Content.TextOrientation.Rotate180 => TextOrientation.Rotated180,
                UglyToad.PdfPig.Content.TextOrientation.Rotate90 => TextOrientation.Rotated90,
                UglyToad.PdfPig.Content.TextOrientation.Rotate270 => TextOrientation.Rotated270,
                _ => throw new NotImplementedException($"Unkwon PdfPig TextOrientation {orientation}."),
            };
        }
    }
}
