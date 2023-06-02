using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using System.Collections.Generic;

namespace MovieRank.Libs.Mappers
{
    public interface IMapper
    {
        IEnumerable<MovieResponse> ToMovieContract(ScanResponse response);

        MovieResponse ToMovieContract(GetItemResponse response);

        IEnumerable<MovieResponse> ToMovieContract(QueryResponse response);
    }
}

