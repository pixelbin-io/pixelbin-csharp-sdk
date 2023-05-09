namespace Pixelbin.Platform
{
    /// <summary>
    /// OAuth Client.
    /// </summary>
    internal class OAuthClient
    {
        private readonly PixelbinConfig _conf;
        public string Token { get; set; }

        public OAuthClient(PixelbinConfig config)
        {
            _conf = config;
            Token = config.ApiSecret;
        }

        /// <summary>
        /// return access token
        /// </summary>
        public string GetAccessToken()
        {
            return Token;
        }
    }
}