﻿using MovieRank.Contracts;
using MovieRank.Integration.Tests.Setup;
using MovieRank.Libs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Scenarios
{
    [Collection("api")]
    public class MovieTests
    {
        private readonly TestContext _sut; // system under test

        public MovieTests(TestContext sut)
        {
            _sut = sut;
        }

        [Fact]
        public async Task AddMovieRankDataReturnsOkStatus()
        {
            const int userId = 1;
                    
            var response = await AddMovieRankData(userId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetAllItemsFromDatabaseReturnsNotNullMovieResponse()
        {
            const int userId = 2;

            await AddMovieRankData(userId);

            var response = await _sut.Client.GetAsync("movies");

            MovieResponse[] result;
            using(var content = response.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<MovieResponse[]>(await content);
            }

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetMovieReturnsExpectedMovieName()
        {
            const int userId = 3;
            const string movieName = "Test-GetMovieBack";

            await AddMovieRankData(userId, movieName);

            var response = await _sut.Client.GetAsync($"movies/{userId}/{movieName}");

            MovieResponse result;
            using (var content = response.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<MovieResponse>(await content);
            }

            Assert.Equal(movieName, result.MovieName);
        }

        [Fact]
        public async Task UpdateMovieReturnsUpdatedMovieRankValue()
        {
            const int userId = 4;
            const string movieName = "Test-UpdateMovie";
            const int ranking = 10;

            await AddMovieRankData(userId, movieName);

            var updateMovie = new MovieUpdateRequest
            {
                MovieName = movieName,
                Ranking = ranking
            };

            var json = JsonConvert.SerializeObject(updateMovie);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            await _sut.Client.PatchAsync($"movies/{userId}", stringContent);

            var response = await _sut.Client.GetAsync($"movies/{userId}/{movieName}");

            MovieResponse result;
            using (var content = response.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<MovieResponse>(await content);
            }

            Assert.Equal(ranking, result.Ranking);
        }

        [Fact]
        public async Task GetMoviesRankingReturnsAnOverallMovieRanking()
        {
            const int userId = 5;
            const string movieName = "Test-GetMovieOverallRanking";

            await AddMovieRankData(userId, movieName);

            var response = await _sut.Client.GetAsync($"movies/{movieName}/ranking");

            MovieRankResponse result;
            using (var content = response.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<MovieRankResponse>(await content);
            }

            Assert.NotNull(result);
        }

        private async Task<HttpResponseMessage> AddMovieRankData(int testUserId, string movieName = "Test-MovieName")
        {
            var movieDbData = new MovieDb
            {
                UserId = testUserId,
                MovieName = movieName,
                Description = "test-Description",
                Actors = new List<string>
                {
                    "testUser1",
                    "testUser2"
                },
                RankedDateTime = "5/10/2018 6:17:17 PM",
                Ranking = 4
            };

            var json = JsonConvert.SerializeObject(movieDbData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await _sut.Client.PostAsync($"movies/{testUserId}", stringContent);
        }
    }
}
