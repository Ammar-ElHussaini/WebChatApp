using ApiOpWebE_C.Models;
using ApiOpWebE_C.OperationResults;
using Microsoft.AspNetCore.Identity;
using WebChatApp.Service.Interfaces.AccountsService;

public class AccountsService : IAccountsService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserValidator _userValidator;

    public AccountsService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IUserValidator userValidator)
    {
        _userManager = userManager ;
        _signInManager = signInManager ;
        _userValidator = userValidator ;
    }

    public async Task<CreationResult<ApplicationUser>> RegisterUser(UserLoginModel userDto)
    {
        if (userDto == null)
        {
            return new CreationResult<ApplicationUser>
            {
                IsSuccess = false,
                Message = MessageResult.DataNull
            };
        }

        var existingUser = await _userValidator.GetUserByUsernameOrEmail(userDto.Username, userDto.Email);

        if (existingUser != null)
        {
            return new CreationResult<ApplicationUser>
            {
                IsSuccess = false,
                Message = MessageResult.IsFind
            };
        }

        ApplicationUser newUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userDto.Username,
            Email = userDto.Email
        };

        var result = await _userManager.CreateAsync(newUser, userDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new CreationResult<ApplicationUser>
            {
                Errors = errors,
                IsSuccess = false
            };
        }

        var signInResult = await _signInManager.PasswordSignInAsync(newUser, userDto.Password, true, true);

        return new CreationResult<ApplicationUser>
        {
            IsSuccess = true,
            Context = newUser
        };
    }

    public async Task<CreationResult<ApplicationUser>> LoginUser(UserLoginModel userDto)
    {
        if (userDto == null)
        {
            return new CreationResult<ApplicationUser>
            {
                IsSuccess = false,
                Message = MessageResult.DataNull
            };
        }

        var existingUser = await _userValidator.GetUserByUsernameOrEmail(userDto.Username, userDto.Email);

        if (existingUser == null)
        {
            return new CreationResult<ApplicationUser>
            {
                IsSuccess = false,
                Message = MessageResult.IsNotFind
            };
        }

        var result = await _signInManager.PasswordSignInAsync(existingUser, userDto.Password, false, false);

        if (result.Succeeded)
        {
            return new CreationResult<ApplicationUser>
            {
                Context = existingUser,
                IsSuccess = true
            };
        }

        return new CreationResult<ApplicationUser>
        {
            IsSuccess = false,
            Message = MessageResult.IsNotFind
        };
    }
}
