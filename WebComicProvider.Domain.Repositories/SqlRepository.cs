using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace WebComicProvider.Domain.Repositories
{

    /// <summary>
    /// The base repository class
    /// </summary>
    /// <typeparam name="T">The type of repository being implemented</typeparam>
    public abstract class SqlRepository<T>
    {

        /// <summary>
        /// Gets the name of the repository
        /// </summary>
        public string RepositoryName { get; init; }

        /// <summary>
        /// Gets the <see cref="ILogger"/> instance for this repository
        /// </summary>
        public ILogger<T> Logger { get; init; }


        private readonly string connectionString;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="repoName"></param>
        public SqlRepository(IConfiguration configuration, ILogger<T> logger)
        {
            connectionString = configuration.GetConnectionString("Main");
            Logger = logger;
            RepositoryName = nameof(T);
        }

        public SqlConnection GetConnection() => new(connectionString);

    }
}
