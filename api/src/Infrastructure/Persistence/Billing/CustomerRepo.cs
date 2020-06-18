using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

public class CustomerRepo : DatabaseInteractor, ICustomerRepo {
    private IExternalCustomerGateway infoService;
    public CustomerRepo(IDatabase database, IExternalCustomerGateway infoService) : base(database) {
        this.infoService = infoService;
    }

    public async Task<Customer?> FindById(Guid id) {
        var c = await Connection.QueryFirstOrDefaultAsync<CustomerModel>(
            @"select * from customers where id = @Id;", new { Id = id }
        );

        if (c == null) {
            return null;
        }

        var info = await infoService.FindByExternalId(c.ExternalId);

        return Map(c, info);
    }

    public async Task Add(Customer entity) {
        entity.External = await infoService.Create(entity.External.Email);

        await Connection.ExecuteAsync(
            @"insert into customers (id, user_id, external_id) values (@Id, @UserId, @ExternalId);",
            new CustomerModel() { Id = entity.Id, UserId = entity.UserId, ExternalId = entity.External.Id }
        );
    }

    public async Task Update(Customer entity) {
        throw new NotImplementedException();
    }

    public async Task Delete(Customer entity) {
        throw new NotImplementedException();
    }

    private Customer Map(CustomerModel model, ExternalCustomer info) {
        return new Customer() {
            Id = model.Id,
            UserId = model.UserId,
            External = info
        };
    }
}