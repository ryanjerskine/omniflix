using Google.Apis.Drive.v3;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace omniflix.Features.Scans.Queries
{
    public class GetDriveMovies
    {

        public class Query : IRequest<IEnumerable<Google.Apis.Drive.v3.Data.File>>
        {
            public IEnumerable<string> FolderIds { get; }

            public Query(IEnumerable<string> folderIds)
            {
                this.FolderIds = FolderIds;
            }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<Google.Apis.Drive.v3.Data.File>>
        {
            private readonly ILogger<QueryHandler> _Logger;
            private readonly DriveService _DriveService;

            public QueryHandler(ILogger<QueryHandler> logger, DriveService driveService)
            {
                this._Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                this._DriveService = driveService ?? throw new System.ArgumentNullException(nameof(driveService));
            }

            public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> Handle(Query request, CancellationToken cancellationToken)
            {
                this._Logger.LogInformation($"Searching for movies in '{request.FolderIds.Count()}' movie folders.");
                var movieFiles = new List<Google.Apis.Drive.v3.Data.File>();
                foreach (var movieFolderId in request.FolderIds)
                {
                    string pageToken = null;
                    do
                    {
                        var req = this._DriveService.Files.List();
                        req.Q = $"'{movieFolderId}' in parents";
                        req.Spaces = "drive";
                        req.Fields = "nextPageToken, files(id, name)";
                        req.PageSize = 1000;
                        var result = await req.ExecuteAsync();
                        movieFiles.AddRange(result.Files);
                        pageToken = result.NextPageToken;
                    } while (pageToken != null);
                }
                this._Logger.LogInformation($"Found '{movieFiles.Count()}' movie files.");
                return movieFiles;
            }
        }
    }
}