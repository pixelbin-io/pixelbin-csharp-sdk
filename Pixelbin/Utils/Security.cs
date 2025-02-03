using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Pixelbin.Common.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Pixelbin.Security
{
    public static class Security
    {
        public static string SignURL(string url, long expirySeconds, string accessKey, string token)
        {
            return GenerateSignedURL(url, expirySeconds, accessKey, token);
        }

        private static string GenerateHMACSHA256(string message, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private static string GenerateSignature(string urlPath, long expiryTimestamp, string key)
        {
            if (urlPath.StartsWith("/"))
                urlPath = urlPath.Substring(1);

            string hash = GenerateHMACSHA256(urlPath + expiryTimestamp, key);
            return hash;
        }

        public static string GenerateSignedURL(string url, long expirySeconds, string accessKey, string token)
        {
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(token))
                throw new PDKIllegalArgumentError("Valid url, accessKey & expirySeconds are required for generating signed URL");

            Uri urlObj = new Uri(url);
            string urlPath = urlObj.PathAndQuery;
            NameValueCollection searchParams = HttpUtility.ParseQueryString(urlObj.Query);

            if (searchParams.Get("pbs") != null || searchParams.AllKeys.Contains("pbs"))
                throw new PDKIllegalArgumentError("URL already has a signature");

            long expiryTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + expirySeconds;

            string signature = GenerateSignature(urlPath, expiryTimestamp, token);

            UriBuilder signedUrlObj = new UriBuilder(urlObj);
            if (signedUrlObj.Query.Length > 0)
                signedUrlObj.Query += "&";
            signedUrlObj.Query += "pbs=" + signature;
            signedUrlObj.Query += "&pbe=" + expiryTimestamp;
            signedUrlObj.Query += "&pbt=" + accessKey;

            return signedUrlObj.ToString();
        }
    }
}