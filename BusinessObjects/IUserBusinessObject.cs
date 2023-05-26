using AlticeApi.Data;

namespace AlticeApi.BusinessObjects;
public interface IUserBusinessObject{
    Task<string> GetUsers();
}