using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidlyBackend.Models;

namespace VidlyBackend.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<MovieModel> _movies;

        public MovieService(IVidlyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _movies = database.GetCollection<MovieModel>(settings.CollectionName);
        }

        public List<MovieModel> Get() => _movies.Find(movie => true).ToList();
     
        public List<MovieModel> Get(string id) => _movies.Find(movie => movie.Id == id).ToList();

        public MovieModel Create(MovieModel movie)
        {
            if (movie.Id is null)
                movie.Id = ObjectId.GenerateNewId().ToString();

            _movies.InsertOne(movie);
            return movie;
        }

        public void Update(string id, MovieModel movieIn) =>
            _movies.ReplaceOne(movie => movie.Id == id, movieIn);

        public void Remove(MovieModel movieIn) =>
            _movies.DeleteOne(movie => movie.Id == movieIn.Id);

        public void Remove(string id) =>
            _movies.DeleteOne(movie => movie.Id == id);
    }
}
