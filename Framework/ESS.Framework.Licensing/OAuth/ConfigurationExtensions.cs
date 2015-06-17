using System;
using System.Web.Http;
using ESS.Framework.Common.Configurations;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ESS.Framework.Licensing.OAuth
{
    public static class ConfigurationExtensions
    {
        public static Configuration UseOAuth(this Configuration configuration, IAppBuilder app,HttpConfiguration config)
        {
            app.UseCors(CorsOptions.AllowAll);

            //全局Authorize
            config.Filters.Add(new AuthorizeAttribute());
            ConfigureOAuth(app);

            return configuration;
        }

        public static void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("ESS")
            };
            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            var issuer = "ESS";
            var audience = "ESS";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                });
        }
    }
}
