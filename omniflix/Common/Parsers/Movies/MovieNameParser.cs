using System.Text.RegularExpressions;

namespace omniflix.Common.Parsers.Movies
{
    public class MovieParser
    {
        public MovieParser()
        {

        }

        public MovieNameParseResult ParseFilename(string filename)
        {
            var withoutPath = System.IO.Path.GetFileNameWithoutExtension(filename);
            var result = new MovieNameParseResult()
            {
                Name = withoutPath.Replace(".", " "),
                Version = "Theatrical",
                Year = 0,
                Uhd = false
            };
            // Check for versions other than theatrical
            if (filename.Contains(" - Unrated"))
            {
                withoutPath = withoutPath.Replace(" - Unrated", "");
                result.Version = "Unrated";
            }
            else if (filename.Contains(" - Extended"))
            {
                withoutPath = withoutPath.Replace(" - Extended", "");
                result.Version = "Extended";
            }
            else if (filename.Contains(" - Directors"))
            {
                withoutPath = withoutPath.Replace(" - Directors", "");
                result.Version = "Directors";
            }
            else if (filename.Contains(" - Super.Duper"))
            {
                withoutPath = withoutPath.Replace(" - Super.Duper", "");
                result.Version = "Super Duper";
            }
            else if (filename.Contains("Ultimate"))
            {
                withoutPath = withoutPath.Replace(" - Ultimate", "");
                result.Version = "Ultimate";
            }
            else if (filename.Contains(" - Colorized"))
            {
                withoutPath = withoutPath.Replace(" - Colorized", "");
                result.Version = "Colorized";
            }
            else if (filename.Contains(" - Unrated.Directors"))
            {
                withoutPath = withoutPath.Replace(" - Unrated.Directors", "");
                result.Version = "Unrated Directors";
            }
            // Get the year
            var year = Regex.Match(filename, @"(?<=\().+?(?=\))").Value;
            if (!string.IsNullOrEmpty(year) && year.Length == 4)
            {
                result.Year = int.Parse(year);
                withoutPath = withoutPath.Replace($"({result.Year})", "");
            }
            // Check for 4K
            if (filename.Contains(" - 4K"))
            {
                withoutPath = withoutPath.Replace(" - 4K", "");
                result.Uhd = true;
            }
            // Filename
            result.Name = withoutPath.Replace(".", " ");
            return result;
        }
    }
}