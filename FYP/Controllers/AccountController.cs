using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using FYP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace FYP.Controllers
{
    public class AccountController : Controller
    {   
        private const string LOGIN_SQL =
        @"SELECT * FROM UserRegister 
            WHERE UserId = '{0}' 
              AND UserPw = HASHBYTES('SHA1', '{1}')";

        private const string LASTLOGIN_SQL =
           @"UPDATE UserRegister SET LastLogin=GETDATE() WHERE UserId='{0}'";

        private const string ROLE_COL = "UserRole";
        private const string NAME_COL = "FirstName";

        private const string REDIRECT_CNTR = "Home";
        private const string REDIRECT_ACTN = "Index";
        
        private const string LOGIN_VIEW = "UserLogin";

        [AllowAnonymous]
        public IActionResult UserLogin(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult UserLogin(UserLogin user)
        {
            if (!AuthenticateUser(user.UserID, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }
            else
            {

                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal,
                   new AuthenticationProperties
                   {
                       IsPersistent = user.RememberMe // <--- Here
                   });

                // Update the Last Login Timestamp of the User
                DBUtl.ExecSQL(LASTLOGIN_SQL, user.UserID);

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            List<UserRegister> list = DBUtl.GetList<UserRegister>("SELECT * FROM UserRegister WHERE UserRole='member' ");
            return View(list);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Delete(string id)
        {
            string delete = "DELETE FROM UserRegister WHERE UserId='{0}'";
            int res = DBUtl.ExecSQL(delete, id);
            if (res == 1)
            {
                TempData["Message"] = "User Record Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("Users");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegister usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Register");
            }
            else
            {
                IFormCollection form = HttpContext.Request.Form;
                string refer = form["Region"].ToString().Trim();

                string insert =
                                @"INSERT INTO UserRegister(UserId, UserPw, FirstName, LastName, Email, ContactNo, StreetAddress, UnitNo, Postal, Region, UserRole) VALUES
                  ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}','{4}','{5}','{6}','{7}', '{8}', '{9}', 'member' )";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FirstName, usr.LastName, usr.Email, usr.ContactNo, usr.StreetAddress, usr.UnitNo, usr.Postal, usr.Region, usr.UserRole) == 1)
                {
                    string template = @"<h2> Hi {0}, </h2><br/>
                                <h1 style='color:red;'> Welcome to FIRE & GAS SENSOR FOR RESIDENTIAL PROPERTIES </h1>
                                <h3 style='color:blue;'> 🔥 PROJECT ID: SOI-2020-2010-0033 🔥 </h3><br/> 
                                Your User ID is <b style='background-color:yellow;'> {1} </b>  and Password is <b style='background-color:yellow;'> {2} </b>.
                                <br><br/>Member";
                    
                    string title = "Registration Successul - Welcome";
                    string message = String.Format(template, usr.FirstName, usr.UserId, usr.UserPw);
                    string result;


                    if (EmailUtl.SendEmail(usr.Email, title, message, out result))
                    {
                        
                        ViewData["Message"] = "User Successfully Registered";
                        ViewData["MsgType"] = "success";
                        return View("UserLogin");

                    }


                    else
                    {
                        ViewData["Message"] = result;
                        ViewData["MsgType"] = "warning";
                    }
                }
                else
                {
                    ViewData["Message"] = "User ID already exist";
                    ViewData["MsgType"] = "danger";
                    return View("Register");
                }
                return View("Register");
            }
        }

        [AllowAnonymous]
        public IActionResult VerifyUserID(string userId)
        {
            string select = $"SELECT * FROM UserRegister WHERE UserId='{userId}'";
            if (DBUtl.GetTable(select).Rows.Count > 0)
            {
                return Json($"[{userId}] already in use");
            }
            return Json(true);
        }

        private bool AuthenticateUser(string UserId, string UserPw, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, UserId, UserPw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, UserId),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0][ROLE_COL].ToString())
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }


    }
}