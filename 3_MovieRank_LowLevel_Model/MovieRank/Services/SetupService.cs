using MovieRank.Libs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Services
{
    public class SetupService : ISetupService
    {
        private readonly IMovieLLRankRepository _movieRankRepositoryRespository;

        public SetupService(IMovieLLRankRepository movieRankRepositoryRespository)
        {
            _movieRankRepositoryRespository = movieRankRepositoryRespository;
        }

        public async Task CreateDynamoDbTable(string dynamoDbTableName)
        {
            await _movieRankRepositoryRespository.CreateDynamoTable(dynamoDbTableName);
        }

        public async Task DeleteDynamoDbTable(string dynamoDbTableName)
        {
            await _movieRankRepositoryRespository.DeleteDynamoDbTable(dynamoDbTableName);
        }
    }
}
