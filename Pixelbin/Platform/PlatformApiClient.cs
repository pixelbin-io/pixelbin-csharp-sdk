using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pixelbin.Common;
using static Pixelbin.Common.Utils;

namespace Pixelbin.Platform
{
    /// <summary>
    /// APIClient help in executing client api calls
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// execute api calls
        /// </summary>
        /// <remarks>
        /// create request params with header signature and execute api call
        /// </remarks>
        /// <param name="conf">configuration details</param>
        /// <param name="method">method name</param>
        /// <param name="url">url to hit</param>
        /// <param name="query">query parameters of url</param>
        /// <param name="body">body content for url</param>
        /// <param name="contentType">content type for request header</param>
        /// <returns></returns>
        public static async Task<Dictionary<string,object>> Execute<T>(PixelbinConfig conf, string method, string url, Dictionary<string, object>? query, T? body, string? contentType) where T : class, new()
        {
            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(conf.GetAccessToken()));
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {token}" }
            };
            if (!string.IsNullOrEmpty(contentType) && body != null)
            {
                headers["Content-Type"] = contentType;
            }

            T? data = body;
            if (contentType == "multipart/form-data")
            {
                data = default;
            }

            if (query != null && method.ToUpper() == "GET")
            {
                Dictionary<string, object> get_params = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> kvp in query)
                {
                    if (kvp.Value is bool b)
                    {
                        get_params[kvp.Key] = b.ToString().ToLower();
                    }
                    else
                    {
                        get_params[kvp.Key] = kvp.Value;
                    }
                }
                query = get_params;
            }

            string query_string = CreateQueryString(query);

            Dictionary<string, string> headers_with_sign = (Dictionary<string, string>)AddSignatureToHeaders(
                domain: conf.Domain,
                method: method,
                url: url,
                query_string: query_string,
                headers: headers,
                body: data,
                exclude_headers: new List<string>() { "Authorization", "Content-Type" }
            );

            headers_with_sign["x-ebg-param"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(headers_with_sign["x-ebg-param"]));
            return await HttpHelper.Request(method: method, url: $"{conf.Domain}{url}", queryParams: query, data: body, headers: headers_with_sign);
        }
    }
}