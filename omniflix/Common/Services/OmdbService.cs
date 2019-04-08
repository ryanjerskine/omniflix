using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace omniflix.Common.Services
{
    public class OmdbService
    {
        private readonly TokenManager _ApiTokens;
        private readonly IHttpClientFactory _HttpClientFactory;

        public OmdbService(IHttpClientFactory httpClientFactory, TokenManager apiTokens)
        {
            this._ApiTokens = apiTokens ?? throw new ArgumentNullException(nameof(apiTokens));
            this._HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<OmdbResponse> GetMovieDetailsAsync(string imdbId)
        {
            var httpClient = this._HttpClientFactory.CreateClient();
            var result = await httpClient.GetAsync($"https://omdbapi.com/?i={imdbId}&plot=full&r=json&apikey={this._ApiTokens.OmdbApiKey}");
            if (!result.IsSuccessStatusCode) return null;
            try
            {
                return await result.Content.ReadAsAsync<OmdbResponse>();
            }
            catch
            {
                return null;
            }
        }
    }

    public class OmdbResponse
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Rated { get; set; }
        public DateTime? Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public List<OmdbRating> Ratings { get; set; }
        public string Metascore { get; set; }
        public double imdbRating { get; set; }
        public long? imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public DateTime? DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public bool? Response { get; set; }
        public int? rottenTomatoesRating
        {
            get
            {
                var val = this.Ratings.Where(r => r.Source == "Rotten Tomatoes").FirstOrDefault()?.Value;
                if (string.IsNullOrEmpty(val)) return null;
                try
                {
                    return int.Parse(val.Replace("%", ""));
                }
                catch
                {
                    return null;
                }
            }
        }
        public int? metacriticRating
        {
            get
            {
                var val = this.Ratings.Where(r => r.Source == "Metacritic").FirstOrDefault()?.Value;
                if (string.IsNullOrEmpty(val)) return null;
                try
                {
                    return int.Parse(val.Split('/')[0]);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
    public class OmdbRating
    {
        public string Source { get; set; }
        public string Value { get; set; }
    }
}