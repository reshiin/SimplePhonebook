using SimplePhonebook.Data.DataAccess;
using SimplePhonebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Manager
{
    public class ProfileManager
    {
        private DataContext db = new DataContext();

        public List<Profile> GetProfiles(string fullName = "")
        {
            if(fullName.Trim() != string.Empty)
                return db.Profiles.Where(x => x.FullName.Contains(fullName)).OrderBy(x => x.FullName)?.ToList() ?? new List<Profile>();

            return db.Profiles.OrderBy(x => x.FullName)?.ToList() ?? new List<Profile>();
        }

        public Profile GetProfile(long id)
        {
            return db.Profiles.Find(id);
        }

        public List<ContactNumber> GetProfileContactNumbers(long id)
        {
            return db.ContactNumbers.Where(x => x.ProfileId == id)?.ToList() ?? new List<ContactNumber>();
        }

        public List<GroupMember> GetProfileGroups(long id)
        {
            return db.GroupMembers.Where(x => x.ProfileId == id)?.ToList() ?? new List<GroupMember>();
        }

        public void AddProfile(Profile profile)
        {
            db.Profiles.Add(profile);
            db.SaveChanges();
        }

        public void UpdateProfile(Profile profile)
        {
            var currentProfile = db.Profiles.Find(profile.Id);
            if (currentProfile == null) throw new Exception("Profile not found");
            currentProfile.FullName = profile.FullName;
            currentProfile.Email = profile.Email;
            currentProfile.HomeAddress = profile.HomeAddress;
            if(profile.ProfilePicture != null)
                currentProfile.ProfilePicture = profile.ProfilePicture;

            var currentContactNumbers = db.ContactNumbers.Where(x => x.ProfileId == profile.Id);
            currentContactNumbers.ToList().ForEach(x => {
                if (!profile.ContactNumbers.Any(y => y.ContactId == x.ContactId))
                    db.ContactNumbers.Remove(x);
            });

            profile.ContactNumbers.ToList().ForEach(x =>
            {
                var currentContact = db.ContactNumbers.Find(x.ContactId);
                if (currentContact == null) db.ContactNumbers.Add(x);
                else db.Entry(currentContact).CurrentValues.SetValues(x);
            });

            if (profile.GroupMembers.Count > 0)
            {
                var currentGroups = db.GroupMembers.Where(x => x.ProfileId == profile.Id)?.ToList();
                if (currentGroups != null) db.GroupMembers.RemoveRange(currentGroups);
                profile.GroupMembers.ToList().ForEach(x =>
                {
                    db.GroupMembers.Add(x);
                });
            }

            db.SaveChanges();
        }

        public void DeleteProfile(string ids)
        {
            foreach (var id in ids.Split(','))
            {
                var profile = db.Profiles.Find(Convert.ToInt64(id));
                db.Profiles.Remove(profile);
            }
            db.SaveChanges();
        }
    }
}
