namespace Isitar.DoenerOrder.Api.Helpers.Auth
{
    /// <summary>
    /// Custom claim types are stored here.
    /// Very similar to  <see cref="System.Security.Claims.ClaimTypes"/>
    /// </summary>
    public static class CustomClaimTypes
    {
        private const string ClaimTypeNamespace = "http://isitar.ch";

        public const string Permission = ClaimTypeNamespace + "/permission";
    }
}