using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public interface IMovieRankRepository
    {
        Task<IEnumerable<Document>> GetAllItems();

        Task<Document> GetMovie(int userId, string movieName);

        Task<IEnumerable<Document>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);

        Task AddMovie(Document documentModel);

        Task UpdateMovie(Document documentModel);

        Task<IEnumerable<Document>> GetMovieRank(string movieName);
    }
}
