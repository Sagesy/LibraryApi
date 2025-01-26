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
    public class OrderService
    {
        private readonly BookOrderService _bookOrderService;
        private readonly IMapper _mapper;
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public OrderService(IMapper mapper,
            BookOrderService bookOrderService,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env
            )
        {
            _mapper = mapper;
            _bookOrderService = bookOrderService;
            _env = env;
        }

        public List<OrderResponse> GetAllData()
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

        public List<OrderResponse> GetAllData(LibraryContext context)
        {
            var datas = context.Orders.ToList();

            List<OrderResponse> result = new List<OrderResponse>();
            foreach (var data in datas)
            {
                var bookOrderData = _bookOrderService.GetAllDataByOrderId(data.OrderId, context);
                if (bookOrderData != null || bookOrderData.Count > 0)
                {
                    var paramData = _mapper.Map<OrderModels, OrderResponse>(data);
                    paramData.BookOrders = bookOrderData;

                    result.Add(paramData);
                }
            }

            return result;
        }

        public OrderResponse GetDataById(string paramTxtId)
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

        public OrderResponse GetDataById(Guid paramId, LibraryContext context)
        {
            var data = context.Orders.Where(x => x.OrderId == paramId).FirstOrDefault();

            OrderResponse result = new OrderResponse();

            var bookOrderData = _bookOrderService.GetAllDataByOrderId(data.OrderId, context);
            if (bookOrderData != null || bookOrderData.Count > 0)
            {
                result = _mapper.Map<OrderModels, OrderResponse>(data);
                result.BookOrders = bookOrderData;
            }

            return result;
        }

        public string Insert(OrderRequest paramData)
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

        public string Insert(OrderRequest paramData, LibraryContext context)
        {
            //Validate dulu
            //ValidateInput(paramData, context);
            var data = _mapper.Map<OrderRequest, OrderModels>(paramData);

            data.OrderId = Guid.NewGuid();
            data.DueDate = DateTime.UtcNow.AddDays(paramData.Duration);

            context.Orders.Add(data);
            context.SaveChanges();

            List<BookOrderModels> bookOrders = new List<BookOrderModels>();
            foreach (var id in paramData.BookIds)
            {
                var bookOrderData = _bookOrderService.Insert(id, data, context);
                bookOrders.Add(bookOrderData);
            }

            return data.OrderId.ToString();
        }

        public string Update(ModifyOrderRequest paramData)
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

        public string Update(ModifyOrderRequest paramData, LibraryContext context)
        {
            //Validate dulu
            //ValidateInput(paramData, context);

            var data = context.Orders.Where(x => x.OrderId == paramData.OrderId).FirstOrDefault();
            if (data is not null)
            {
                data.OrderName = paramData.OrderName;
                data.Duration = paramData.Duration;
                data.DueDate = DateTime.UtcNow.AddDays(paramData.Duration);
                data.UpdatedDate = DateTime.UtcNow;

                context.Orders.Update(data);

                var bookOrderData = _bookOrderService.GetAllDataByOrder(data.OrderId, context);

                #region Insert / Delete BookOrder if exist
                foreach (var item in paramData.BookIds)
                {
                    if (!bookOrderData.Any(x => x.BookId == item))
                    {
                        var newBookOrder = _bookOrderService.Insert(item, data, context);
                        bookOrderData.Add(newBookOrder);
                    }
                }

                foreach (var item in bookOrderData)
                {
                    if (!paramData.BookIds.Any(x => x == item.BookId))
                    {
                        _bookOrderService.Delete(item.BookId, data.OrderId, context);
                        bookOrderData.Remove(item);
                    }
                }
                #endregion

                context.SaveChanges();
            }

            return paramData.OrderId.ToString();
        }

        public string UpdateStatus(string id, UpdateStatusOrderRequest paramData)
        {
            using var context = new LibraryContext();
            using var trans = context.Database.BeginTransaction();
            try
            {
                var paramId = new Guid(id);
                var txtId = UpdateStatus(paramId, paramData, context);
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

        public string UpdateStatus(Guid paramId, UpdateStatusOrderRequest paramData, LibraryContext context)
        {
            //Validate dulu
            //ValidateInput(paramData, context);

            var data = context.Orders.Where(x => x.OrderId == paramId).FirstOrDefault();

            context.Orders.Update(data);

            if (data is not null)
            {
                foreach (var item in paramData.BookIds)
                {
                    var bookOrderData = _bookOrderService.UpdateStatus(item, data.OrderId, context);
                    data.BookOrders.Where(x => x.BookId == item).First().Status = "Avalaible";
                }

                context.SaveChanges();
            }

            return paramId.ToString();
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
            var data = context.Orders.Where(x => x.OrderId == paramId).FirstOrDefault();
            if (data is not null)
            {
                if (data.BookOrders.Any())
                {
                    foreach (var item in data.BookOrders)
                    {
                        _bookOrderService.Delete(item.BookId, paramId, context);
                    }
                }

                context.Orders.Remove(data);
                context.SaveChanges();
            }
        }

        public List<BookOrderResponse> SearchBookByTitle(string searchKey)
        {
            using var context = new LibraryContext();
            try
            {
                return _bookOrderService.GetBooksByTitle(searchKey, context);
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
    }
}