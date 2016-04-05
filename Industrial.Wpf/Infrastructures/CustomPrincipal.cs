using System.Linq;
using System.Security.Principal;
using Industrial.Data.Domain;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.Infrastructures
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _identity;

        public CustomIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        #region IPrincipal Members
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.Roles.Any(a => a.Name.Equals(role));
        }
        #endregion
    }
}