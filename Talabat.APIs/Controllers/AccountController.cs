using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talbat.Core.Entities.Identity;
using Talbat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthServices _authServices;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , IAuthServices authServices, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
            this.mapper = mapper;
        }
        [HttpPost("login")]//Post api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));
            var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!Result.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _authServices.CreateTokenAsunc(user, _userManager)
            });
        }
        [HttpPost("register")]//Post api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new[] { "Email is already in use" }
                });

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber
            };
            var Result = await _userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsunc(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet] // Get api/account
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsunc(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet("address")]// Get api/account/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddress(User);

            var address = mapper.Map<AddressDto>(user.Address);
            return Ok(address);
        }
        [Authorize]
        [HttpPut("address")] // Put api/account/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
        {
            var address = mapper.Map<AddressDto, Address>(UpdatedAddress);


            var user = await _userManager.FindUserWithAddress(User);

            user.Address.FName = UpdatedAddress.FirstName;
            user.Address.LName = UpdatedAddress.LastName;
            user.Address.Street = UpdatedAddress.Street;
            user.Address.City = UpdatedAddress.City;
            user.Address.Country = UpdatedAddress.Country;


            var res = await _userManager.UpdateAsync(user);

            if (!res.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(UpdatedAddress);
        }
        [HttpGet("emailExist")] // Get: /api/account/emailExist
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
