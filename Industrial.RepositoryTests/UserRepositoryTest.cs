using System;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using Core;
using Industrial.Data.Domain;
using Industrial.Data.Repositories;
using Industrial.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Industrial.Test.RepositoryTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void RepoCanSave()
        {
            IUserRepository _repository = new UserRepository();
            IUnitOfWorkFactory _unitOfWorkFactory = new EFUnitOfWorkFactory();

            var user = new User
            {
                CreatedDate = DateTime.Now,
                IsActive = true,
                PasswordHash = CalculateHash("password", "admin"),
                UserName = "Admin",
                Id = Guid.NewGuid()

            };
            User result;
            using (_unitOfWorkFactory.Create())
            {
                result = _repository.Add(user);
            }

            Assert.AreNotEqual(0,result.Id);
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