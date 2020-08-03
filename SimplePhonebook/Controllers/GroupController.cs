using SimplePhonebook.Data.Manager;
using SimplePhonebook.Data.Models;
using SimplePhonebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePhonebook.Controllers
{
    public class GroupController : Controller
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Group
        public ActionResult Index()
        {
            var model = new GroupViewModel()
            {
                Group = new Group(),
                Groups = new List<GroupSummary>()
            };

            try
            {
                var manager = new GroupManager();
                var groups = manager.GetGroups();
                groups.ForEach(x => model.Groups.Add(new GroupSummary()
                {
                    Group = x,
                    MembersCount = manager.GetGroupMembers(x.GroupId).Count()
                }));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(Group group)
        {
            var model = new GroupViewModel()
            {
                Group = new Group(),
                Groups = new List<GroupSummary>()
            };

            try
            {
                model.Group.GroupName = group.GroupName;
                var manager = new GroupManager();
                var groups = manager.GetGroups(group.GroupName ?? string.Empty);
                groups.ForEach(x => model.Groups.Add(new GroupSummary()
                {
                    Group = x,
                    MembersCount = manager.GetGroupMembers(x.GroupId).Count()
                }));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult GetProfiles()
        {
            try
            {
                var result = new ProfileManager().GetProfiles();
                return Json(new { status = true, profiles = result });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetGroupMembers(long groupId)
        {
            try
            {
                var result = new GroupManager().GetGroupMembers(groupId);
                return Json(new { status = true, members = result });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult AddGroup(Group group)
        {
            try
            {
                if (group.GroupId == 0)
                    new GroupManager().AddGroup(group);
                else
                    new GroupManager().UpdateGroup(group);
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteGroup(string ids)
        {
            try
            {
                new GroupManager().DeleteGroup(ids);
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