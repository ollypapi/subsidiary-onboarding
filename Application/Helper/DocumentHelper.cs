using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class DocumentHelper
    {
        private readonly IOnboardingDbContext _context;
        private readonly IExternalServiceInterface _externalService;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
       

        public DocumentHelper(IExternalServiceInterface externalService, IOnboardingDbContext context, IConfiguration configuration, IAccountService accountService)
        {
            _context = context;
            _externalService = externalService;
            _configuration = configuration;
            _accountService = accountService;
          
        }
        public async Task<ResponseModel> SaveIdentityDocument(long customerId, IdentificationModel doc)
        {
            doc.DocumentFolder = "IDcard";
            var docExist = await _context.Documents.AnyAsync(c => c.CustomerId == customerId && c.Identification != null && c.Status == DocumentStatus.Pending);
            if (docExist)
                throw new CustomException("Document with the same purpose already created..");
            var docs = new SaveFilesModel();
            docs.Folder = doc.DocumentFolder;
            docs.Files = new List<string>();
            docs.Files.Add(doc.DocumentName);

            var saveResponse = await _externalService.SaveFiles(docs);
            if (saveResponse == null)
                throw new CustomException("Attempt to move document failed...");

            if (!saveResponse.MigratedFiles.Contains(doc.DocumentName))
                throw new CustomException("Identity card not found...");

            var document = new Document
            {
                CustomerId = customerId,
                DateCreated = DateTime.Now,
                DocumentUrl = $"{doc.DocumentFolder}/{doc.DocumentName}",
                DocumentFolder = doc.DocumentFolder,
                DocumentName = doc.DocumentName,
                Identification = (Identification)doc.IdentificationType,
                IdentificationNumber = doc.IdentificationNumber,
                Status = DocumentStatus.Pending
            };
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }
        public async Task<List<Document>> GetCustomerDocuments(string AccountNumber)
        {
            return await _context.Documents.Where(c => c.Customer.AccountNumber == AccountNumber).ToListAsync();
        }

        public async Task<List<Document>> GetCustomerDocuments(long id)
        {
            return await _context.Documents.Where(c => c.Customer.Id == id).ToListAsync();
        }

        public async Task<Document> GetCustomerDocument(long documentId)
        {
            return await _context.Documents.FirstOrDefaultAsync(c => c.Id == documentId);
        }

        public async Task<ResponseModel> SaveDocument(long customerId, List<DocumentModel> documents)
        {
            var documentInfo = new List<Document>();
            var docs = new MoveFilesModel() ; 
            docs.fileModels = new List<FileModel>();
            foreach (var doc in documents)
            {
                doc.DocumentFolder = doc.DocumentType.GetDescription();
                docs.fileModels.Add(new FileModel { File =doc.DocumentName,Folder=doc.DocumentFolder});
            
                var docExist = await _context.Documents.AnyAsync(c => c.CustomerId == customerId && c.DocumentType == (DocumentType)doc.DocumentType && c.Status == DocumentStatus.Pending);
                if (!docExist)
                {
                    var docum = new Document
                    {
                        CustomerId = customerId,
                        DateCreated = DateTime.Now,
                        DocumentUrl = $"{doc.DocumentFolder}/{doc.DocumentName}",
                        DocumentFolder = doc.DocumentFolder,
                        DocumentName = doc.DocumentName,
                        DocumentType = (DocumentType)doc.DocumentType,
                        Status = DocumentStatus.Pending
                    };
                    documentInfo.Add(docum);
                }
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(docs);

            var saveResponse = await _externalService.MoveFiles(docs);
            var docProcessed = documentInfo.Select(c => c.DocumentName).Intersect(docs.fileModels.Select(d=>d.File));
            var docToSave = documentInfo.Where(c => docProcessed.Contains(c.DocumentName));

            _context.Documents.AddRange(docToSave);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful");
        }
        public string GetBase64Image(string imageFolderPath)
        {
            var baseUrl = $"{_configuration.GetValue<string>("Util:FileManagerBase")}";
            var request = WebRequest.Create($"{baseUrl}/{imageFolderPath}");
            var response = request.GetResponse().GetResponseStream();

            using (MemoryStream m = new MemoryStream())
            {
                response.CopyTo(m);
                byte[] imageBytes = m.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public async Task<Document> UpdateCustomerDocument(Document document)
        {
              _context.Documents.Update(document);
            _context.SaveChanges();
            return document;
        }
    }
}
