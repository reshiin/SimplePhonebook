using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplePhonebook.Data.Manager;
using SimplePhonebook.Data.Models;
using System.Collections.Generic;

namespace SimplePhonebook.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddProfile()
        {
            try
            {
                var manager = new ProfileManager();
                var contactNumbers = new List<ContactNumber>() {
                new ContactNumber() { Number = "09260381395", Type = "Home" },
                new ContactNumber() { Number = "09260381396", Type = "Telephone" },
            };

                var groupMembers = new List<GroupMember>();
                manager.AddProfile(new Profile
                {
                    FullName = "Jane Flores",
                    Email = "janemflores12896@gmail.com",
                    HomeAddress = "Makati",
                    ProfilePicture = System.IO.File.ReadAllBytes(@"D:\Images\JMF.jpg"),
                    ContactNumbers = contactNumbers,
                    GroupMembers = groupMembers
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void UpdateProfile()
        {
            try
            {
                var manager = new ProfileManager();
                var contactNumbers = new List<ContactNumber>() {
                new ContactNumber() { ProfileId = 2, ContactId = 3, Number = "09260381111", Type = "Home" },
                new ContactNumber() { ProfileId = 2, ContactId = 0, Number = "11111111111", Type = "Office" }
            };

                var groupMembers = new List<GroupMember>();
                manager.UpdateProfile(new Profile
                {
                    Id = 2,
                    FullName = "Jane Floresa",
                    Email = "janemflores12896@gmail.com",
                    HomeAddress = "Makati",
                    ProfilePicture = System.IO.File.ReadAllBytes(@"D:\Images\JMF.jpg"),
                    ContactNumbers = contactNumbers,
                    GroupMembers = groupMembers
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [TestMethod]
        public void DeleteProfile()
        {
            try
            {
                var manager = new ProfileManager();
                manager.DeleteProfile(2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
