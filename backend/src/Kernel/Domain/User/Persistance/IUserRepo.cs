using System.Threading.Tasks;

public interface IUserRepo : IRepo<User> {
    Task<User?> FindByAuth0Id(string id);
}