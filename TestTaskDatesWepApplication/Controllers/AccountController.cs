using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestTaskDatesCommon.Models;
using TestTaskDatesCommon.Payloads;
using TestTaskDatesWepApplication.Database;

namespace TestTaskDatesWepApplication.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDBContext context;

        public AccountController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [Route("api/[controller]/register")]
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            var foundedUser = context.Users.ToList().FirstOrDefault(x => String.Equals(x.Login, user.Login, StringComparison.InvariantCultureIgnoreCase));
            if (foundedUser != null)
            {
                return BadRequest(new GeneralResponsePayload() { isSuccess = false, errorText = "User with such login already exist." });
            }

            context.Users.Add(user);
            context.SaveChanges();

            return Json(new GeneralResponsePayload { isSuccess = true });
        }

        [Route("api/[controller]/token")]
        [HttpPost]
        public IActionResult CreateToken(User user)
        {
            var identity = GetIdentity(user.Login, user.Password);
            if (identity == null)
            {
                return BadRequest(new LoginResultPayload() { errorText = "Invalid username or password." });
            }

            TimeSpan tokenExpiresIn = new TimeSpan(7, 0, 0, 0);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: DateTime.Now.Add(tokenExpiresIn),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new LoginResultPayload()
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = context.Users.ToList().FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}
