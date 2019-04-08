using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace omniflix.Features.GoogleStreams
{
    [Route("api/playback")]
    [ApiController]
    public class VideoStreamsController : ControllerBase
    {
        private readonly Common.Parsers.GoogleDrive.VideoInfoParser _VideoInfoParser;
        private readonly Common.TokenManager _TokenManager;
        private readonly Common.Parsers.GoogleDrive.QualityMapper _QualityMapper;
        private readonly IHttpClientFactory _HttpClientFactory;

        public VideoStreamsController(
            Common.Parsers.GoogleDrive.VideoInfoParser videoInfoParser,
            Common.TokenManager tokenManager,
            Common.Parsers.GoogleDrive.QualityMapper qualityMapper,
            IHttpClientFactory httpClientFactory)
        {
            this._VideoInfoParser = videoInfoParser ?? throw new ArgumentNullException(nameof(videoInfoParser));
            this._TokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            this._QualityMapper = qualityMapper ?? throw new ArgumentNullException(nameof(qualityMapper));
            this._HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] string id, [FromQuery] int? quality)
        {
            var httpClient = this._HttpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = await this._TokenManager.GetGoogleDriveAuthHeaderAsync();
            var result = await httpClient.GetAsync($"https://drive.google.com/get_video_info?docid={id}");
            var actualResult = await result.Content.ReadAsStringAsync();
            var videoInfo = this._VideoInfoParser.Parse(actualResult);
            var q = videoInfo.maps.Where(m => m.quality == this._QualityMapper.MapQuality(quality)).FirstOrDefault();
            return new FileStreamResult(await httpClient.GetStreamAsync(q.url), "video/mp4");
        }
    }
}