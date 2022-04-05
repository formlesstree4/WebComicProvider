using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebComicProvider.Domain.Repositories.Comics
{
    internal sealed class ComicRepository : SqlRepository<ComicRepository>
    {
        public ComicRepository(IConfiguration configuration, ILogger<ComicRepository> logger) : base(configuration, logger)
        {
        }
    }
}
