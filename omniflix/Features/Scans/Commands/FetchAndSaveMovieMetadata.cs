using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace omniflix.Features.Scans.Commands
{
    public class FetchAndSaveMovieMetadata
    {
        public class Command : IRequest<Unit>
        {
            public readonly string Filename;
            public readonly string GoogleDriveId;

            public Command(string filename, string googleDriveId)
            {
                this.Filename = filename;
                this.GoogleDriveId = googleDriveId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Filename).NotNull().NotEmpty();
                RuleFor(x => x.GoogleDriveId).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILogger<Handler> _Logger;
            private readonly Common.Parsers.Movies.MovieParser _MovieParser;
            private readonly Common.Services.OmdbService _OmdbService;
            private readonly TMDbLib.Client.TMDbClient _TMDbClient;

            public Handler(
                ILogger<Handler> logger,
                Common.Parsers.Movies.MovieParser movieParser,
                Common.Services.OmdbService omdbService,
                TMDbLib.Client.TMDbClient tmdbClient)
            {
                this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
                this._MovieParser = movieParser ?? throw new ArgumentNullException(nameof(movieParser));
                this._MovieParser = movieParser ?? throw new ArgumentNullException(nameof(movieParser));
                this._TMDbClient = tmdbClient ?? throw new ArgumentNullException(nameof(tmdbClient));
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                /*
                // Initial info
                var movie = new object();
                movie.Title = "";
                movie.FileName = command.Filename;
                movie.GoogleDriveId = command.GoogleDriveId;
                
                // Initial match
                var parsedName = this._MovieParser.ParseFilename(movie.FileName);
                var bestResult = (await this._TMDbClient.SearchMovieAsync(parsedName.Name, year: parsedName.Year))?.Results?.FirstOrDefault();
                // Skip if no match
                if (bestResult == null)
                {
                    this._Logger.LogWarning($"Unable to automatically match file '{command.Filename}'.");
                    return Unit.Value;
                }
                // Make API calls
                var movieDetails = await this._TMDbClient.GetMovieAsync(bestResult.Id);
                var movieRelease = await this._TMDbClient.GetMovieReleaseDatesAsync(bestResult.Id);
                var omdbDetails = await this._OmdbService.GetMovieDetailsAsync(movie.ImdbId);
                if (movieDetails == null)
                {
                    this._Logger.LogWarning($"Matched file '{movie.FileName}' but unable to obtain metadata.");
                    return Unit.Value;
                }
                // Update metadata
                movie.MetadataMatch = true;
                movie.MetadataUpdated = DateTime.Now;
                movie.Budget = movieDetails.Budget;
                movie.TmdbId = movieDetails.Id;
                movie.ImdbId = movieDetails.ImdbId;
                movie.Summary = movieDetails.Overview;
                movie.TmdbPopularity = (decimal)movieDetails.Popularity;
                movie.ReleaseDate = movieDetails.ReleaseDate;
                movie.Revenue = movieDetails.Revenue;
                movie.RuntimeInMinutes = movieDetails.Runtime;
                movie.Tagline = movieDetails.Tagline;
                movie.Title = movieDetails.Title;
                movie.TmdbRating = (decimal)movieDetails.VoteAverage;
                movie.TmdbRatingVotes = movieDetails.VoteCount;
                movie.MpaaRating = "";
                // Add new genres
                var dbGenres = await this._MediaContext.Genres
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var genre in movieDetails.Genres)
                {
                    var dbGenre = dbGenres.FirstOrDefault(g => g.Name == genre.Name);
                    if (dbGenre != null)
                    {
                        await this._MediaContext.MovieGenres.AddAsync(new Data.Models.MovieGenre()
                        {
                            GenreId = dbGenre.Id,
                            MovieId = movie.Id
                        });
                    }
                }

                // Update ratings
                if (omdbDetails != null)
                {
                    movie.RatingUpdated = DateTime.Now;
                    movie.ImdbRating = (decimal)omdbDetails.imdbRating;
                    movie.ImdbVotes = omdbDetails.imdbVotes;
                    movie.RtTomatoMeter = omdbDetails.rottenTomatoesRating;
                    // TODO: Figure out if there is an API to get these from
                    // movie.RtAudienceScore = null;
                    // movie.RtAudienceVotes = null;
                    // movie.RtCriticVotes = null;
                }


                 // OTHER FIELDS:
                 // AccountStates, AlternativeTitles, Adult, BackdropPath, ExternalIds, Homepage, Images, Lists, MediaType, OriginalLanguage,
                 // OriginalTitle, PosterPath, ProductionCountries, ReleaseDates, Releases, Reviews, SpokenLanguages, Translations, Video, Videos

                // TODO: Implement production companies
                var productionCompanies = movieDetails.ProductionCompanies;
                // TODO: Implement recommendations/similar
                var recommendations = movieDetails.Recommendations;
                var similar = movieDetails.Similar;
                // TODO: Implement status (released?)
                var status = movieDetails.Status;
                // TODO: Implement keywords
                var keywords = movieDetails.Keywords;
                // TODO: Implement credits
                var credits = movieDetails.Credits;
                // TODO: Implement changes
                var changes = movieDetails.Changes;
                // TODO: Implement collections
                var collections = movieDetails.BelongsToCollection;

                // TODO: Save
                // await mediaContext.SaveChangesAsync();
                */

                return Unit.Value;
            }
        }
    }
}