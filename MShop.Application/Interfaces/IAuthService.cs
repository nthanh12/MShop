using Microsoft.AspNetCore.Identity;
using MShop.Application.DTOs;
using MShop.Application.Responses;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse<IdentityResult>> RegisterSystemUser(SystemRegisterUserDTO userDTO);
        Task<AuthResponse<LoginResponseDTO>> LoginSystemUser(SystemSignInUserDTO credentials);
    }
}
