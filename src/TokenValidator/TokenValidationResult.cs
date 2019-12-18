using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace Numaka.Common.TokenValidator
{
    /// <summary>
    /// Token validation result class
    /// </summary>
    public class TokenValidationResult
    {
        /// <summary>
        /// Token validation result
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal</param>
        /// <param name="securityToken">The security token</param>
        public TokenValidationResult(ClaimsPrincipal claimsPrincipal, SecurityToken securityToken)
        {
            ClaimsPrincipal = claimsPrincipal ?? throw new ArgumentNullException(nameof(claimsPrincipal));
            SecurityToken = securityToken ?? throw new ArgumentNullException(nameof(securityToken));
        }

        /// <summary>
        /// The claims principal
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// The security token
        /// </summary>
        public SecurityToken SecurityToken { get; }
    }
}