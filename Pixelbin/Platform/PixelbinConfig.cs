using System.Collections.Generic;
using Pixelbin.Common.Exceptions;
using static Pixelbin.Common.Constants;

namespace Pixelbin.Platform
{

    /// <summary>
    /// PixelbinConfig hold the configuration detail
    /// </summary>
    public class PixelbinConfig
    {
        public string Domain { get; }
        public string ApiSecret { get; }
        internal OAuthClient OauthClient { get; }

        /// <summary>
        /// create instance of PixelbinConfig
        /// </summary>
        /// <param name="config">configuration details in key value terms</param>
        public PixelbinConfig(Dictionary<string, string> config)
        {
            Domain = config.ContainsKey("domain") ? config["domain"] : DEFAULT_DOMAIN;
            ApiSecret = config.ContainsKey("apiSecret") ? config["apiSecret"] : "";
            OauthClient = new OAuthClient(this);
            Validate();
        }

        /// <summary>
        /// return the access token
        /// </summary>
        public string GetAccessToken()
        {
            string token = OauthClient.GetAccessToken();
            return token;
        }

        /// <summary>
        /// Validates apiSecret.
        /// </summary>
        /// <exception cref="PDKInvalidCredentialError"></exception>
        private void Validate()
        {
            if (string.IsNullOrEmpty(ApiSecret))
            {
                throw new PDKInvalidCredentialError("No API Secret Token Present");
            }

            if (ApiSecret.Length < APPLICATION_MIN_TOKEN_LENGTH)
            {
                throw new PDKInvalidCredentialError("Invalid API Secret Token");
            }
        }
    }
}