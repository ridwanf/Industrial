using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Industrial.Data.Domain;
using Industrial.Repository.Repositories;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.Infrastructures
{
    public interface IAuthenticationService
    {
        UserModel AuthenticateUser(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {

        private readonly IRoleService _roleService;
        private readonly IUserService  _userService;


        public AuthenticationService(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public UserModel AuthenticateUser(string username, string password)
        {
            var user = _userService.GetAll().FirstOrDefault(a => a.UserName.Equals(username)
                                                                 && a.PasswordHash.Equals(CalculateHash(password,username)));
            if (user == null)
            {
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");
            }
            return user;
        }
 
        private string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }
 


    
}