using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebComicProvider.Domain.Comics;
using WebComicProvider.Domain.Repositories.Interfaces;

namespace WebComicProvider.Domain.Repositories.Comics
{
    public sealed class ComicRepository : SqlRepository<ComicRepository>, IComicRepository
    {
        public ComicRepository(IConfiguration configuration, ILogger<ComicRepository> logger) : base(configuration, logger) { }




        public async Task<IEnumerable<ComicModel>> GetAllComics()
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            return await connection.QueryAsync<ComicModel>("spGetAllComics", null,
                transaction, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<(ComicModel, Dictionary<IssueModel, IEnumerable<PageModel>>)> Get(int comicId)
        {
            (ComicModel, Dictionary<IssueModel, IEnumerable<PageModel>>) blank = default;
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            using var results = await connection.QueryMultipleAsync("spGetFullComicDetails", new { comicId }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            var comic = await results.ReadFirstOrDefaultAsync<ComicModel>();
            if (comic is null) return blank;
            var issues = await results.ReadAsync<IssueModel>();
            var pages = await results.ReadAsync<PageModel>();
            var issuesAndPages = issues.ToDictionary(c => c, c => pages.Where(p => p.IssueID == c.ID).OrderBy(p => p.Number).AsEnumerable());
            return (comic, issuesAndPages);
        }



    }
}
