﻿using LtePlatform.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LtePlatform.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                Email = await UserManager.GetEmailAsync(userId),
                EmailHasBeenConfirmed = await UserManager.IsEmailConfirmedAsync(userId)
            };
            UserContextConfiguration.CurrentUser = model;
            return View();
        }

        public async Task<ActionResult> ConfirmEmail(IndexViewModel model)
        {
            var userId = User.Identity.GetUserId();
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userId, "确认你的帐户", "请通过单击<a href=\"" + callbackUrl + "\">此处</a>来确认你的帐户");
            return Json(new { Type = "success", Message = "确认邮箱链接的电子邮件已发送到" +  model.Email});
        }

        [HttpGet]
        public async Task<ActionResult> EmailHasBeenConfirmed(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null) return Json(new {Name = userName, Result = false}, JsonRequestBehavior.AllowGet);
            bool result = await UserManager.IsEmailConfirmedAsync(user.Id);
            return Json(new { Name = userName, Result = result}, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }
        
        [HttpPost]
        public async Task<ActionResult> AddPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            // 生成令牌并发送该令牌
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.PhoneNumber);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.PhoneNumber,
                    Body = "你的安全代码是: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }

            return Json(new VerifyPhoneNumberViewModel
            {
                PhoneNumber = model.PhoneNumber,
                Code = code
            });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }
        
        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (!result.Succeeded)
            {
                var message = result.Errors.Aggregate("验证电话号码失败！", (current, errorMessage) => current + (";" + errorMessage));
                return Json(message);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return Json("验证电话号码成功！");
        }

        //
        // GET: /Manage/RemovePhoneNumber
        [HttpPost]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                var message = result.Errors.Aggregate("删除电话号码失败！", (current, errorMessage) => current + (";" + errorMessage));
                return Json(message);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return Json("删除电话号码成功！");
        }
        
        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.Aggregate("修改密码失败！", (current, errorMessage) => current + (";" + errorMessage));
                return Json(new {Type = "warning", Message = message});
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return Json(new {Type = "warning", Message = "获取当前用户信息失败！"});
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return Json(new { Type = "success", Message = "修改密码成功！"});
        }
        
        //
        // POST: /Manage/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var userByName = await UserManager.FindByNameAsync(model.UserName);
            if (userByName == null)
                return Json(new {Type = "warning", Message = "用户名称对应的用户不存在！"});
            var userByEmail = await UserManager.FindByEmailAsync(model.Email);
            if (userByEmail == null)
                return Json(new { Type = "warning", Message = "用户邮箱对应的用户不存在！" });
            if (userByEmail.UserName!=model.UserName)
                return Json(new { Type = "warning", Message = "用户邮箱和用户名不匹配！" });
            if (!(await UserManager.IsEmailConfirmedAsync(userByName.Id)))
            {
                return Json(new { Type = "warning", Message = "该用户邮箱未经确认！" });
            }
            
            string code = await UserManager.GeneratePasswordResetTokenAsync(userByEmail.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userByEmail.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userByEmail.Id, "重置密码", "请通过单击<a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
            return Json(new { Type = "success", Message = "启用帐户确认和密码重置链接的电子邮件已发送到" +  model.Email});
        }

        //
        // POST: /Manage/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userByName = await UserManager.FindByNameAsync(model.UserName);
            if (userByName == null)
                return Json(new { Type = "warning", Message = "用户名称对应的用户不存在！" });
            var userByEmail = await UserManager.FindByEmailAsync(model.Email);
            if (userByEmail == null)
                return Json(new { Type = "warning", Message = "用户邮箱对应的用户不存在！" });
            if (userByEmail.UserName != model.UserName)
                return Json(new { Type = "warning", Message = "用户邮箱和用户名不匹配！" });
            var result = await UserManager.ResetPasswordAsync(userByName.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Json(new { Type = "success", Message = "用户密码修改成功，请重新登陆！" });
            }
            return Json(new { Type = "warning", Message = "用户密码修改失败！" });
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "已删除外部登录名。"
                : message == ManageMessageId.Error ? "出现错误。"
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // 请求重定向至外部登录提供程序，以链接当前用户的登录名
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}