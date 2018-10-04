using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Tasks.Model;
using Tasks.Model.Abstract;
using Tasks.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Security.Cryptography;
using Tasks.Helpers;
using Tasks.Abstract;

namespace Tasks.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IUserTasksRepository _userRepository;
        private IEmailSender _mailSender;
        private IAppSettings _appSettings;

        public AuthController(IUserTasksRepository userRepository, IEmailSender mailSender, IAppSettings appSettings)
        {
            _userRepository = userRepository;
            _mailSender = mailSender;
            _appSettings = appSettings;
        }

        // GET: Auth
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel vm = new LoginViewModel() { ReturnUrl = returnUrl };
            return View(vm);
        }

        // POST: Auth
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            //получение user из базы
            User user = _userRepository.GetUser(model.UserName, model.Password);
            
            if(user == null)
            {
                //пользоватлель в БД не найден
                ModelState.AddModelError("", "Invalid user name or password!");
                return View(model);
            }

            //создание объекта хранящего в себе информацию о пользователе
            AppUserState appUserState = new AppUserState()
            {
                UserName = user.UserName,
                Role = user.RoleString
            };

            //авторизация
            IdentitySignin(appUserState);

            if (!string.IsNullOrEmpty(model.ReturnUrl))
                //если пользователь хотел открыть какую-то страницу и она не открылась
                //из-за того, что он был не авторизован, вернуть ее.
                return Redirect(model.ReturnUrl);

            //в противном случае вернуть список задач, это view по умолчанию
            return Redirect(Url.Action("TaskList", "Home"));
        }

        public ViewResult Logout() {
            IdentitySignOut();
            return View();
        }

        public ViewResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ViewResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            //e-mail точно не пустой
            IEnumerable<User> emailUsers = _userRepository.GetUserByEmail(model.Email);

            if (emailUsers.Count() > 1)
            {
                TempData["Error"]="More than one user exists with this email!";
                return View();
            }

            //найден один пользователь с таким e-mail
            // сохраняем его
            User user = emailUsers.First();

            string tokenStr = GenerateRandomToken();
            //сохраняем токен в базе
            user.PasswordResetToken = tokenStr;
            user.PasswordResetTokenDate = DateTime.Now;

            _userRepository.Save();
            // и в ссылке пользователю
            string callbackUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content(Url.Action("ChangePassword", new { token = tokenStr, userId = user.UserName }))}";
            string mailBody = String.Format("Please reset your password by clicking <a href='" + callbackUrl + "'>here</a>");
            _mailSender.SendEmail(user.Email, "Task4u: Reset Password", mailBody);

            return View("ResetEmailIsSent");
        }

        public ActionResult ChangePassword(string token, string userid)
        {
            User user = _userRepository.GetUserByToken(userid, token);
            if (user==null || 
                DateTime.Now.Subtract(user.PasswordResetTokenDate.Value).TotalSeconds > 
                    _appSettings.ResetPasswordTokenExpireTime)
            {
                return HttpNotFound();
            }
            ChangePasswordViewModel vm = new ChangePasswordViewModel() { Token = token, UserId = userid };

            return View(vm);
        }

        [HttpPost]
        public ActionResult ChangePassword(string token, string userid, string newPwd)
        {
            User user = _userRepository.GetUserByToken(userid, token);
            if (user == null ||
                DateTime.Now.Subtract(user.PasswordResetTokenDate.Value).TotalSeconds >
                    _appSettings.ResetPasswordTokenExpireTime)
            {
                return HttpNotFound();
            }

            user.Password = newPwd;
            user.PasswordResetToken = null;
            user.PasswordResetTokenDate = null;
            _userRepository.Save();

            return View("ChangePwdSuccess");
        }
        private string GenerateRandomToken()
        {
            String result;
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {

                Byte[] bytes = new Byte[10];
                rng.GetBytes(bytes);

                result = Convert.ToBase64String(bytes);
            }

            return result;
        }
        private void IdentitySignin (AppUserState appUserState, bool isPesistent =false)
        {
            List<Claim> claims = new List<Claim>();

            //create required claims
            //claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserState.UserId));
            string[] roles = appUserState.Role.Split(';');
            claims.Add(new Claim(ClaimTypes.Name, appUserState.UserName));
            foreach (string r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }
            ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPesistent,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            }, identity);
            
        }

        private void IdentitySignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}