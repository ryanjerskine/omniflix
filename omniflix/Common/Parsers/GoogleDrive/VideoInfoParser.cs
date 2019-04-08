using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace omniflix.Common.Parsers.GoogleDrive
{
    public class VideoInfoParser
    {
        public VideoInfoParser()
        {

        }

        public VideoInfoParseResult Parse(string info)
        {
            var result = JsonConvert.DeserializeObject<VideoInfoParseResult>("{" + info.Replace("&", "\",").Replace("=", ":\"") + "\"}");
            // Decode Uris
            result.BASE_URL = Uri.UnescapeDataString(result.BASE_URL);
            result.cc3_module = Uri.UnescapeDataString(result.cc3_module);
            result.fmt_list = Uri.UnescapeDataString(result.fmt_list);
            result.fmt_stream_map = Uri.UnescapeDataString(result.fmt_stream_map);
            result.iurl = Uri.UnescapeDataString(result.iurl);
            result.reportabuseurl = Uri.UnescapeDataString(result.reportabuseurl);
            result.ttsurl = Uri.UnescapeDataString(result.ttsurl);
            result.title = Uri.UnescapeDataString(result.title);
            // Parse maps
            result.url_encoded_fmt_stream_map = Uri.UnescapeDataString(result.url_encoded_fmt_stream_map);
            var maps = JsonConvert.DeserializeObject<IEnumerable<Map>>("[{" + result.url_encoded_fmt_stream_map.Replace("=", ":\"").Replace(",", "\"},{").Replace("&", "\",") + "\"}]");
            foreach (var m in maps)
            {
                m.type = Uri.UnescapeDataString(m.type);
                m.url = Uri.UnescapeDataString(m.url);
            }
            result.maps = maps;
            // Return
            return result;
        }
    }
}