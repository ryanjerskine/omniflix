using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace omniflix.Features.Scans
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly ILogger<ScansController> _Logger;

        public ScansController(ILogger<ScansController> logger)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> PostAsync()
        {
            // TODO: Trigger another function that scans for metadata
            return new OkResult();
        }
    }
}