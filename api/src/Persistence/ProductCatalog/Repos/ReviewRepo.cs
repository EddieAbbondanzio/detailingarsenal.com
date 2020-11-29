using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.ProductCatalog;
using System.Linq;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class ReviewRepo : DatabaseInteractor, IReviewRepo {
        public ReviewRepo(IDatabase database) : base(database) {
        }

        public async Task<Review?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                return (await conn.QueryAsync<ReviewRow>(
                    @"select * from reviews where id = @Id;", new { Id = id }
                )).Select(r => new Review(r.UserId, r.PadId, r.CreatedDate, r.Stars, r.Cut, r.Finish, r.Title, r.Body)).First();
            }
        }

        public async Task Add(Review review) {
            var row = new ReviewRow() {
                Id = review.Id,
                UserId = review.UserId,
                CreatedDate = review.CreatedDate,
                Stars = review.Stars,
                Cut = review.Cut,
                Finish = review.Finish,
                Title = review.Title,
                Body = review.Body
            };

            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"insert into reviews 
                        (id, user_id, pad_id, created_date, stars, cut, finish, title, body)
                        values (@Id, @UserId, @PadId, @CreatedDate, @Stars, @Cut, @Finish, @Title, @Body);",
                        row
                );
            }

            throw new NotImplementedException();
        }

        public Task Update(Review entity) {
            throw new NotImplementedException();
        }

        public async Task Delete(Review entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync("delete from reviews where id = @Id;", entity);
            }
        }
    }
}