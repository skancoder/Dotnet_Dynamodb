using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Contracts;
using System.Collections.Generic;

namespace MovieRank.Libs.Mappers
{
    public interface IMapper
    {
        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items);
        MovieResponse ToMovieContract(Document movie);
        Document ToDocumentModel(int userId, MovieRankRequest addRequest);
        Document ToDocumentModel(int userId, MovieResponse movieResponse, MovieUpdateRequest movieUpdateRequest);
    }
}

