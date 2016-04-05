using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Core.Common.UI;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;

namespace Industrial.Wpf.ViewModels
{
    public class AuthenticationViewModel : ValidatableBindableBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly RelayCommand _loginCommand;
        private readonly RelayCommand _logoutCommand;
        private readonly RelayCommand _showViewCommand;
        private string _userName;
        private string _status;
        private string _password;


        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new RelayCommand(Login, CanLogin);
            _logoutCommand = new RelayCommand(Logout, CanLogout);
            _showViewCommand = new RelayCommand(ShowView, null);
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrators") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private void ShowView()
        {
            throw new NotImplementedException();
        }

        private bool CanLogout()
        {
            return IsAuthenticated;
        }

        private void Logout()
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }

        private bool CanLogin()
        {
            return !IsAuthenticated;
        }

        private void Login()
        {
          
            try
            {
                //Validate credentials through the authentication service
                UserModel user = _authenticationService.AuthenticateUser(UserName, Password);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.UserName, user.Roles);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                UserName = string.Empty; //reset
                Password = string.Empty; //reset
                Status = string.Empty;
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value);}
        }

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
