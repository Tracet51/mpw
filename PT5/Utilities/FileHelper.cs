using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MPW.Data;

namespace MPW.Utilities
{
    public static class MemoryStreamExtention
    {
        public static async Task<byte[]> ToArrayAsync(this MemoryStream memoryStream)
        {
            var task = await Task.Run(() =>
            {
               var byteArray = memoryStream.ToArray();

                return byteArray;
            });

            return task;
        }

        public static async Task<Document> FileToDocumentAsync(FileUpload fileUpload, Document document)
        {
            using (var stream = fileUpload.Document.OpenReadStream())
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                document.FileSize = fileUpload.Document.Length;
                document.FileType = fileUpload.Document.ContentType;
                document.Name = fileUpload.Document.FileName;
                document.Category = fileUpload.Category;
                document.UploadDate = DateTime.Now;
                document.File = await memoryStream.ToArrayAsync();

                return document;
            }
        }
    }
}
