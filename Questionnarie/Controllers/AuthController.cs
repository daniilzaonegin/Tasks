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

namespace Tasks.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IUserTasksRepository _userRepository;

        public AuthController(IUserTasksRepository userRepository)
        {
            _userRepository = userRepository;
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