using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pixelbin.Common.Exceptions;

namespace Pixelbin.Utils
{
    public class UrlObjOptions
    {
        public object? dpr { get; set; }
        public object? f_auto { get; set; }

        public UrlObjOptions(object? dpr = null, object? f_auto = null)
        {
            this.dpr = dpr;
            this.f_auto = f_auto;
        }

        public override int GetHashCode()
        {
            if (dpr == null && f_auto == null) return 0;

            int hash = 17;
            hash = hash * 23 + (dpr != null ? dpr.GetHashCode() : 0);
            hash = hash * 23 + (f_auto != null ? f_auto.GetHashCode() : 0);
            return hash;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UrlTransformation
    {
        public List<Dictionary<string, string>>? values { get; set; }
        public string? plugin { get; set; }
        public string? name { get; set; }

        public UrlTransformation(List<Dictionary<string, string>>? values = null, string? plugin = null, string? name = null)
        {
            this.values = values;
            this.plugin = plugin;
            this.name = name;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public static class UrlTransformationExtension
    {
        public static string GetString(this List<UrlTransformation> transformations)
        {
            return string.Concat(transformations.Select(o => o.ToString()));
        }
    }

    /// <summary>
    /// object used in constructing and deconstructing Pixelbin urls
    /// </summary>
    public class UrlObj : IEquatable<UrlObj>
    {
        public bool isCustomDomain { get; set; }
        public string protocol { get; set; }
        public string host { get; set; }
        public string search { get; set; }
        public bool worker { get; set; }
        public string workerPath { get; set; }

        /// <summary>
        /// cdn api version
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// Base url
        /// </summary>
        public string baseUrl { get; set; }
        /// <summary>
        /// Path to the file on Pixelbin storage
        /// </summary>
        public string filePath { get; set; }
        public string pattern { get; set; }
        /// <summary>
        /// The cloudname extracted from the url
        /// </summary>
        public string cloudName { get; set; }
        /// <summary>
        /// optional query parameters
        /// </summary>
        public UrlObjOptions? options { get; set; }
        /// <summary>
        /// 6 character zone slug
        /// </summary>
        public string zone { get; set; }
        /// <summary>
        /// Extracted transformations from the url
        /// </summary>
        public List<UrlTransformation>? transformations { get; set; }

        public UrlObj(
            string protocol = "",
            string host = "",
            string search = "",
            string workerPath = "",
            string version = "",
            string baseUrl = "",
            string filePath = "",
            string pattern = "",
            string cloudName = "",
            string zone = "",
            bool worker = false,
            bool isCustomDomain = false,
            UrlObjOptions? options = null,
            List<UrlTransformation>? transformations = null
        )
        {
            this.protocol = protocol;
            this.host = host;
            this.search = search;
            this.worker = worker;
            this.workerPath = workerPath;
            this.version = version;
            this.baseUrl = baseUrl;
            this.filePath = filePath;
            this.pattern = pattern;
            this.cloudName = cloudName;
            this.options = options;
            this.zone = zone;
            this.transformations = transformations;
            this.isCustomDomain = isCustomDomain;
        }

        public bool Equals(UrlObj other)
        {
            bool optionsValid = false;
            bool transformationsValid = false;

            if (options != null && other.options != null)
            {
                optionsValid = options.ToString() == other.options.ToString();
            }
            else if (options != null ^ other.options != null)
                optionsValid = false;

            if (transformations != null && other.transformations != null)
                transformationsValid = transformations.GetString() == other.transformations.GetString();
            else if (transformations != null ^ other.transformations != null)
                transformationsValid = false;

            return protocol == other.protocol &&
                    host == other.host &&
                    search == other.search &&
                    worker == other.worker &&
                    workerPath == other.workerPath &&
                    version == other.version &&
                    baseUrl == other.baseUrl &&
                    filePath == other.filePath &&
                    pattern == other.pattern &&
                    cloudName == other.cloudName &&
                    zone == other.zone &&
                    isCustomDomain == other.isCustomDomain &&
                    optionsValid && transformationsValid;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((UrlObj)obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 23 + (protocol != null ? protocol.GetHashCode() : 0);
            hash = hash * 23 + (host != null ? host.GetHashCode() : 0);
            hash = hash * 23 + (search != null ? search.GetHashCode() : 0);
            hash = hash * 23 + worker.GetHashCode();
            hash = hash * 23 + (workerPath != null ? workerPath.GetHashCode() : 0);
            hash = hash * 23 + (version != null ? version.GetHashCode() : 0);
            hash = hash * 23 + (baseUrl != null ? baseUrl.GetHashCode() : 0);
            hash = hash * 23 + (filePath != null ? filePath.GetHashCode() : 0);
            hash = hash * 23 + (pattern != null ? pattern.GetHashCode() : 0);
            hash = hash * 23 + (cloudName != null ? cloudName.GetHashCode() : 0);
            hash = hash * 23 + (zone != null ? zone.GetHashCode() : 0);
            hash = hash * 23 + isCustomDomain.GetHashCode();
            hash = hash * 23 + (options != null ? options.GetHashCode() : 0);
            hash = hash * 23 + (transformations != null ? transformations.GetString().GetHashCode() : 0);

            return hash;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public struct UrlConfig
    {
        public string? parameterSeparator;
        public string? operationSeparator;
        public bool? isCustomDomain;

        public UrlConfig(
            bool? isCustomDomain = null,
            string? parameterSeparator = null,
            string? operationSeparator = null
        )
        {
            this.isCustomDomain = isCustomDomain;
            this.parameterSeparator = parameterSeparator;
            this.operationSeparator = operationSeparator;
        }
    }

    internal static class Utils
    {
        const string OPERATION_SEPARATOR = "~";
        const string PARAMETER_SEPARATOR = ",";
        const string PARAMETER_LINK = ":";

        public static List<T> Splice<T>(this List<T> source, int start, int deleteCount)
        {
            List<T> items = source.GetRange(start, deleteCount);
            source.RemoveRange(start, deleteCount);
            return items;
        }

        internal static string GetUrlFromObj(UrlObj obj, UrlConfig config)
        {
            if (string.IsNullOrWhiteSpace(obj.baseUrl))
                obj.baseUrl = "https://cdn.pixelbin.io";

            if (!obj.isCustomDomain && string.IsNullOrWhiteSpace(obj.cloudName))
                throw new PDKIllegalArgumentError("key cloudName should be defined");

            if (obj.isCustomDomain && !string.IsNullOrWhiteSpace(obj.cloudName))
                throw new PDKIllegalArgumentError("key cloudName is not valid for custom domains");

            if (!obj.worker && string.IsNullOrWhiteSpace(obj.filePath))
                throw new PDKIllegalArgumentError("key filePath should be defined");

            if (obj.worker && string.IsNullOrWhiteSpace(obj.workerPath))
                throw new PDKIllegalArgumentError("key workerPath should be defined");

            if (obj.worker)
            {
                obj.pattern = "wrkr";
            }
            else
            {
                obj.pattern = GetPatternFromTransformations(obj.transformations, config) ?? "original";
            }

            if (string.IsNullOrWhiteSpace(obj.version) || !RegexUtils.version2Regex.IsMatch(obj.version))
                obj.version = "v2";

            if (string.IsNullOrWhiteSpace(obj.zone) || !RegexUtils.zoneSlug.IsMatch(obj.zone))
                obj.zone = "";

            List<string> urlArr = new List<string>() { obj.baseUrl, obj.version, obj.cloudName, obj.zone, obj.pattern };
            List<string> removeArr = new List<string>();

            if (obj.worker)
            {
                urlArr.Add(obj.workerPath);
            }
            else
            {
                urlArr.Add(obj.filePath);
            }

            foreach (string value in urlArr)
            {
                if (string.IsNullOrWhiteSpace(value))
                    removeArr.Add(value);
            }

            foreach (string value in removeArr)
            {
                urlArr.Remove(value);
            }

            List<string> queryArr = new List<string>();
            if (obj.options != null)
            {
                object? dpr = obj.options.dpr;
                object? f_auto = obj.options.f_auto;

                if (dpr != null)
                {
                    (string? strDpr, float? numDpr) = ParseDPR(dpr.ToString());
                    if (strDpr != null)
                        queryArr.Add($"dpr={strDpr}");
                    else
                        queryArr.Add($"dpr={(numDpr ?? 0.0f).ToString("0.0")}");
                }
                if (f_auto != null)
                {
                    ValidateFAuto(f_auto.ToString());
                    queryArr.Add($"f_auto={f_auto.ToString().ToLower()}");
                }
            }
            string urlStr = string.Join("/", urlArr);

            if (queryArr.Count > 0)
                urlStr += "?" + string.Join("&", queryArr);
            return urlStr;
        }

        private static UrlObj GetPartsFromUrl(string url, UrlConfig config)
        {
            UrlObj parts = UrlParts.GetUrlParts(url, config);
            UrlObjOptions queryObj = ProcessQueryParams(parts);

            return new UrlObj(
                baseUrl: $"{parts.protocol}://{parts.host}",
                filePath: parts.filePath,
                pattern: parts.pattern,
                version: parts.version,
                zone: parts.zone,
                cloudName: parts.cloudName,
                worker: parts.worker,
                workerPath: parts.workerPath,
                options: queryObj
            );
        }

        internal static UrlObj GetObjFromUrl(string url, UrlConfig config, bool flatten = false)
        {
            var parts = GetPartsFromUrl(url, config);
            try
            {
                parts.transformations = !string.IsNullOrWhiteSpace(parts.pattern)
                    ? GetTransformationDetailsFromPattern(parts.pattern, url, config, flatten)
                    : new List<UrlTransformation>();
            }
            catch (Exception)
            {
                throw new PDKInvalidUrlError($"Error Processing url. Please check the url is correct");
            }
            return parts;
        }

        private static string RemoveLeadingDash(string str)
        {
            if (str.Length > 0 && str[0] == '-')
            {
                return str.Substring(1);
            }
            return str;
        }

        private static List<string> GetParamsList(string dSplit, string prefix)
        {
            string paramsList = dSplit.Split("(")[1].Replace(")", "");

            if (string.IsNullOrWhiteSpace(prefix))
                paramsList = RemoveLeadingDash(paramsList);
            else
                paramsList = RemoveLeadingDash(paramsList.Replace(prefix, ""));

            return paramsList
                    .Split(PARAMETER_SEPARATOR)
                    .ToList();
        }

        private static List<Dictionary<string, string>> GetParamsObject(List<string> paramsList)
        {
            var parameters = new List<Dictionary<string, string>>();
            foreach (var item in paramsList)
            {
                if (item.Contains(":"))
                {
                    string[] split = item.Split(":", StringSplitOptions.RemoveEmptyEntries);
                    string param = split[0];
                    string val = split[1];
                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        parameters.Add(new Dictionary<string, string>() { { "key", param }, { "value", val } });
                    }
                }
            }

            if (parameters.Count > 0)
            {
                return parameters;
            }
            return new List<Dictionary<string, string>>();
        }

        private static UrlTransformation GetOperationDetailsFromOperation(string dSplit)
        {
            // Figure out module
            var fullFnName = dSplit.Split("(", StringSplitOptions.RemoveEmptyEntries)[0];

            string pluginId;
            string operationName;
            if (dSplit.StartsWith("p:"))
            {
                pluginId = fullFnName.Split(":", StringSplitOptions.RemoveEmptyEntries)[0];
                operationName = fullFnName.Split(":", StringSplitOptions.RemoveEmptyEntries)[1];
            }
            else
            {
                pluginId = fullFnName.Split(".")[0];
                operationName = fullFnName.Split(".")[1];
            }

            var values = new List<Dictionary<string, string>>();
            if (pluginId == "p")
            {
                if (dSplit.Contains("("))
                {
                    values = GetParamsObject(GetParamsList(dSplit, ""));
                }
            }
            else
            {
                values = GetParamsObject(GetParamsList(dSplit, ""));
            }

            var transformation = new UrlTransformation
            {
                plugin = pluginId,
                name = operationName
            };
            if (values.Count > 0)
            {
                transformation.values = values;
            }
            return transformation;
        }

        private static List<UrlTransformation> GetTransformationDetailsFromPattern(string pattern, string url, UrlConfig config, bool flatten = false)
        {
            if (pattern == "original")
                return new List<UrlTransformation>();

            var dSplit = pattern.Split(config.operationSeparator, StringSplitOptions.RemoveEmptyEntries);

            List<UrlTransformation> opts = dSplit.Select(x => GetOperationDetailsFromOperation(x)).ToList();

            return opts;
        }

        private static string? GetPatternFromTransformations(List<UrlTransformation>? transformationList, UrlConfig config)
        {
            if (transformationList != null)
            {
                if (transformationList.Count > 0)
                {
                    List<string> transformationListStr = transformationList.Select(o =>
                    {
                        if (o.name != null)
                        {
                            o.values = o.values != null ? o.values : new List<Dictionary<string, string>>();
                            var paramlist = new List<string>();
                            foreach (var items in o.values)
                            {
                                if (!items.ContainsKey("key") || string.IsNullOrWhiteSpace(items["key"]))
                                {
                                    throw new PDKIllegalArgumentError($"key not specified in '{o.name}'");
                                }
                                if (!items.ContainsKey("value") || string.IsNullOrWhiteSpace(items["value"]))
                                {
                                    throw new PDKIllegalArgumentError($"value not specified for key '{items["key"]}' in '{o.name}'");
                                }
                                paramlist.Add($"{items["key"]}:{items["value"]}");
                            }
                            var paramStr = string.Join(config.parameterSeparator, paramlist);

                            if (o.plugin == "p")
                            {
                                return paramStr != "" ? $"{o.plugin}:{o.name}({paramStr})" : $"{o.plugin}:{o.name}";
                            }
                            return $"{o.plugin}.{o.name}({paramStr})";
                        }
                        else
                        {
                            return null;
                        }
                    }).Where(ele => ele != null).Select(o => o!).ToList();

                    if (transformationListStr.Count > 0)
                        return string.Join(config.operationSeparator, transformationListStr);
                    else
                        return null;
                }
            }

            return null;
        }

        private static (string?, float?) ParseDPR(string dpr)
        {
            if (dpr == "auto") return (dpr, null);

            if (!float.TryParse(dpr, out float numeric_dpr) || numeric_dpr < 0.1f || numeric_dpr > 5.0f)
            {
                throw new PDKIllegalQueryParameterError("DPR value should be numeric and should be between 0.1 to 5.0");
            }

            return (null, numeric_dpr);
        }

        private static void ValidateFAuto(string f_auto)
        {
            try
            {
                bool boolean_f_auto = bool.Parse(f_auto);
            }
            catch (Exception)
            {
                throw new PDKIllegalQueryParameterError("F_auto value should be boolean");
            }
        }

        private static UrlObjOptions ProcessQueryParams(UrlObj urlParts)
        {
            UrlObjOptions queryObj = new UrlObjOptions();
            if (!string.IsNullOrWhiteSpace(urlParts.search))
            {
                string[] queryParams = urlParts.search.Substring(1).Split("&", StringSplitOptions.RemoveEmptyEntries);

                foreach (string param in queryParams)
                {
                    string[] queryElements = param.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (queryElements[0] == "dpr")
                    {
                        (string? strDpr, float? numDpr) = ParseDPR(queryElements[1]);
                        if (strDpr != null)
                            queryObj.dpr = strDpr;
                        else
                            queryObj.dpr = numDpr;
                    }
                    if (queryElements[0] == "f_auto")
                    {
                        ValidateFAuto(queryElements[1].ToLower());
                        queryObj.f_auto = bool.Parse(queryElements[1]);
                    }
                }
            }

            return queryObj;
        }
    }
}