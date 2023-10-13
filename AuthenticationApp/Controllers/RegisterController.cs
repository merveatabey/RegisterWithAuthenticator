using AuthenticationApp.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using SendGrid;

namespace AuthenticationApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;

        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Name = userRegister.Name,
                    Surname = userRegister.Surname,
                    UserName = userRegister.UserName,
                    Email = userRegister.Email
                };

                var result = await _userManager.CreateAsync(user, userRegister.Password);
                if (result.Succeeded)
                {
                    //iki aşamalı doğrulama kodunu oluştur
                    var twoFactorKey = _userManager.GenerateTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider);
                   
                    // Kullanıcıya iki aşamalı doğrulama kodunu e-posta ile gönderin
                    SendTwoFactorEmail(user.Email, twoFactorKey);

                    // Kullanıcıyı e-posta doğrulama sayfasına yönlendirin
                    return RedirectToAction("VerifyEmail");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }               
            }
            return View(userRegister);
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // Kullanıcının girdiği iki aşamalı doğrulama kodunu doğrulayın
            var isTwoFactorValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);

            if (isTwoFactorValid)
            {
                // Kullanıcının e-posta adresini onaylayın
                await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));

                return RedirectToAction("Index"); // Kayıt işlemi tamamlandı
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz iki aşamalı doğrulama kodu.");
                return View();
            }
        }

        private void SendTwoFactorEmail(string email, Task<string> twoFactorKey)
        {
            throw new NotImplementedException();
        }
    }
}
