using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebApp.Hubs;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        private readonly IHubContext<MyHub> _hubContext;
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHubContext<MyHub> hubContext):base(userManager,signInManager)
        {
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {

            AppUser user = CurrentUser;//async
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);
        }

        public void Logout()
        {
            signInManager.SignOutAsync();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
