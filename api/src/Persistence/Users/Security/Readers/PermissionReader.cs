using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Application.Users.Security;

namespace DetailingArsenal.Persistence.Users.Security {
    [DependencyInjection(RegisterAs = typeof(IPermissionReader))]
    public class PermissionReader : DatabaseInteractor, IPermissionReader {
        public PermissionReader(IDatabase database) : base(database) { }

        public async Task<List<PermissionReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                return (await conn.QueryAsync<PermissionRow>("select * from permissions;")).
                Select(p => new PermissionReadModel(p.Id, p.Action, p.Scope)).ToList();
            }
        }

        public async Task<PermissionReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                var raw = await conn.QueryFirstOrDefaultAsync<PermissionRow>("select * from permissions where id = @Id;", new { Id = id });
                return raw == null ? null : new PermissionReadModel(raw.Id, raw.Action, raw.Scope);
            }
        }
    }
}