﻿using DTOLayer.DTOs.AuthDTOs;
using DTOLayer.DTOs.GeneralIdentityResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAuthService
    {
        Task<GeneralIdentityDto> SeedRolesAsync();
        Task<GeneralIdentityDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
        Task<GeneralIdentityDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
        Task<GeneralIdentityDto> UpdateUserAsync(ClaimsPrincipal User, UpdateUserDto updateUserDto);
        Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
        Task<IEnumerable<UserInfoResult>> GetUsersListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
        Task<GeneralIdentityDto> DeleteUser(UserInfoResult user);

        Task<UserInfoResult?> GetUserDetailsByUserId(string id);

        Task<IEnumerable<string>> GetUsernamesListAsync();
    }
}
