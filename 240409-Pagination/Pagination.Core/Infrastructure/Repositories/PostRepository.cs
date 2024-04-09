using FilteringPagination.Domain.Entities;
using FilteringPagination.Infrastructure.Database;

namespace FilteringPagination.Infrastructure.Repositories
{
    public class PostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public (Post[] data, int count) GetAll(int offset, int pageSize = 10)
        {
            var query = _context.Posts
                .OrderBy(x => x.Id);

            return (
                data: query
                    .Skip(offset)
                    .Take(pageSize)
                    .ToArray(),
                count: query.Count()
                );
        }

        public (Post[] data, int count) GetAll(
            (DateTime lastDate, int lastId) cursor,
            int pageSize = 10
            )
        {
            var query = _context.Posts
                .OrderBy(x => x.CreatedOn)
                .ThenBy(x => x.Id)
                .Where(x => x.CreatedOn > cursor.lastDate || (x.CreatedOn == cursor.lastDate && x.Id > cursor.lastId));

            return (
                data: query
                    .Take(pageSize)
                    .ToArray(),
                count: query.Count()
                );
        }
    }
}
