using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MShop.Application.DTOs;
using MShop.Application.Interfaces;
using MShop.Application.Responses;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(SignInManager<SystemUser> signInManager, UserManager<SystemUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task<AuthResponse<IdentityResult>> RegisterSystemUser(SystemRegisterUserDTO userDTO)
        {
            var systemUser = new SystemUser
            {
                UserName = userDTO.Username,
                Email = userDTO.Email,
                Name = userDTO.Name,
                Phone = userDTO.Phone,
                Address = userDTO.Address
            };

            // Đăng ký người dùng với mật khẩu
            var result = await _userManager.CreateAsync(systemUser, userDTO.Password);

            // Nếu đăng ký thành công, thực hiện thêm quyền user
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(systemUser, "User");
            }

            // Trả về kết quả
            return new AuthResponse<IdentityResult>
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "User Registration Successful!" : "User Registration Failed!",
                Data = result
            };
        }
        public async Task<AuthResponse<LoginResponseDTO>> LoginSystemUser(SystemSignInUserDTO credentials)
        {
            // Tìm theo email
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return new AuthResponse<LoginResponseDTO>
                {
                    Success = false,
                    Message = "Email or password incorrect",
                    Data = new LoginResponseDTO()
                };
            }
            // Tìm thấy email thì tiếp tục kiểm tra đăng nhập
            var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, false, true);
            if (!result.Succeeded)
            {
                return new AuthResponse<LoginResponseDTO>
                {
                    Success = false,
                    Message = "Email or password is incorrect",
                    Data = new LoginResponseDTO()
                };
            }
            // Nếu đúng thông tin tài khoản thì thực hiện lấy ra Role của người dùng
            var roles = await _userManager.GetRolesAsync(user);

            // Tạo claim trả dữ liệu về
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Tạo JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                    );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse<LoginResponseDTO>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Token = tokenString
                }
            };
        }
    }
}
