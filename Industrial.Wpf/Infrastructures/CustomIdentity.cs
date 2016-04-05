using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.Infrastructures
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, IEnumerable<RoleModel> roles)
        {
            Name = name;
            Roles = roles;
        }

        public string Name { get; private set; }
        public IEnumerable<RoleModel> Roles { get; private set; }

        public string AuthenticationType { get { return "Custom authentication"; } }
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
    }
}
