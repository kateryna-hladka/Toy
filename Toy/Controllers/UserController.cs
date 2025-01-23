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

                        if (user == 0)
                        {
                            errors.Add("contact-info-data", contactInfo);
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
                    using (ToyContext toyContext = new ())
                    {
                        int user = toyContext.User
                                    .Where(u => u.Phone == contactInfo)
                                    .Select(u => u.Id)
                                    .FirstOrDefault();
                        if (user != 0)
                            errors.Add("user-exist", "Користувач вже існує!");
                    }
                    phone = true;
                }
                else if (CheckData.CheckEmail(contactInfo))
                {
                    using(ToyContext toyContext = new())
                    {
                        int user = toyContext.User
                                    .Where(u => u.Email == contactInfo)
                                    .Select(u => u.Id)
                                    .FirstOrDefault();
                        if (user != 0)
                            errors.Add("user-exist", "Користувач вже існує!");
                    }
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
                    using (ToyContext toyContext = new())
                    {
                        toyContext.User.Add(new User() 
                        { Name = userName, Surname = userSurname, 
                          Email = (email)? contactInfo : null, 
                          Phone = (phone)? contactInfo : null,
                          Password = password
                        });
                        toyContext.SaveChanges();
                    }
                    return View("_Success");
                }
            }
            else
                return View();

        }
    }
}
