using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Pixelbin.Common
{
    public static class Utils
    {
        public static string CreateQueryString(Dictionary<string, object>? parameters)
        {
            string queryString = "";
            if (parameters != null)
            {
                List<string> queryKeys = parameters.Keys.ToList();
                queryKeys.Sort();
                List<string> finalParams = new List<string>();
                foreach (string key in queryKeys)
                {
                    if (parameters[key] is IEnumerable<string> list)
                    {
                        list = list.OrderBy(s => s);
                        foreach(string item in list)
                        {
                            finalParams.Add(HttpUtility.UrlEncode(Convert.ToString(key)) + "=" + HttpUtility.UrlEncode(Convert.ToString(item)));
                        }
                    }
                    else
                    {
                        finalParams.Add(HttpUtility.UrlEncode(Convert.ToString(key)) + "=" + HttpUtility.UrlEncode(Convert.ToString(parameters[key])));
                    }
                }
                queryString = string.Join("&", finalParams);
            }
            return queryString;
        }

        public static object AddSignatureToHeaders<T>(string domain, string method, string url, string query_string, Dictionary<string, string> headers, T? body = null, List<string>? exclude_headers = null, bool sign_query = false) where T : class
        {
            query_string = HttpUtility.UrlDecode(query_string);
            var ebg_date = DateTime.Now.ToString("yyyyMMddTHHmmssZ");
            var headers_str = "";
            var host = domain.Replace("https://", "").Replace("http://", "");
            headers["host"] = host;
            if (!sign_query)
            {
                headers["x-ebg-param"] = ebg_date;
            }
            else
            {
                query_string += (!string.IsNullOrEmpty(query_string) ? "&" : "?") + "x-ebg-param=" + ebg_date;
            }
            var excluded_headers = new Dictionary<string, string>();
            if (exclude_headers != null)
            {
                foreach (var header in exclude_headers)
                {
                    if (headers.Remove(header, out var value))
                    {
                        excluded_headers[header] = value;
                    }
                }
            }
            foreach (var item in headers)
            {
                headers_str += $"{item.Key}:{item.Value}\n";
            }
            
            var body_hex = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(""))).Replace("-", "").ToLower();
            if (body != null)
            {
                var bodyJson = JsonConvert.SerializeObject(body, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.Default }).Replace(", ", ",").Replace(": ", ":");
                body_hex = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(bodyJson))).Replace("-", "").ToLower();
            }

            var request_list = new List<string> {
                method.ToUpper(),
                url,
                query_string,
                headers_str,
                string.Join(";", headers.Keys.Where(h => h == "host" || h.StartsWith("x-ebg-"))),
                body_hex
            };
            var request_str = string.Join("\n", request_list);
            request_str = string.Join("\n", ebg_date, BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(request_str))).Replace("-", "").ToLower());
            string signature = "v1:" + BitConverter.ToString(new HMACSHA256(Encoding.UTF8.GetBytes("1234567")).ComputeHash(Encoding.UTF8.GetBytes(request_str))).Replace("-", "").ToLower();
            if (!sign_query)
            {
                headers["x-ebg-signature"] = signature;
            }
            else
            {
                query_string += (!string.IsNullOrEmpty(query_string) ? "&" : "?") + "x-ebg-signature=" + signature;
            }
            foreach (var item in excluded_headers)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    headers[item.Key] = item.Value;
                }
            }
            return sign_query ? (object)query_string : (object)headers;
        }

        public static string ConvertImageToString(string path)
        {
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
    }
}