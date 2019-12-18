namespace Numaka.Common.TokenValidator
{
    /// <summary>
    /// Token validator interface
    /// </summary>
    public interface ITokenValidator
    {
        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        TokenValidationResult ValidateToken(string token);
    }
}
