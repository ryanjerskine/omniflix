using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace omniflix.Features.Scans.Queries
{
    public class GetMovieFolders
    {
        public class Query : IRequest<IEnumerable<string>>
        {
            
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<string>>
        {
            private readonly ILogger<QueryHandler> _Logger;

            public QueryHandler(ILogger<QueryHandler> logger)
            {
                this._Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }

            public async Task<IEnumerable<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Log
                this._Logger.LogInformation("Fetching list of Google Drive folders to scan for movies.");
                // TODO: This list will be stored in a database somewhere.
                var movieFolderIds = new List<string>();
                this._Logger.LogInformation($"Found '{movieFolderIds.Count()}' Google Drive folders that contain movies.");
                return movieFolderIds;
            }
        }
    }
}