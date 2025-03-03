using DocumentManagement.Models;

namespace DocumentManagement.Repositories
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync(); // List all documents
        Task<Document?> GetByIdAsync(int id); // Get file metadata by ID
        Task<Document> AddDocumentAsync(Document document); // Store file metadata
        Task DeleteDocumentAsync(int id); // Delete a file
        Task<byte[]> GetFileContentAsync(int id); // method for retrieving file content as bytes

    }
}
