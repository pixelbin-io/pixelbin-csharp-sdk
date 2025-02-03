using System.Text.RegularExpressions;

namespace Pixelbin.Utils
{
    public static class RegexUtils
    {
        public static Regex version2Regex { get; } = new Regex(@"^v[1-2]$");
        public static class PixelbinDomainRegex
        {
            public static Regex UrlWithZone { get; } = new Regex(@"^\/([a-zA-Z0-9_-]*)\/([a-zA-Z0-9_-]{6})\/(.+)\/(.*)$");
            public static Regex UrlWithoutZone { get; } = new Regex(@"^\/([a-zA-Z0-9_-]*)\/(.+)\/(.*)");
            public static Regex UrlWithWorkerAndZone { get; } = new Regex(@"^\/([a-zA-Z0-9_-]*)\/([a-zA-Z0-9_-]{6})\/wrkr\/(.*)$");
            public static Regex UrlWithWorker { get; } = new Regex(@"^\/([a-zA-Z0-9_-]*)\/wrkr\/(.*)$");
        };
        public static class CustomDomainRegex
        {
            public static Regex UrlWithZone { get; } = new Regex(@"^\/([a-zA-Z0-9_-]{6})\/(.+)\/(.*)$");
            public static Regex UrlWithoutZone { get; } = new Regex(@"^\/(.+)\/(.*)");
            public static Regex UrlWithWorkerAndZone { get; } = new Regex(@"^\/([a-zA-Z0-9_-]{6})\/wrkr\/(.*)$");
            public static Regex UrlWithWorker { get; } = new Regex(@"^\/wrkr\/(.*)$");
        };
        public static Regex zoneSlug = new Regex(@"([a-zA-Z0-9_-]{6})");
        public static bool Test(this string pattern, string input)
        {
            return Regex.IsMatch(input, pattern);
        }
    }
}