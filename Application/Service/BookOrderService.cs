using AutoMapper;
using LibraryApi.Domain.Library;
using LibraryApi.Domain.Library.Interfaces;
using LibraryApi.Domain.Models;
using LibraryApi.Domain.Request;
using LibraryApi.Domain.Response;
using LibraryApi.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Application.Service
{
    public class BookOrderService
    {
        private readonly BookService _bookService;
        private readonly IMapper _mapper;
        // private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public BookOrderService(IMapper mapper,
            BookService bookService
            )
        {
            _mapper = mapper;
            _bookService = bookService;
            // _env = env;
        }

        public List<BookOrderResponse> GetAllDataByOrderId(string paramId)
        {
            using var context = new LibraryContext();
            try
            {
                var id = new Guid(paramId);
                return GetAllDataByOrderId(id, context);
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

        public List<BookOrderResponse> GetAllDataByOrderId(Guid paramId,LibraryContext context)
        {
            var data = context.BookOrders.Where(x => x.OrderId == paramId).ToList();

            List<BookOrderResponse> result = new List<BookOrderResponse>();
            foreach (var bookOrder in data)
            {
                var bookData = _bookService.GetDataById(bookOrder.BookId.ToString());
                if (bookData != null)
                {
                    var paramData = _mapper.Map<BookResponse, BookOrderResponse>(bookData);
                    paramData.BookOrderId = bookOrder. BookOrderId;
                    paramData.Status = bookOrder.Status;

                    result.Add(paramData);
                }
            }

            return result;
        }

        public List<BookOrderModels> GetAllDataByOrder(Guid paramId,LibraryContext context)
        {
            var data = context.BookOrders.Where(x => x.OrderId == paramId).Include(x => x.Book).ToList();

            return data;
        }

        public BookOrderResponse GetDataById(string paramTxtId)
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

        public BookOrderResponse GetDataById(Guid paramId, LibraryContext context)
        {
            var data = context.BookOrders.Where(x => x.BookOrderId == paramId).FirstOrDefault();

            var bookData = _bookService.GetDataById(data.BookId.ToString());
            BookOrderResponse result = new BookOrderResponse();
            if (bookData != null)
            {
                result = _mapper.Map<BookResponse, BookOrderResponse>(bookData);
                result.BookOrderId = data. BookOrderId;
                result.Status = data.Status;
            }

            return result;
        }

        public BookOrderModels Insert(Guid bookId, OrderModels orderModel)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                var model = Insert(bookId, orderModel, context);
                trans.Commit();

                return model;
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

        public BookOrderModels Insert(Guid bookId, OrderModels orderModel, LibraryContext context)
        {
            var data = new BookOrderModels();

            data.BookOrderId = Guid.NewGuid();
            data.BookId = bookId;
            data.Book = context.Books.Where(x => x.BookId == bookId).First();
            data.OrderId = orderModel.OrderId;
            data.Order = orderModel;
            data.Status = "Not Avalaible";

            context.BookOrders.Add(data);
            context.SaveChanges();

            return data;
        }

        public string UpdateStatus(Guid bookId, Guid orderId)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                var txtId = UpdateStatus(bookId, orderId, context);
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

        public string UpdateStatus(Guid bookId, Guid orderId, LibraryContext context)
        {
            var data = context.BookOrders.Where(x => x.BookId == bookId && x.OrderId == orderId).FirstOrDefault();
            if (data is not null)
            {
                data.Status = "Avalaible";

                context.BookOrders.Update(data);
                context.SaveChanges();
            }

            return data.BookOrderId.ToString();
        }

        public string Delete(Guid bookId, Guid orderId)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                Delete(bookId, orderId, context);
                trans.Commit();

                return "success";
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

        public void Delete(Guid bookId, Guid orderId, LibraryContext context)
        {
            var data = context.BookOrders.Where(x => x.BookId == bookId && x.OrderId == orderId).FirstOrDefault();
            if (data is not null)
            {
                context.BookOrders.Remove(data);
                context.SaveChanges();
            }
        }

        public List<BookOrderResponse> GetBooksByTitle(string searchKey, LibraryContext context)
        {
            var data = context.BookOrders.Include(x => x.Book)
                .Where(x => x.Book.Title.Contains(searchKey)).ToList();

            List<BookOrderResponse> result = new List<BookOrderResponse>();
            foreach (var item in data)
            {
                var bookData = _bookService.GetDataById(item.BookId.ToString());
                var checkOrder = context.Orders.Where(x => x.OrderId == item.OrderId).FirstOrDefault();
                if (bookData != null && checkOrder.DueDate.Date >= DateTime.UtcNow.Date)
                {
                    var paramData = _mapper.Map<BookResponse, BookOrderResponse>(bookData);
                    paramData.BookOrderId = item. BookOrderId;
                    paramData.Status = item.Status;

                    result.Add(paramData);
                }
            }

            return result;
        }
    }
}