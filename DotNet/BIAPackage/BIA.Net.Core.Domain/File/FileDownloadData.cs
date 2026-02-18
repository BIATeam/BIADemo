namespace BIA.Net.Core.Domain.File
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FileDownloadData
    {
        public Guid FileGuid { get; set; }
        public required string FileName { get; set; }
        public required string FileContentType { get; set; }
        public string FilePath { get; set; }
        public int RequestByUserId { get; set; }
    }
}
