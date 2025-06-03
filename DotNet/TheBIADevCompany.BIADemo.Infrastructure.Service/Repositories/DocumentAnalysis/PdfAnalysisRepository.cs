// BIADemo only
// <copyright file="PdfAnalysisRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Domain.DocumentAnalysis.Models;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using UglyToad.PdfPig;
    using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

    /// <summary>
    /// Document analysis repository for PDF files.
    /// </summary>
    public class PdfAnalysisRepository : IDocumentAnalysisRepository
    {
        /// <inheritdoc/>
        public DocumentType DocumentType => DocumentType.Pdf;

        /// <inheritdoc/>
        public List<DocumentPage> GetPagesContent(Stream stream)
        {
            var documentPages = new List<DocumentPage>();

            // Open the PDF document using PdfPig
            using var document = PdfDocument.Open(stream);

            // Iterate trought document's pages
            foreach (var page in document.GetPages())
            {
                // Getting all letters of current page
                // Optional: letters can be filtered by some criterias
                var letters = page.Letters;

                // Set word extractor based on current instance of PdfPig NearestNeighbourWordExtractor class
                // Optional: custom word extractor can be set
                var wordExtractor = NearestNeighbourWordExtractor.Instance;

                // Getting words using word extractor page's letters and map them into DocumentWord
                var documentWords = wordExtractor.GetWords(letters)
                    .Select(word => new DocumentWord
                    {
                        Text = word.Text,
                        PositionX = word.BoundingBox.BottomLeft.X,
                        PositionY = word.BoundingBox.BottomLeft.Y,
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
                UglyToad.PdfPig.Content.TextOrientation.Horizontal => TextOrientation.Horizontal,
                UglyToad.PdfPig.Content.TextOrientation.Rotate180 => TextOrientation.Rotated180,
                UglyToad.PdfPig.Content.TextOrientation.Rotate90 => TextOrientation.Rotated90,
                UglyToad.PdfPig.Content.TextOrientation.Rotate270 => TextOrientation.Rotated270,
                _ => TextOrientation.Other
            };
        }
    }
}
