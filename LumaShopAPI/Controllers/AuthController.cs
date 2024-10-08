/*
 * <summery>
 * This controller handles user authentication operations such as registration
 * and login. It interacts with UserManager and RoleManager to manage user data 
 * and generates JWT tokens for authenticated users.
 * </summery>
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LumaShopAPI.DTOModals.User;
using System.Net;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRoles> _roleManager;
        private readonly IConfiguration _configuration;

        /*
         * Constructor for AuthController that initializes the user manager, role manager, and configuration settings.
         * Accepts instances of UserManager, RoleManager, and IConfiguration as parameters to facilitate user and role management.
         */
        public AuthController(UserManager<User> userManager, RoleManager<UserRoles> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }



        [HttpPost]
        [Route("register")]
        /*
         * Handles user registration by accepting a RegisterRequest,
         * invoking the registration logic, and returning an HTTP response based on the outcome.
        */
        public async Task<IActionResult> Register([FromBody] DTOModals.User.RegisterRequest request)
        {
            var result = await RegisterAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);

        }

        /*
         * This method registers a new user by checking if the user already exists,
         * creating a new user, and assigning the user to a role.
         * It returns a RegisterResponse object based on the outcome.
        */
        private async Task<RegisterResponse> RegisterAsync(DTOModals.User.RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null) return new RegisterResponse { Message = "User already exists", Success = false };

                //if we get here, no user with this email..
                if (request.Role == UserRoleEnum.VENDOR)
                {
                    userExists = new User
                    {
                        CompanyName = request.CompanyName,
                        Description = request.Description,
                        Role = request.Role,
                        Email = request.Email,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        UserName = request.Email,

                    };
                }
                else
                {
                    userExists = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Role = request.Role,
                        Email = request.Email,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        UserName = request.Email,

                    };
                }
                string roleName = string.Empty;

                switch (request.Role)
                {
                    case UserRoleEnum.VENDOR:
                        roleName = "VENDOR";
                        break;
                    case UserRoleEnum.ADMIN:
                        roleName = "ADMIN";
                        break;
                    case UserRoleEnum.CUSTOMER:
                        roleName = "CUSTOMER";
                        break;
                    case UserRoleEnum.CSR:
                        roleName = "CSR";
                        break;
                    default:
                        roleName = "CUSTOMER";
                        break;
                }
                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded) return new RegisterResponse { Message = $"Create user failed {createUserResult?.Errors?.First()?.Description}", Success = false };
                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, roleName);
                if (!addUserToRoleResult.Succeeded) return new RegisterResponse { Message = $"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}", Success = false };

                return new RegisterResponse
                {
                    Success = true,
                    Message = "User registered successfully"
                };



            }
            catch (Exception ex)
            {
                return new RegisterResponse { Message = ex.Message, Success = false };
            }
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]

        /*
         * Handles user login by accepting a LoginRequest,
         * invoking the login logic, and returning an HTTP response based on the outcome.
        */
        public async Task<IActionResult> Login([FromBody] DTOModals.User.LoginRequest request)
        {
            var result = await LoginAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);


        }


        /*
         * This method logs in a user by checking if the user exists,
         * generating a JWT token, and returning a LoginResponse object based on the outcome.
        */
        private async Task<LoginResponse> LoginAsync(DTOModals.User.LoginRequest request)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null) return new LoginResponse { Message = "Invalid email/password", Success = false };
                if (user.Status == UserStatusEnum.INACTIVE) return new LoginResponse { Message = "Your account is inactive. Please contact support for assistance.", Success = false };
                if (user.Status == UserStatusEnum.PENDING) return new LoginResponse { Message = "Your account is pending activation. Please check your email for further instructions.", Success = false };

                //all is well if ew reach here
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSigningKey"] ?? throw new ArgumentNullException("JWTSigningKey", "JWT signing key is not configured.")));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(1440);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds

                    );

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Successful",
                    Email = user?.Email,
                    Success = true,
                    UserId = user?.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginResponse { Success = false, Message = ex.Message };
            }


        }
    }
}
