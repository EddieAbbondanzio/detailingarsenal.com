using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class ReviewReader : DatabaseInteractor, IReviewReader {
        public ReviewReader(IDatabase database) : base(database) {
        }

        public async Task<ReviewReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync(
                    @"select r.*, u.username from reviews r
                        join users u on r.user_id = u.id 
                        where id = @Id;",
                        new { Id = id }
                );

                return new ReviewReadModel(
                    r.pad_id,
                    r.username,
                    r.created_date,
                    r.stars,
                    r.cut,
                    r.finish,
                    r.title,
                    r.body
                );
            }
        }

        public async Task<List<ReviewReadModel>> ReadForPad(Guid padId) {
            using (var conn = OpenConnection()) {
                var reviews = await conn.QueryAsync(
                    @"select r.*, u.username from reviews r
                        join users u on r.user_id = u.id 
                        where pad_id = @Id",
                        new { Id = padId }
                );

                return reviews.Select(r => new ReviewReadModel(
                    r.pad_id,
                    r.username,
                    r.created_date,
                    r.stars,
                    r.cut,
                    r.finish,
                    r.title,
                    r.body
                )).ToList();
            }
        }
    }
}