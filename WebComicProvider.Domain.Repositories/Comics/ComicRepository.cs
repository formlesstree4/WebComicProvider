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

        public async Task<ComicModel> GetComic(int comicId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            return await connection.QueryFirstAsync<ComicModel>("spGetComicDetails", new { comicId }, transaction, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<StatusModel>> GetComicStatuses()
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            return await connection.QueryAsync<StatusModel>("spGetStatuses", new { statusType = StatusTypes.Comic }, transaction, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> SaveComic(ComicModel comic)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var comicId = await connection.QueryFirstAsync<int>("spCreateComic", new
            {
                userId = comic.CreatedBy,
                name = comic.Name,
                description = comic.Description,
                status = comic.Status,
                cover = comic.Cover
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return comicId;
        }

        public async Task<int> SaveIssue(int comicId, IssueModel issue)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var issueId = await connection.QueryFirstAsync<int>("spCreateIssue", new
            {
                comicId = comicId,
                name = issue.Name,
                synopsis = issue.Synopsis,
                status = issue.Status,
                releaseDate = issue.ReleaseDate
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return issueId;
        }

        public async Task<int> SavePage(int comicId, int issueId, PageModel page)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var pageId = await connection.QueryFirstAsync<int>("spCreatePage", new
            {
                issueId = issueId,
                title = page.Title,
                toolTip = page.ToolTip,
                commentary = page.Commentary,
                status = page.Status,
                location = page.Location,
                releaseDate = page.ReleaseDate
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
            return pageId;
        }

    }
}
