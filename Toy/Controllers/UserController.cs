using Microsoft.AspNetCore.Mvc;
using Toy.Models;
using Toy.Utilit;

namespace Toy.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Method == "POST")
            {

                string contactInfo = Request.Form["contact-info"].ToString();
                string password = Request.Form["password"].ToString();
                Dictionary<string, string> errors = new();

                if ((!CheckData.CheckRegex(contactInfo, StaticVariables.regEmail) &&
                    !CheckData.CheckRegex(contactInfo, StaticVariables.regPhone)) ||
                    contactInfo.Length < 5
                    )
                    errors.Add("error-contact-info", StaticVariables.contactInfo);

                if (password.Length < 4 || password.Length > 8)
                    errors.Add("error-password", StaticVariables.passwordInfo);

                if (errors.Count > 0)
                {
                    errors.Add("contact-info-data", contactInfo);
                    return View(errors);
                }
                else
                {
                    using (ToyContext toyContext = new())
                    {
                        int user = toyContext.User
                                    .Where(u =>
                                    (u.Email == contactInfo ||
                                     u.Phone == contactInfo) &&
                                     u.Password == password
                                    )
                                    .Select(u => u.Id)
                                    .FirstOrDefault();


                        Console.WriteLine(user);

                        if (user == 0)
                        {
                            errors.Add("error-sign-in", "Невірна пошта/телефон або пароль");
                            return View(errors);
                        }
                    }
                }

                return View();
            }
            else
                return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
