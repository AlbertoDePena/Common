using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Numaka.Common;

namespace Numaka.TokenValidator
{
    /// <summary>
    /// Token validator
    /// </summary>
    public class TokenValidator : ITokenValidator
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        /// <summary>
        /// Validate authenticated user/application using open ID connection configuration
        /// </summary>
        /// <param name="authority">The authority</param>
        /// <param name="clientId">The client ID</param>
        public TokenValidator(string authority, string clientId)
        {
            if (string.IsNullOrWhiteSpace(authority)) throw new ArgumentNullException(nameof(authority));
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));

            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(authority + "/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            var configuration = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));

            _tokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidAudience = clientId,
                    ValidIssuer = configuration.Issuer,
                    IssuerSigningKeys = configuration.SigningKeys,
                    ValidateIssuerSigningKey = true,
                };
        }

        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenValidationResult ValidateToken(string token)
        {
            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, _tokenValidationParameters, out SecurityToken securityToken);

            return new TokenValidationResult(claimsPrincipal, securityToken);
        }
    }
}
