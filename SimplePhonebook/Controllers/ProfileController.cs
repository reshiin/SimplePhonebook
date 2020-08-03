using Newtonsoft.Json;
using SimplePhonebook.Data.Manager;
using SimplePhonebook.Data.Models;
using SimplePhonebook.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePhonebook.Controllers
{
    public class ProfileController : Controller
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Profile
        public ActionResult Index()
        {
            var model = new ProfileViewModel();
            try
            {
                model.Profiles = new ProfileManager().GetProfiles();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(Profile profile)
        {
            var model = new ProfileViewModel();
            try
            {
                model.Profiles = new ProfileManager().GetProfiles(profile.FullName ?? string.Empty);
                model.Profile.FullName = profile.FullName;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult GetGroups()
        {
            try
            {
                var result = new GroupManager().GetGroups();
                return Json(new { status = true, groups = result });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetProfile(Profile profile)
        {
            try
            {
                var manager = new ProfileManager();
                var result = manager.GetProfile(profile.Id);
                result.GroupMembers = manager.GetProfileGroups(profile.Id);
                result.ContactNumbers = manager.GetProfileContactNumbers(profile.Id);
                return Json(new { status = true, profile = result });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult AddProfile(HttpPostedFileBase image, string profile)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<Profile>(profile);
                if (image != null)
                {
                    var imageStream = new MemoryStream();
                    image.InputStream.CopyTo(imageStream);
                    model.ProfilePicture = imageStream.ToArray();
                }
                if (model.Id == 0)
                    new ProfileManager().AddProfile(model);
                else
                    new ProfileManager().UpdateProfile(model);
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteProfile(string ids)
        {
            try
            {
                new ProfileManager().DeleteProfile(ids);
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}