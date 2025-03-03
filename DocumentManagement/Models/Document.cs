namespace DocumentManagement.Models
{
    public class Document
    {
        public int Id { get; set; } // Auto-generated ID
        public string FileName { get; set; } // Original filename
        public string FilePath { get; set; } // Path where file is saved
        public string FileType { get; set; } // "pdf" or "txt"
        public DateTime UploadDate { get; set; } // Timestamp of upload
    }
}
