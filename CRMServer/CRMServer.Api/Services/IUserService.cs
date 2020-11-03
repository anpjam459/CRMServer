using CRMServer.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRMServer.Api.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

        //Task<UserManagerResponse> ForgetPasswordAsync(string email);
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuation;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuation = configuration;
        }

        //public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if(user == null)
        //    {
        //        return new UserManagerResponse {
        //            IsSusscess = false,
        //            Message = "No user associated with email"
        //        };
        //    }

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodedToken = Encoding.UTF8.GetBytes(token);
        //    var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
             
        //}

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that username",
                    IsSusscess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSusscess = false
                };
            }

            var claims = new[] {
                new Claim("UserName", model.UserName),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuation["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuation["Jwt:Issuer"],
                audience: _configuation["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenString,
                IsSusscess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register model is null");


            if(model.Password != model.ConfirmPassword)
                return new UserManagerResponse()
                {
                    Message = "Password confirm doesn't math the password",
                    IsSusscess = false, 
                };

            var identityUser = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {
                return new UserManagerResponse()
                {
                    Message = "User created successfully",
                    IsSusscess = true,
                };
            }
            else
            {
                return new UserManagerResponse()
                {
                    Message = "User did not create",
                    IsSusscess = false,
                    Errors = result.Errors.Select(p => p.Description)
                };
            } 
        }
    }
}
