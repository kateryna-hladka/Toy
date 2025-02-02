using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Toy.Models;
using Toy.Utilit;
using Toy.Utilit.DataBase;

namespace Toy.Controllers
{
    public class UserController : Controller
    {
        private readonly ToyContext _context;
        public UserController(ToyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Dictionary<string, string?> userExist = new();
            userExist.Add("user-loging", Request.Cookies["login"]);

            if (Request.Method == "POST")
            {

                string contactInfo = Request.Form["contact-info"].ToString();
                string password = Request.Form["password"].ToString();
                Dictionary<string, string> errors = new();

                if (!CheckData.CheckPhone(contactInfo) && !CheckData.CheckEmail(contactInfo))
                    errors.Add("error-contact-info", StaticVariables.contactInfo);

                if (!CheckData.CheckPassword(password))
                    errors.Add("error-password", StaticVariables.passwordInfo);

                if (errors.Count > 0)
                {
                    errors.Add("contact-info-data", contactInfo);
                    return View(errors);
                }
                else
                {
                    DataBaseHelper dataBase = new();
                    User? user = dataBase.GetUserByFilter(u => u.Email == contactInfo ||
                                                                u.Phone == contactInfo);

                    if (user is null || !BCrypt.Net.BCrypt.Verify(password, user?.Password))
                    {
                        errors.Add("contact-info-data", contactInfo);
                        errors.Add("error-sign-in", "Невірна пошта/телефон або пароль");
                        return View(errors);
                    }
                    if (Request.Cookies["login"] == null)
                        HttpContext.Response.Cookies.Append("login", user.Email ?? user.Phone, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(30),
                            HttpOnly = true
                        });

                    if (HttpContext.Session.GetString($"{HttpContext.Session.GetString("newUser")}_basket") != null)
                       return RedirectToAction("AddFromSession", "Basket");

                    return View("_Success", "Успішно");
                }
            }
            else
                return View(userExist);
        }

        public IActionResult Register()
        {
            Dictionary<string, string?> userExist = new();
            userExist.Add("user-loging", Request.Cookies["login"]);
            if (Request.Method == "POST")
            {
                string userName = Request.Form["user-name"].ToString();
                string userSurname = Request.Form["user-surname"].ToString();
                string contactInfo = Request.Form["contact-info"].ToString();
                string password = Request.Form["password"].ToString();

                bool phone = false, email = false;
                Dictionary<string, string> errors = new();

                if (!CheckData.CheckRegex(userName, StaticVariables.regUserName))
                    errors.Add("error-user-name", StaticVariables.nameInfo);

                if (!CheckData.CheckRegex(userSurname, StaticVariables.regUserSurname))
                    errors.Add("error-user-surname", StaticVariables.surnameInfo);

                if (CheckData.CheckPhone(contactInfo))
                {
                    DataBaseHelper dataBase = new();
                    User? user = dataBase.GetUserByFilter(u => u.Phone == contactInfo);

                    if (user is not null)
                        errors.Add("user-exist", "Користувач вже існує!");

                    phone = true;
                }
                else if (CheckData.CheckEmail(contactInfo))
                {
                    DataBaseHelper dataBase = new();
                    User? user = dataBase.GetUserByFilter(u => u.Email == contactInfo);

                    if (user is not null)
                        errors.Add("user-exist", "Користувач вже існує!");

                    email = true;
                }
                else
                    errors.Add("error-contact-info", StaticVariables.contactInfo);


                if (!CheckData.CheckPassword(password))
                    errors.Add("error-password", StaticVariables.passwordInfo);

                if (errors.Count > 0)
                {
                    errors.Add("user-name-data", userName);
                    errors.Add("user-surname-data", userSurname);
                    errors.Add("contact-info-data", contactInfo);
                    return View(errors);
                }
                else
                {
                    _context.User.Add(new User()
                        {
                            Name = userName,
                            Surname = userSurname,
                            Email = (email) ? contactInfo : null,
                            Phone = (phone) ? contactInfo : null,
                            Password = BCrypt.Net.BCrypt.HashPassword(password)
                        });
                    _context.SaveChanges();
                    
                    return View("_Success", "Успішно");
                }
            }
            else
                return View(userExist);

        }

        [Route("api/check-login")]
        public IActionResult CheckLoginStatus()
        {
            HttpContext.Session.SetString("newUser", HttpContext.Session.Id);

            if (Request.Cookies["login"] != null)
                return Json(new { loggedIn = true, session = false });

            else
                return Json(new { loggedIn = false, session = true });
        }
    }
}
