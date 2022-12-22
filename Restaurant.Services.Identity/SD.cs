using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Restaurant.Services.Identity
{
    public class SD
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(name: "palyanuchka",   displayName: "Restaurant server"),
                new ApiScope(name: "read",   displayName: "Read your data."),
                new ApiScope(name: "write",  displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data.")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile" }
                },
                new Client
                {
                    ClientId = "palyanuchka",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "palyanuchka", 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                        JwtClaimTypes.Role
                    },
                    RedirectUris = { "https://localhost:44350/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44350/signout-callback-oidc" },
                }
            };
    }
}
