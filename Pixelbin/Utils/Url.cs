using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Pixelbin.Common.Exceptions;

namespace Pixelbin.Utils
{
    /// <summary>
    /// url utilities to construct and deconstruct Pixelbin urls.
    /// </summary>
    public class Url
    {
        private const string OPERATION_SEPARATOR = "~";
        private const string PARAMETER_SEPARATOR = ",";
        private const string VERSION2_REGEX = @"^v[1-2]$";
        private const string URL_WITH_ZONE = @"([a-zA-Z0-9_-]*)\/([a-zA-Z0-9_-]{6})\/(.+)\/(.*)";
        private const string URL_WITHOUT_ZONE = @"([a-zA-Z0-9_-]*)\/(.+)\/(.*)";
        private const string ZONE_SLUG = @"([a-zA-Z0-9_-]{6})";
        private const string BASE_URL = "https://cdn.pixelbin.io";
        private static readonly string[] ALLOWED_OPTIONAL_PARAMS = { "dpr", "f_auto" };

        /// <summary>
        /// Deconstruct a pixelbin url
        /// </summary>
        /// <param name="url">A valid pixelbin url</param>
        /// <returns>UrlObj containing deconstruction of pixelbin url</returns>
        public static UrlObj UrlToObj(string url)
        {
            return GetObjFromUrl(url);
        }

        /// <summary>
        /// Converts the extracted url obj to a Pixelbin url.
        /// </summary>
        /// <param name="obj">object containing parameters to build a Pixelbin url</param>
        /// <returns>a valid pixelbin url</returns>
        public static string ObjToUrl(UrlObj obj)
        {
            return GetUrlFromObj(obj);
        }

        private static UrlObj GetUrlParts(string url, out string? pattern)
        {
            var urlDetails = new UrlObj();
            var parseUrl = new Uri(url);
            urlDetails.baseUrl = $"{parseUrl.Scheme}://{parseUrl.Host}";
            urlDetails.options = ProcessQueryParams(parseUrl.Query.Replace("?", ""));
            urlDetails.version = "v1";

            var parts = parseUrl.AbsolutePath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            if (Regex.IsMatch(parts[0], VERSION2_REGEX))
            {
                urlDetails.version = parts[0];
                parts = parts[1..];
            }

            if (parts[0].Length < 3)
            {
                throw new PixelbinInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");
            }

            if (Regex.IsMatch(string.Join("/", parts), URL_WITH_ZONE))
            {
                urlDetails.cloudName = parts[0];
                parts = parts[1..];
                urlDetails.zone = parts[0];
                parts = parts[1..];
                pattern = parts[0];
                parts = parts[1..];
                urlDetails.filePath = string.Join("/", parts);
            }
            else if (Regex.IsMatch(string.Join("/", parts), URL_WITHOUT_ZONE))
            {
                urlDetails.cloudName = parts[0];
                parts = parts[1..];
                pattern = parts[0];
                parts = parts[1..];
                urlDetails.filePath = string.Join("/", parts);
            }
            else
            {
                throw new PixelbinInvalidUrlError("Invalid pixelbin url. Please make sure the url is correct.");
            }
            return urlDetails;
        }

        private string RemoveLeadingDash(string str)
        {
            if (str.Length > 0 && str[0] == '-')
            {
                return str.Substring(1);
            }
            return str;
        }

        private static List<string> GetParamsList(string operation)
        {
            var dSplit = operation.Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
            var paramsList = dSplit[1].Replace(")", "").Split(new string[] { PARAMETER_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            return new List<string>(paramsList);
        }

        private static List<Dictionary<string, string>> GetParamsObject(List<string> paramsList)
        {
            var parameters = new List<Dictionary<string, string>>();
            foreach (var item in paramsList)
            {
                if (item.Contains(":"))
                {
                    var param = item.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var val = item.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (param != null)
                    {
                        parameters.Add(new Dictionary<string, string>() { { "key", param }, { "value", val} });
                    }
                }
            }

            if (parameters.Count > 0)
            {
                return parameters;
            }
            return new List<Dictionary<string, string>>();
        }

        private static UrlTransformation GetOperationDetailsFromOperation(string operation)
        {
            var dSplit = operation.Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
            var fullFnName = dSplit[0];

            var pluginId = "";
            var operationName = "";
            if (fullFnName.StartsWith("p:"))
            {
                var parts = fullFnName.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                pluginId = parts[0];
                operationName = parts[1];
            }
            else
            {
                var parts = fullFnName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                pluginId = parts[0];
                operationName = parts[1];
            }

            var values = new List<Dictionary<string, string>>();
            if (pluginId == "p")
            {
                if (dSplit.Length > 1)
                {
                    values = GetParamsObject(GetParamsList(operation));
                }
            }
            else
            {
                values = GetParamsObject(GetParamsList(operation));
            }

            var transformation = new UrlTransformation();
            transformation.plugin = pluginId;
            transformation.name = operationName;
            if (values.Count > 0)
            {
                transformation.values = values;
            }
            return transformation;
        }

        private static List<UrlTransformation> GetTransformationDetailsFromPattern(string pattern, string url)
        {
            if (pattern == "original")
            {
                return new List<UrlTransformation>();
            }

            var dSplit = pattern.Split(new[] { OPERATION_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            List<UrlTransformation> opts = dSplit.Select(x => GetOperationDetailsFromOperation(x)).ToList();

            return opts;
        }

        private static UrlObj GetObjFromUrl(string url, bool flatten = false)
        {
            string? pattern;
            var parts = GetUrlParts(url, out pattern);
            try
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    parts.transformations = GetTransformationDetailsFromPattern(
                        (string)pattern,
                        url
                    );
                    parts.pattern = (string)pattern;
                }
                else
                {
                    parts.transformations = new List<UrlTransformation>();
                }
            }
            catch (Exception e)
            {
                throw new PixelbinInvalidUrlError($"Error Processing url. Please check the url is correct, {e}");
            }
            return parts;
        }

        private static string GetUrlFromObj(UrlObj obj)
        {
            if (string.IsNullOrEmpty(obj.baseUrl))
            {
                obj.baseUrl = BASE_URL;
            }

            if (string.IsNullOrEmpty(obj.cloudName))
            {
                throw new PixelbinIllegalArgumentError("cloudName should be defined");
            }
            if (string.IsNullOrEmpty(obj.filePath))
            {
                throw new PixelbinIllegalArgumentError("filePath should be defined");
            }

            string? pattern_value = GetPatternFromTransformations(obj.transformations);
            obj.pattern = pattern_value != null ? pattern_value : "original";
            if (string.IsNullOrEmpty(obj.version) || !Regex.IsMatch(obj.version, VERSION2_REGEX))
            {
                obj.version = "v2";
            }
            if (string.IsNullOrEmpty(obj.zone) || !Regex.IsMatch(obj.zone, ZONE_SLUG))
            {
                obj.zone = "";
            }

            string[] urlKeySorted = new string[] { "baseUrl", "version", "cloudName", "zone", "pattern", "filePath" };
            List<string> urlArr = new List<string>();
            foreach (string key in urlKeySorted)
            {
                if (obj.GetType().GetProperty(key).GetValue(obj) != null)
                {
                    urlArr.Add((string)obj.GetType().GetProperty(key).GetValue(obj));
                }
            }

            List<string> queryArr = new List<string>();
            if (obj.options != null)
            {
                UrlObjOptions options = obj.options;
                string? dpr = Convert.ToString(options.dpr) == string.Empty ? null : Convert.ToString(options.dpr);
                string? f_auto = Convert.ToString(options.f_auto) == string.Empty ? null : Convert.ToString(options.f_auto);
                if (dpr != null)
                {
                    ValidateDPR(dpr);
                    queryArr.Add("dpr=" + float.Parse(dpr).ToString(".0#"));
                }
                if (f_auto != null)
                {
                    ValidateFAuto(f_auto);
                    queryArr.Add("f_auto=" + f_auto);
                }
            }
            urlArr.RemoveAll(x => x == "");
            string urlStr = string.Join("/", urlArr);
            if (queryArr.Count > 0)
            {
                urlStr += "?" + string.Join("&", queryArr);
            }
            return urlStr;
        }

        private static string? GetPatternFromTransformations(List<UrlTransformation>? transformationList)
        {
            if (transformationList != null)
            {
                if (transformationList.Count == 0)
                {
                    return null;
                }

                var transformationListStr = transformationList.Select(o =>
                {
                    if (o.name != null)
                    {
                        o.values = o.values != null ? o.values : new List<Dictionary<string, string>>();
                        var paramlist = new List<string>();
                        foreach (var items in o.values)
                        {
                            if (!items.ContainsKey("key") || items["key"] == null)
                            {
                                throw new PixelbinIllegalArgumentError($"key not specified in '{o.name}'");
                            }
                            if (!items.ContainsKey("value") || items["value"] == null)
                            {
                                throw new PixelbinIllegalArgumentError($"value not specified for '{items["key"]}' in '{o.name}'");
                            }
                            paramlist.Add($"{items["key"]}:{items["value"]}");
                        }
                        var paramstr = string.Join(PARAMETER_SEPARATOR, paramlist);

                        if (o.plugin == "p")
                        {
                            return paramstr != "" ? $"p:{o.name}({paramstr})" : $"p:{o.name}";
                        }
                        return $"{o.plugin}.{o.name}({paramstr})";
                    }
                    return null;
                }).Where(ele => ele != null).ToList();

                return string.Join(OPERATION_SEPARATOR, transformationListStr);
            }
            else
            {
                return null;
            }
        }

        private static void ValidateDPR(string dpr)
        {
            try
            {
                float numeric_dpr = float.Parse(dpr);
                if (numeric_dpr < 0.1 || numeric_dpr > 5.0)
                {
                    throw new PixelbinIllegalQueryParameterError("DPR value should be between 0.1 to 5.0");
                }
            }
            catch (Exception)
            {
                throw new PixelbinIllegalQueryParameterError("DPR value should be numeric");
            }
        }

        private static void ValidateFAuto(string f_auto)
        {
            try
            {
                bool boolean_f_auto = bool.Parse(f_auto);
                //if (f_auto.GetType() != typeof(bool))
                //{
                //    throw new PixelbinIllegalQueryParameterError("F_auto value should be boolean");
                //}
            }
            catch (Exception)
            {
                throw new PixelbinIllegalQueryParameterError("F_auto value should be boolean");
            }
        }

        private static UrlObjOptions? ProcessQueryParams(string? queryString)
        {
            if (queryString != null)
            {
                var queryParams = (queryString as string).Split("&");
                var queryObj = new UrlObjOptions();
                foreach (var param in queryParams)
                {
                    var queryElements = param.Split("=");
                    if (ALLOWED_OPTIONAL_PARAMS.Contains(queryElements[0]))
                    {
                        if (queryElements[0] == "dpr")
                        {
                            queryObj.dpr = float.Parse(queryElements[1]);
                            ValidateDPR(Convert.ToString(queryObj.dpr));
                        }
                        else
                        {
                            queryObj.f_auto = bool.Parse(queryElements[1]);
                            ValidateFAuto(Convert.ToString(queryObj.f_auto));
                        }
                    }
                }
                return queryObj;
            }
            return null;
        }
    }

    /// <summary>
    /// object used in constructing and deconstructing Pixelbin urls
    /// </summary>
    public class UrlObj
    {
        /// <summary>
        /// cdn api version
        /// </summary>
        public string? version { get; set; }
        /// <summary>
        /// Base url
        /// </summary>
        public string? baseUrl { get; set; }
        /// <summary>
        /// Path to the file on Pixelbin storage
        /// </summary>
        public string? filePath { get; set; }
        public string? pattern { get; set; }
        /// <summary>
        /// The cloudname extracted from the url
        /// </summary>
        public string? cloudName { get; set; }
        /// <summary>
        /// optional query parameters
        /// </summary>
        public UrlObjOptions? options { get; set; }
        /// <summary>
        /// 6 character zone slug
        /// </summary>
        public string? zone { get; set; }
        /// <summary>
        /// Extracted transformations from the url
        /// </summary>
        public List<UrlTransformation>? transformations { get; set; }

        public UrlObj(
            string? version = null,
            string? baseUrl = null,
            string? filePath = null,
            string? pattern = null,
            string? cloudName = null,
            UrlObjOptions? options = null,
            string? zone = null,
            List<UrlTransformation>? transformations = null
        )
        {
            this.version = version;
            this.baseUrl = baseUrl;
            this.filePath = filePath;
            this.pattern = pattern;
            this.cloudName = cloudName;
            this.options = options;
            this.zone = zone;
            this.transformations = transformations;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UrlObjOptions
    {
        public object? dpr { get; set; }
        public object? f_auto { get; set; }

        public UrlObjOptions(object? dpr = null, object? f_auto = null)
        {
            this.dpr = dpr;
            this.f_auto = f_auto;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UrlTransformation
    {
        public List<Dictionary<string,string>>? values { get; set; }
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
}