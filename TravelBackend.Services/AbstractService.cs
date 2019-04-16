using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;

namespace TravelBackend.Services
{
    public abstract class AbstractService
    {
        public static ApplicationDbContext Context;
        public Guid _userId;

        public AbstractService() : this(new Guid()) { }
        public AbstractService(Guid uid)
        {
            this._userId = uid;
            if (Context == null) Context = new ApplicationDbContext();
        }

        public RoleService CreateRoleService() => new RoleService(this._userId);
    }
}
