namespace MovieServiceWebAPI.Services
{
    public interface IRepository<T> 
    {
        List<T> GetAll();
        T GetById(string id);
        T Add(T entity);
        void Update(string id, T entity, ref bool isSuccess);
        void DeleteById(string id, ref bool isSuccess);
    }
}
