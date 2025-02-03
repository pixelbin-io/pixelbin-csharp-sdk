namespace Pixelbin.Utils
{
    /// <summary>
    /// url utilities to construct and deconstruct Pixelbin urls.
    /// </summary>
    public class Url
    {
        public static UrlConfig config = new UrlConfig(operationSeparator: "~", parameterSeparator: ",");

        /// <summary>
        /// Deconstruct a pixelbin url
        /// </summary>
        /// <param name="url">A valid pixelbin url</param>
        /// <returns>UrlObj containing deconstruction of pixelbin url</returns>
        public static UrlObj UrlToObj(string url, UrlConfig? opts = null)
        {
            return Utils.GetObjFromUrl(url, new UrlConfig(
                operationSeparator: config.operationSeparator,
                parameterSeparator: config.parameterSeparator,
                isCustomDomain: opts?.isCustomDomain ?? false
            ), false);
        }

        /// <summary>
        /// Converts the extracted url obj to a Pixelbin url.
        /// </summary>
        /// <param name="obj">object containing parameters to build a Pixelbin url</param>
        /// <returns>a valid pixelbin url</returns>
        public static string ObjToUrl(UrlObj obj)
        {
            return Utils.GetUrlFromObj(obj, config);
        }
    }
}