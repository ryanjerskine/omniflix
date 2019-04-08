using System.Collections.Generic;

namespace omniflix.Common.Parsers.GoogleDrive
{
    public class VideoInfoParseResult
    {
        public string status { get; set; }
        public string hl { get; set; }
        public int allow_embed { get; set; }
        public string ps { get; set; }
        public int partnerid { get; set; }
        public int autoplay { get; set; }
        public string docid { get; set; }
        public bool @public { get; set; }
        public string el { get; set; }
        public string title { get; set; }
        public string BASE_URL { get; set; }
        public string iurl { get; set; }
        public string cc3_module { get; set; }
        public string ttsurl { get; set; }
        public string reportabuseurl { get; set; }
        public string fmt_list { get; set; }
        public int token { get; set; }
        public string plid { get; set; }
        public string fmt_stream_map { get; set; }
        public string url_encoded_fmt_stream_map { get; set; }
        public IEnumerable<Map> maps { get; set; }
        public long timestamp { get; set; }
        public long length_seconds { get; set; }
    }

    public class Map
    {
        public int itag { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string quality { get; set; }
    }
}