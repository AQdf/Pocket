using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sho.Pocket.Auth.IdentityServer.Utils;
using Sho.Pocket.Core.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtIssuerOptions _jwtOptions;

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public JwtGenerator(RoleManager<IdentityRole<Guid>> roleManager, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedTokenAsync(Guid userId, string email, IList<string> roles)
        {
            var claims = await GenerateClaims(userId, email, roles);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<IEnumerable<Claim>> GenerateClaims(Guid id, string userName, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
{
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 new Claim(JwtClaimIdentifiers.Id, id.ToString())
            };

            IEnumerable<Claim> roleClaims = roles.Select(role => new Claim(JwtClaimIdentifiers.Role, role));
            claims.AddRange(roleClaims);

            return claims;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
