namespace LibraryApi.Domain.Library.Interfaces
{
    public interface IOperation<TRequest, TResponse, DBContext>
    {
        List<TResponse> GetAllData(DBContext context);
        List<TResponse> GetAllData();
        TResponse GetDataById(Guid paramId, DBContext context);
        TResponse GetDataById(string paramTxtId);
        string Update(TRequest paramData);
        string Update(TRequest paramData, DBContext context);
        string Delete(string paramTxtId);
        void Delete(Guid paramId, DBContext context);
        string Insert(TRequest paramData);
        string Insert(TRequest paramData, DBContext context);
    }
}