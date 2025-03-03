using DocumentManagement.Data;
using DocumentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        //gives access to the database
        private readonly AppDbContext _context;

        //Constructor will allow us to use appDbContext's Documents object into the repo
        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }
        //get all documents from the db
        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }
        //find document by id 
        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }
        //add document to db
        public async Task<Document> AddDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }
        //delete document from db
        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return;

            // Delete the file from the server
            if (File.Exists(document.FilePath))
            {
                File.Delete(document.FilePath);
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
        }

        //read the file content from disk
        public async Task<byte[]> GetFileContentAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null || !System.IO.File.Exists(document.FilePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            return await System.IO.File.ReadAllBytesAsync(document.FilePath);
        }

    }
}
