using ShoppingCartApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ShoppingCartApp.Models
{
    /// <summary>
    /// Executed on application startup to create default user account
    /// </summary>
    public class InitializeAdmin
    {
        #region Private Fields
        private ApplicationDbContext context; 
        #endregion

        #region Constructor
        public InitializeAdmin()
        {
            context = new ApplicationDbContext();
            InitializeIdentityAdmin(context);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates Admin Role and Administrator account if they don't exist.
        /// </summary>
        /// <param name="context"></param>
        private void InitializeIdentityAdmin(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string name = "Admin";
            string password = "P@ssw0rd";
            string email = "administrator@catch.com";

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(name))
            {
                var roleresult = RoleManager.Create(new IdentityRole(name));
            }

            //Create User=Admin with password
            if (!UserManager.Users.Any(u => u.UserName == email))
            {
                var user = new ApplicationUser();
                user.UserName = email;
                user.Email = email;
                user.LockoutEnabled = true;
                var adminresult = UserManager.Create(user, password);

                if (adminresult.Succeeded)
                {
                    //Add User Admin to Role Admin
                    var result = UserManager.AddToRole(user.Id, name);
                }
            }
        } 
        #endregion
    }
}
