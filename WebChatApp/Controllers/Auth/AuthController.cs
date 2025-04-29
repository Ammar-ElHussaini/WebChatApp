using ApiOpWebE_C.Models;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebChatApp.Service.Interfaces.AccountsService;

namespace WebChatApp.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountsService _accountsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, AccountsService accountsService)
        {
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _accountsService = accountsService;
        }


        [HttpPost]
        public async Task<IActionResult> RigsterUser(UserLoginModel userLoginModel)
        {
            var s = await _accountsService.RegisterUser(userLoginModel);

            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var user = await _userManager.FindByNameAsync(userLoginModel.Username);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> loginUser(UserLoginModel userLoginModel)
        {

            var s = await _accountsService.LoginUser(userLoginModel);

            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var user = await _userManager.FindByNameAsync(userLoginModel.Username);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View("Index");
        }
    }
}
