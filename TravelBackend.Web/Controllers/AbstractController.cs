using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TravelBackend.Services;

namespace TravelBackend.Web.Controllers
{
    public abstract class AbstractController : ApiController
    {
        protected Guid GetUserId() => User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.GetUserId()) : new Guid() ;
        protected bool IsUserAdmin
        {
            get
            {
                if (!IsUserSignedIn) return false;
                var svc = new RoleService(GetUserId());
                var rid = Guid.Parse(svc.GetRole(e => e.Name == "Admin").Id);
                return svc.IsUserInRole(GetUserId(),rid);
            }
        }
        protected bool IsUserSignedIn => User.Identity.IsAuthenticated;
        protected bool IsUserAdminOr(bool b) => b || IsUserAdmin;
    }
}