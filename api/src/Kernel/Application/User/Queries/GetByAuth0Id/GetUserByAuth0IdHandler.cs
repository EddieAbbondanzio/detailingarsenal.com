using System.Threading.Tasks;

public class GetUserByAuth0IdHandler : ActionHandler<GetUserByAuth0IdQuery, UserDto> {
    private IMapper mapper;

    public GetUserByAuth0IdHandler(IMapper mapper) {
        this.mapper = mapper;
    }

    public override Task<UserDto> Execute(GetUserByAuth0IdQuery input, User? user) {
        // Change this later.
        return Task.FromResult(mapper.Map<User, UserDto>(user!));
    }
}