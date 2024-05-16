﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.CoreLayer.Entities.Idintity;

namespace Talabat.APIs.Controllers
{
 
    public class AccountController : APIBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("login")] // POST : /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var restult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!restult.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "This will be Token"
            });
        }
    }
}
