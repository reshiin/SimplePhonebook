using SimplePhonebook.Data.DataAccess;
using SimplePhonebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Manager
{
    public class GroupManager
    {
        private DataContext db = new DataContext();

        public List<Group> GetGroups(string groupName = "")
        {
            if(groupName != string.Empty)
                return db.Groups.Where(x => x.GroupName.Contains(groupName)).OrderBy(x => x.GroupName)?.ToList() ?? new List<Group>();
            return db.Groups.OrderBy(x => x.GroupName)?.ToList() ?? new List<Group>();
        }

        public Group GetGroup(long id)
        {
            return db.Groups.Find(id);
        }

        public List<GroupMember> GetGroupMembers(long id)
        {
            return db.GroupMembers.Where(x => x.GroupId == id)?.ToList() ?? new List<GroupMember>();
        }

        public void AddGroup(Group group)
        {
            db.Groups.Add(group);

            db.SaveChanges();
        }

        public void UpdateGroup(Group group)
        {
            var currenyGroup = db.Groups.Find(group.GroupId);
            db.Entry(currenyGroup).CurrentValues.SetValues(group);
            var groupMembers = db.GroupMembers.Where(x => x.GroupId == group.GroupId)?.ToList();
            if (groupMembers != null) db.GroupMembers.RemoveRange(groupMembers);
            if (group.GroupMembers.Count > 0) db.GroupMembers.AddRange(group.GroupMembers);
            db.SaveChanges();
        }

        public void DeleteGroup(string ids)
        {
            foreach (var id in ids.Split(','))
            {
                var group = db.Groups.Find(Convert.ToInt64(id));
                db.Groups.Remove(group);
            }
            db.SaveChanges();
        }
    }
}
