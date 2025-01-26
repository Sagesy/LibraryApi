using AutoMapper;
using LibraryApi.Domain.Library;
using LibraryApi.Domain.Library.Interfaces;
using LibraryApi.Domain.Models;
using LibraryApi.Domain.Request;
using LibraryApi.Domain.Response;
using LibraryApi.Infra.Context;

namespace LibraryApi.Application.Service
{
    public class BookService
    {
        private readonly AttachmentService _attachmentService;
        private readonly IMapper _mapper;
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public BookService(IMapper mapper,
            AttachmentService attachmentService,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env
            )
        {
            _mapper = mapper;
            _attachmentService = attachmentService;
            _env = env;
        }

        public List<BookResponse> GetAllData()
        {
            using var context = new LibraryContext();
            try
            {
                return GetAllData(context);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<BookResponse> GetAllData(LibraryContext context)
        {
            var data = context.Books.ToList();

            var result = _mapper.Map<List<BookModels>, List<BookResponse>>(data);

            return result;
        }

        public BookResponse GetDataById(string paramTxtId)
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

        public BookResponse GetDataById(Guid paramId, LibraryContext context)
        {
            var data = context.Books.Where(x => x.BookId == paramId).FirstOrDefault();

            var result = _mapper.Map<BookModels, BookResponse>(data);

            return result;
        }

        public string Insert(BookRequest paramData)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                var txtId = Insert(paramData, context);
                trans.Commit();

                return txtId;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                context.Dispose();
                trans.Dispose();
            }
        }

        public string Insert(BookRequest paramData, LibraryContext context)
        {
            //Validate dulu
            ValidateInput(paramData, context);

            var data = _mapper.Map<BookRequest, BookModels>(paramData);

            data.BookId = Guid.NewGuid();

            #region Insert Attachment if exist
            if (paramData.fileData != null)
            {
                foreach (var formFile in paramData.fileData)
                {
                    var attachment = new AttachmentModels();
                    attachment.AttachmentId = Guid.NewGuid();
                    attachment.TransactionType = "BOOK_COVER";
                    data.CoverId = _attachmentService.Save(formFile, attachment, context, this._env.ContentRootPath);
                }
            }
            #endregion

            context.Books.Add(data);
            context.SaveChanges();

            return data.BookId.ToString();
        }

        public string Update(ModifyBookRequest paramData)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                var txtId = Update(paramData, context);
                trans.Commit();

                return txtId;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                context.Dispose();
                trans.Dispose();
            }
        }

        public string Update(ModifyBookRequest paramData, LibraryContext context)
        {
            //Validate dulu
            ValidateInput(paramData, context);

            var data = context.Books.Where(x => x.BookId == paramData.BookId).FirstOrDefault();
            if (data is not null)
            {
                data.Title = paramData.Title;
                data.Publisher = paramData.Publisher;
                data.Author = paramData.Author;
                data.PublishedYear = paramData.PublishedYear;
                data.UpdatedDate = DateTime.UtcNow;


                #region Insert Attachment if exist
                if (paramData.fileData != null)
                {
                    var prevFile = context.Attachments.Where(x => x.AttachmentId == new Guid(data.CoverId)).FirstOrDefault();
                    CommonFunction.UpdateAttachment(prevFile.Url, prevFile.NameFile, this._env.ContentRootPath);

                    foreach (var formFile in paramData.fileData)
                    {
                        var attachment = new AttachmentModels();
                        attachment.AttachmentId = Guid.NewGuid();
                        attachment.TransactionType = "BOOK_COVER";
                        data.CoverId = _attachmentService.Save(formFile, attachment, context, this._env.ContentRootPath);
                    }
                }
                #endregion

                context.Books.Update(data);
                context.SaveChanges();
            }

            return paramData.BookId.ToString();
        }

        public string Delete(string paramTxtId)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                Guid id = new Guid(paramTxtId);
                Delete(id, context);
                trans.Commit();

                return paramTxtId;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                context.Dispose();
                trans.Dispose();
            }
        }

        public void Delete(Guid paramId, LibraryContext context)
        {
            var data = context.Books.Where(x => x.BookId == paramId).FirstOrDefault();
            if (data is not null)
            {
                if (!string.IsNullOrEmpty(data.CoverId))
                {
                    var cover = context.Attachments.Where(x => x.AttachmentId == new Guid(data.CoverId)).FirstOrDefault();
                    if (File.Exists(cover.Url)) File.Delete(cover.Url);
                    context.Attachments.Remove(cover);
                }

                context.Books.Remove(data);
                context.SaveChanges();
            }
        }

        private static bool ValidateInput(BookRequest dat, LibraryContext context)
        {
            if (string.IsNullOrEmpty(dat.Title))
            {
                throw new Exception("Title Tidak boleh kosong!");
            }

            if (string.IsNullOrEmpty(dat.Author))
            {
                throw new Exception("Author Tidak boleh kosong!");
            }

            if (string.IsNullOrEmpty(dat.Publisher))
            {
                throw new Exception("Publisher tidak boleh kosong!");
            }

            if (dat.PublishedYear <= 0 )
            {
                throw new Exception("Published Year tidak boleh kosong!");
            }

            if (dat.fileData is null )
            {
                throw new Exception("Cover tidak boleh kosong!");
            }

            if (context.Books.Any(x => x.Title == dat.Title && x.Author == dat.Author))
            {
                throw new Exception($"Author {dat.Author} dengan Title {dat.Title} sudah exist");
            }

            return true;
        }

        private static bool ValidateInput(ModifyBookRequest dat, LibraryContext context)
        {
            if (string.IsNullOrEmpty(dat.Title))
            {
                throw new Exception("Title Tidak boleh kosong!");
            }

            if (string.IsNullOrEmpty(dat.Author))
            {
                throw new Exception("Author Tidak boleh kosong!");
            }

            if (string.IsNullOrEmpty(dat.Publisher))
            {
                throw new Exception("Publisher tidak boleh kosong!");
            }

            if (dat.PublishedYear <= 0 )
            {
                throw new Exception("Published Year tidak boleh kosong!");
            }

            if (dat.fileData is null )
            {
                throw new Exception("Cover tidak boleh kosong!");
            }

            if (context.Books.Any(x => x.BookId != dat.BookId && x.Title == dat.Title && x.Author == dat.Author))
            {
                throw new Exception($"Author {dat.Author} dengan Title {dat.Title} sudah exist");
            }

            return true;
        }
    }
}