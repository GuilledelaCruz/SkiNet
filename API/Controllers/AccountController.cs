using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            AppUser user = await _userManager.FindByEmailFromClaimsPrincipalAsync(User);

            return new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            AppUser user = await _userManager.FindByEmailFromClaimsPrincipalWithAddressAsync(User);

            return _mapper.Map<Address, AddressDTO>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
        {
            AppUser user = await _userManager.FindByEmailFromClaimsPrincipalWithAddressAsync(User);
            user.Address = _mapper.Map<AddressDTO, Address>(address);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDTO>(user.Address));

            return BadRequest("Problem updating user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email already in use" } });

            AppUser user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }
    }
}