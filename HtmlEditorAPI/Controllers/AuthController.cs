using CentennialAPI.Classes.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CentennialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AuthController(IConfiguration config, IUserService userService, AppDbContext context)
        {
           _config = config;
           _userService = userService;
            _context = context;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpGet("{id:int}")]
        public ActionResult<UserPublicModel> GetUserById(int id) {
            try
            {
                var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
                if(user == null)
                {
                    return BadRequest("Usuário não encontrado");
                }
                UserPublicModel userModel = new UserPublicModel();
                userModel.id = id;
                userModel.Name = user.Nome;
                userModel.Email = user.Email;

                return Ok(userModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrarAdmin"), Authorize(Roles = "Admin")]
        public ActionResult<User> RegisterAdmin(UserDto request)
        {
            CreatePassHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = request.Email;
            user.Nome = request.Nome;
            user.Role = "Admin";

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("registrarUser")]
        public ActionResult<User> RegisterUser(UserDto request)
        {
            CreatePassHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = request.Email;
            user.Nome = request.Nome;
            user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            string AdminToken;
            string UserToken;
            string Message = "Usuário ou senha inválida";
            try
            {
                var User = _context.Users.Where(t => t.Username == request.Username).FirstOrDefault();
                if (User == null)
                {
                    return BadRequest(Message);
                }
                if(VerifyPassHash(request.Password, User.PasswordHash, User.PasswordSalt))
                {
                    if (User.Role == "Admin")
                    {
                        AdminToken = CreateAdminToken(User);
                        return Ok(AdminToken);
                    }
                    else
                    {
                        UserToken = CreateUserToken(User);
                        return Ok(UserToken);
                    }
                } else return BadRequest(Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string CreateAdminToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string CreateUserToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePassHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt)) {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
