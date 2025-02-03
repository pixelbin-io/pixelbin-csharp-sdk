using System;
using System.Collections.Generic;
using System.Linq;
using Pixelbin.Common.Exceptions;

namespace Pixelbin.Utils
{
    internal static class UrlParts
    {
        internal static UrlObj GetUrlParts(string pixelbinUrl, UrlConfig config)
        {
            var parsedUrl = new Uri(pixelbinUrl);
            var urlDetails = new UrlObj
            {
                protocol = parsedUrl.Scheme,
                host = parsedUrl.Host,
                search = parsedUrl.Query,
                version = "v1",
                worker = false,
                workerPath = ""
            };

            List<string> parts = parsedUrl.AbsolutePath.Split("/").ToList();

            if (config.isCustomDomain ?? false)
            {
                // parsing custom domain urls
                if (RegexUtils.version2Regex.IsMatch(parts[1]))
                {
                    urlDetails.version = parts.Splice(1, 1)[0];
                }
                else
                {
                    throw new PDKInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");
                }

                if (RegexUtils.CustomDomainRegex.UrlWithWorkerAndZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.zone = parts.Splice(1, 1)[0];
                    urlDetails.pattern = "";
                    urlDetails.filePath = "";
                    urlDetails.worker = true;
                    urlDetails.workerPath = string.Join("/", parts.GetRange(2, parts.Count - 2));
                }
                else if (RegexUtils.CustomDomainRegex.UrlWithWorker.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.pattern = "";
                    urlDetails.filePath = "";
                    urlDetails.worker = true;
                    urlDetails.workerPath = string.Join("/", parts.GetRange(2, parts.Count - 2));
                }
                else if (RegexUtils.CustomDomainRegex.UrlWithZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.zone = parts.Splice(1, 1)[0];
                    urlDetails.pattern = parts.Splice(1, 1)[0];
                    urlDetails.filePath = string.Join("/", parts.GetRange(1, parts.Count - 1));
                }
                else if (RegexUtils.CustomDomainRegex.UrlWithoutZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.pattern = parts.Splice(1, 1)[0];
                    urlDetails.filePath = string.Join("/", parts.GetRange(1, parts.Count - 1));
                }
                else
                {
                    throw new PDKInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");
                }
            }
            else
            {
                // parsing pixelbin urls
                if (RegexUtils.version2Regex.IsMatch(parts[1]))
                    urlDetails.version = parts.Splice(1, 1)[0];

                if (parts.Count < 2 || string.IsNullOrWhiteSpace(parts[1]) || parts[1].Length < 3)
                    throw new PDKInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");

                if (RegexUtils.PixelbinDomainRegex.UrlWithWorkerAndZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.cloudName = parts.Splice(1, 1)[0];
                    urlDetails.zone = parts.Splice(1, 1)[0];
                    urlDetails.pattern = "";
                    urlDetails.filePath = "";
                    urlDetails.worker = true;
                    urlDetails.workerPath = string.Join("/", parts.GetRange(2, parts.Count - 2));
                }
                else if (RegexUtils.PixelbinDomainRegex.UrlWithWorker.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.cloudName = parts.Splice(1, 1)[0];
                    urlDetails.pattern = "";
                    urlDetails.filePath = "";
                    urlDetails.worker = true;
                    urlDetails.workerPath = string.Join("/", parts.GetRange(2, parts.Count - 2));
                }
                else if (RegexUtils.PixelbinDomainRegex.UrlWithZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.cloudName = parts.Splice(1, 1)[0];
                    urlDetails.zone = parts.Splice(1, 1)[0];
                    urlDetails.pattern = parts.Splice(1, 1)[0];
                    urlDetails.filePath = string.Join("/", parts.GetRange(1, parts.Count - 1));
                }
                else if (RegexUtils.PixelbinDomainRegex.UrlWithoutZone.IsMatch(string.Join("/", parts)))
                {
                    urlDetails.cloudName = parts.Splice(1, 1)[0];
                    urlDetails.pattern = parts.Splice(1, 1)[0];
                    urlDetails.filePath = string.Join("/", parts.GetRange(1, parts.Count - 1));
                }
                else
                {
                    throw new PDKInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");
                }
            }

            return urlDetails;
        }
    }
}