using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApp.Models;
using WebApp.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,  AppIdentityDbContext dbContext):base(userManager,signInManager,dbContext)
        {
            
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user!=null)
                {
                    await signInManager.SignOutAsync();
                    SignInResult result= await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (!userManager.IsEmailConfirmedAsync(user).Result)
                    {
                        ModelState.AddModelError("","Email adresi onaylanmamıştır");
                        return View(loginViewModel);
                    }

                    if (result.Succeeded)
                    {
                        #region Oturum Açma İşleminin Süresi DB'ye eklendiği Kısım

                        watch.Stop();
                        int time = watch.Elapsed.Milliseconds;
                        DateTime dt = DateTime.Now;
                        user.StopWatch = time;
                        user.DateTime = dt;
                        await userManager.UpdateAsync(user);

                        #endregion


                        if (TempData["ReturnUrl"]!=null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Geçersiz email veya şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Geçersiz email veya şifre");
                }
            }

            
            return View(loginViewModel);
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.FirstName = userViewModel.FirstName;
                user.Lastname = userViewModel.Lastname;
                user.Email = userViewModel.Email;

                IdentityResult result=await userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action("ConfirmEmail", "Home", new
                    {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);
                    Helper.EmailConfirmation.SendConfirmEmail(link,user.Email);
                    return RedirectToAction("Login");
                }
                else
                {
                    AddModelError(result);
                }
        
                return View(userViewModel);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
               AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;//async
                if (user!=null)
                {
                    string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;
                    string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                    {
                        userId=user.Id,
                        token=passwordResetToken

                    },HttpContext.Request.Scheme);
                    Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink,passwordResetViewModel.Email);
                    ViewBag.status = "successfull";
                }
                else
                {
                    ModelState.AddModelError("","Kayıtlı e-mail adresi bulunamamıştır");
                }
                return View(passwordResetViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")]PasswordResetViewModel passwordResetViewModel)
        {
            
                string token = TempData["token"].ToString();
                string userId = TempData["userId"].ToString();
                AppUser user = await userManager.FindByIdAsync(userId);

                if (user!=null)
                {
                    IdentityResult result =
                        await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);
                    if (result.Succeeded)
                    {
                        await userManager.UpdateSecurityStampAsync(user);
                        TempData["passwordResetInfo"]="Şifre başarıyla yenilendi.";
                        ViewBag.status = "success";
                    }
                    else
                    {
                        AddModelError(result);    
                    }
                }
                return View(passwordResetViewModel);
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.status = "Email adresiniz onaylanmıştır.";
            }
            else
            {
                ViewBag.status = "Bir hata meydana geldi. lütfen daha sonra tekrar deneyiniz.";
            }
            return View();
        }
    }
}
