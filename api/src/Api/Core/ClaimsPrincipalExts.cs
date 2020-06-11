using System;
using System.Linq;
using System.Security.Claims;

/// <summary>
/// Extension methods for ClaimsPrincipal.
/// </summary>
public static class ClaimsPrincipalExts {
    /// <summary>
    /// Get the user Id from the NameIdentifier claim on the user principal.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <returns>The unique Id of the user the principal is for.</returns>
    public static string GetUserId(this ClaimsPrincipal principal) {
        if (principal.Claims.Count() == 0) {
            throw new Exception("No claims exist on principal");
        }

        Claim subjectClaim = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        return subjectClaim.Value;
    }
}