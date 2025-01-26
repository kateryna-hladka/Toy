﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Toy.Models;
using Toy.Utilit;

namespace Toy.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            Dictionary<string, string?> userExist = new();
            userExist.Add("user-loging", Request.Cookies["user-login"]);

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
                    if (HttpContext.Session.GetInt32("userId") == null)
                    {
                        HttpContext.Session.SetInt32("userId", user.Id);
                        HttpContext.Response.Cookies.Append("user-login", "exist");
                    }
                    return View("_Success");
                }
            }
            else
                return View(userExist);
        }

        public IActionResult Register()
        {
            Dictionary<string, string?> userExist = new();
            userExist.Add("user-loging", Request.Cookies["user-login"]);
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
                    using (ToyContext toyContext = new())
                    {

                        toyContext.User.Add(new User()
                        {
                            Name = userName,
                            Surname = userSurname,
                            Email = (email) ? contactInfo : null,
                            Phone = (phone) ? contactInfo : null,
                            Password = BCrypt.Net.BCrypt.HashPassword(password)
                        });
                        toyContext.SaveChanges();
                    }
                    return View("_Success");
                }
            }
            else
                return View(userExist);

        }
    }
}
