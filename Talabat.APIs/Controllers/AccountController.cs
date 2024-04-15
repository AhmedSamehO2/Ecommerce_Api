using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOS;
using Talabat.APIs.Errors;
using Talabat.APIs.Extension;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,ITokenServices tokenServices,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if (CheckEmailExsist(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email Is Already Exsisted"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
              //  PhoneNumber = model.PhoneNumber,
            };
          var result =  await _userManager.CreateAsync(User,model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User,_userManager)
            };
            return Ok(ReturnedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if(User is null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password,false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName= User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User, _userManager)
            });

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<AppUser>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var userIn = await _userManager.FindByEmailAsync(Email);
            //var ReturnedToDto = new UserDto()
            //{
            //    DisplayName = userIn.DisplayName,
            //    Email = userIn.Email,
            //    Token = await _tokenServices.CreateTokenAsync(userIn, _userManager)
            //};
            return Ok(new UserDto()
            {
                DisplayName = userIn.DisplayName,
                Email = userIn.Email,
                Token = await _tokenServices.CreateTokenAsync(userIn, _userManager)
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            
            var result = _mapper.Map<AddressDto>(user.Address);
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdaeteAdaress(AddressDto updateaddress)
        {  
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var MappedAddress = _mapper.Map<AddressDto,Address>(updateaddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updateaddress);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExsist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
