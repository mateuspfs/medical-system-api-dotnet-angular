using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace SistemaMedico.Utilies
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class JwtProtectRouteAttribute(string role) : Attribute, IAuthorizationFilter
    {
        private readonly string _role = role;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var jwtSecret = configuration["Jwt:Secret"] ?? string.Empty;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtSecret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var find = jwtToken.Claims.First(x => x.Type == "role").Value;

                if (!find.Equals(_role, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Result = new ForbidResult();
                    return; 
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }   
        }
    }
}
