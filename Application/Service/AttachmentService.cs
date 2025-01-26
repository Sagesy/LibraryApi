using LibraryApi.Domain.Library;
using LibraryApi.Domain.Library.Interfaces;
using LibraryApi.Domain.Models;
using LibraryApi.Infra.Context;
using Microsoft.AspNetCore.Http;

namespace LibraryApi.Application.Service
{
    public class AttachmentService
    {
        public AttachmentModels GetDataById(string paramTxtId)
        {
            using var context = new LibraryContext();
            try
            {
                Guid id = new Guid(paramTxtId);
                return GetDataById(id, context);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public AttachmentModels GetDataById(Guid paramId, LibraryContext context)
        {
            var data = context.Attachments.Where(x => x.AttachmentId == paramId).FirstOrDefault();
            return data;
        }


        public string Save(IFormFile formFile, AttachmentModels paramData, LibraryContext context
            , string contentRootPath)
        {
            paramData.AttachmentId = Guid.NewGuid();

            InsertAttachment(paramData, formFile, contentRootPath);
            paramData.OriginalNameFile = formFile.FileName;
            paramData.ContentType = formFile.ContentType;

            context.Attachments.Add(paramData);
            context.SaveChanges();

            return paramData.AttachmentId.ToString();
        }

        public static void InsertAttachment(AttachmentModels attachment, IFormFile file
            , string contentRootPath)
        {
            FileInfo fi = new FileInfo(file.FileName);
            var fileExt = fi.Extension;
            var fileName = Guid.NewGuid().ToString() + fileExt;
            var pathAttachment = GlobalConstant.PathAttachmentConstant.path;
            attachment.NameFile = CommonFunction.saveAttachment(file, pathAttachment, contentRootPath, fileName);
            pathAttachment = pathAttachment.Replace("~", "").Replace("/", Path.DirectorySeparatorChar.ToString());
            attachment.Url = contentRootPath + pathAttachment + fileName;
        }

        public static List<AttachmentModels> addAttachment(IEnumerable<IFormFile> fileData, AttachmentModels detailDoc
            , string contentRootPath)
        {
            using LibraryContext context = new LibraryContext();
            var attachmentTable = new List<AttachmentModels>();

            if (fileData != null)
            {
                foreach (IFormFile file in fileData)
                {
                    AttachmentModels attachment = new AttachmentModels();
                    InsertAttachment(attachment, file, contentRootPath);

                    attachment.TransactionType = detailDoc.TransactionType;

                    attachment.OriginalNameFile = file.FileName;
                    attachment.CreatedDate = DateTime.Now;
                    attachment.ContentType = file.ContentType;

                    context.Attachments.Add(attachment);
                }

                context.SaveChanges();
            }

            return attachmentTable;
        }

        public void Delete(Guid paramId, LibraryContext context)
        {
            var attachment = GetDataById(paramId, context);
            DeleteAttachment(attachment.Url);

            context.Attachments.Remove(attachment);
            context.SaveChanges();
        }

        public static void DeleteAttachment(string fileUrlOrPath)
        {
            try
            {
                if (File.Exists(fileUrlOrPath))
                {
                    File.Delete(fileUrlOrPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}