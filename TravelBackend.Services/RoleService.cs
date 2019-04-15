using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;

/***********************************************************************************************************
 * ASK BEFORE CHANGING THIS FILE
 ***********************************************************************************************************/
namespace TravelBackend.Services
{
    public class RoleService : AbstractService
    {
        private static readonly string ADMIN_HARDCODED_EMAIL = "admin@admin.com";
        private static readonly string ADMIN_HADRCODED_PASS = "admin@1234";

        private static RoleManager<IdentityRole> RoleManager;
        private static UserManager<ApplicationUser> UserManager;
        private static IdentityRole RoleAdmin = new IdentityRole("Admin");
        private static ApplicationUser UserAdmin;

        public RoleService() : base()
        {
            if (RoleManager == null) RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Context));
            if (UserManager == null) UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            if (Context.Roles.Where(e => e.Name == "User").Count() < 1)
            {
                Guid id;
                IEnumerable<string> errors;
                if (!CreateRole("User",out id,out errors))
                {
                    throw new Exception("Some errors occured!");
                }
                Context.SaveChanges();
            }
            Context.SaveChanges();
            foreach (var user in Context.Users)
            {
                if (user.Roles.Count < 1) new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context)).AddToRoleAsync(user.Id,"User");
                Context.SaveChanges();
            }
            if (Context.Roles.Where(e=>e.Name=="Admin").Count() < 1) {
                IEnumerable<string> errors;
                var res = Task.Run(async () => { return await UserManager.CreateAsync(new ApplicationUser() { UserName = ADMIN_HARDCODED_EMAIL, Email=ADMIN_HARDCODED_EMAIL },ADMIN_HADRCODED_PASS); }).Result;
                if (!res.Succeeded) {
                    errors = res.Errors;
                    throw new Exception("Some errors occured!");
                }
                UserAdmin = UserManager.FindByEmail(ADMIN_HARDCODED_EMAIL);
                Guid adminRoleId;
                if (!CreateRole("Admin",out adminRoleId,out errors))
                {
                    throw new Exception("Some errors occured!");
                }
                if (!AssignRoleToUser(adminRoleId,Guid.Parse(UserAdmin.Id),out errors))
                {
                    throw new Exception("Some errors occured!");
                }
                Context.SaveChanges();
            }
        }

        private class CreateUserException : Exception
        {
            public IEnumerable<string> Errors;

            public CreateUserException(string msg,IEnumerable<string> errors) : base(msg) { Errors = errors.ToArray(); }
        }

        private bool CreateUser(string email,string pass,out IEnumerable<string> errors)
        {
            try
            {
                var user = new ApplicationUser() { UserName = email,Email = email };
                UserManager.CreateAsync(user,pass);
                //CreateUserAsync(email,pass);
            }
            catch (CreateUserException e)
            {
                errors = e.Errors;
                return false;
            }
            errors = null;
            return true;
        }
        private async void CreateUserAsync(string email,string pass)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var user = new ApplicationUser() { UserName = email,Email = email };

                var usm = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                IdentityResult result = await usm.CreateAsync(user,pass);

                if (result.Errors.Count() > 0)
                {
                    throw new CreateUserException($"{result.Errors.Count()} error(s) occured while creating the new User...",result.Errors);
                }

                ctx.SaveChanges();
            }
        }

        public IdentityRole[] GetRoles(Expression<Func<IdentityRole,bool>> x) => Context.Roles.Where(x).ToArray();
        public IdentityRole GetRole(Expression<Func<IdentityRole,bool>> x) => GetRoles(x)[0];

        public IdentityRole GetRoleById(Guid roleId) => GetRole(e => e.Id == roleId.ToString());
        public IdentityRole GetRoleOfUser(string uid) => GetRoleOfUser(Guid.Parse(uid));
        public IdentityRole GetRoleOfUser(Guid userId)
        {
            var user = Context.Users.Where(e => e.Id == userId.ToString()).ToArray()[0];
            var roles = user.Roles.ToArray();
            if (roles.Length < 1) return Context.Roles.Where(e => e.Name == "User").Single();
            return GetRoleById(Guid.Parse(roles[0].RoleId));
        }
        public ApplicationUser[] GetUsersInRole(Guid roleId) => Context.Users.Where(e => GetRoleOfUser(e.Id).Id == roleId.ToString()).ToArray();
        public bool IsUserInRole(Guid uid,Guid roleId) => Context.Users.Where(e => e.Id == uid.ToString()).Single().Roles.Where(e=>e.RoleId==roleId.ToString()).Count() > 0;

        public bool CreateRole(string name,out Guid id,out IEnumerable<string> errors)
        {
            var r = new IdentityRole(name);
            var result = RoleManager.Create(r);
            id = Guid.Parse(r.Id);
            errors = result.Errors;
            return result.Succeeded;
        }
        public bool DeleteRole(Guid roleId,out IEnumerable<string> errors)
        {
            var res = RoleManager.Delete(GetRoleById(roleId));
            errors = res.Errors;
            return res.Succeeded;
        }
        public bool AssignRoleToUser(Guid roleId,Guid userId,out IEnumerable<string> errors)
        {
            var idS = userId.ToString();
            UserManager.RemoveFromRoles(idS,GetRoleOfUser(userId).Id);
            var res = UserManager.AddToRole(idS,GetRoleById(roleId).Name);
            errors = res.Errors;
            return res.Succeeded;
        }
    }
}
