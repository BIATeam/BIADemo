namespace BIA.Net.Core.Domain.File
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FileDownloadData
    {
        public required string FileName { get; set; }
        public required byte[] FileContent { get; set; }
        public required string FileContentType { get; set; }
        public string Token { get; set; }
        public string FilePath { get; set; }
    }
}
