using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Application.ProductCatalog;

namespace DetailingArsenal.Persistence.ProductCatalog {
    [DependencyInjection(RegisterAs = typeof(IReviewReader))]
    public class ReviewReader : DatabaseInteractor, IReviewReader {
        public ReviewReader(IDatabase database) : base(database) {
        }

        public async Task<ReviewReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                var r = await conn.QueryFirstOrDefaultAsync(
                    @"select r.*, u.username from reviews r
                        join users u on r.user_id = u.id 
                        where r.id = @Id;",
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

        public async Task<PadReviews> ReadForPad(Guid padId) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(@"
                    select r.*, u.username from reviews r
                        join users u on r.user_id = u.id 
                        where pad_id = @Id;
                    select avg(r.stars) from reviews r where pad_id = @Id;
                    select count(r.*) from reviews r where r.pad_id = @Id; 
                    select stars, count(*) as count, pad_id from reviews r 
                        left join pads p on r.pad_id = p.id 
                        where pad_id = @Id 
                        group by pad_id, stars; 

                ", new { Id = padId })) {
                    var reviews = reader.Read().Select(r => new ReviewReadModel(
                        r.pad_id,
                        r.username,
                        r.created_date,
                        r.stars,
                        r.cut,
                        r.finish,
                        r.title,
                        r.body
                    )).ToList();

                    var stars = reader.ReadFirstOrDefault<int?>();
                    var count = reader.ReadFirst<int>();

                    var stats = reader.Read().Select(s => new ReviewStarStat((int)s.stars, (int)s.count, (float)count / s.count));


                    return new PadReviews(new RatingReadModel(stars, count), reviews, stats);
                }
            }
        }
    }
}