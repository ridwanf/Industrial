using System;
using System.Linq;
using Core;
using Industrial.Data.Domain;
using Industrial.Data.Repositories;
using Industrial.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Industrial.Test.RepositoryTests
{
    [TestClass]
    public class ItemRepositoryTest
    {

        [TestMethod]
        public void RepoCanSave()
        {
            IItemProductRepository productRepository = new ItemProductRepository();
            IUnitOfWorkFactory _unitOfWorkFactory = new EFUnitOfWorkFactory();

            var item = new ItemProduct
            {
                Description = "Description From Test",
                IsActive = true,
                Name = "Name From Test",
                Price = 45000,
                CreatedDate = DateTime.Now,
                Quantity = 40,
            };
            ItemProduct result;

            using (_unitOfWorkFactory.Create())
            {
                result = productRepository.Add(item);
            }
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void RepoCanGet()
        {
            IItemProductRepository productRepository = new ItemProductRepository();

            var result = productRepository.FindAll();

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void RepoCanGetById()
        {
            IItemProductRepository productRepository = new ItemProductRepository();

            var result = productRepository.FindBy(a => a.Id == 1).FirstOrDefault();

            Assert.AreEqual(45000, result.Price);

        }

        [TestMethod]
        public void RepoCanUpdate()
        {
            IItemProductRepository productRepository = new ItemProductRepository();
            IUnitOfWorkFactory _unitOfWorkFactory = new EFUnitOfWorkFactory();

            var item = productRepository.FindBy(a => a.Id == 1).First();
            item.Description = "Description From Test Edit";
            item.IsActive = false;
            item.Name = "Name From Test edit";
            item.Price = 5000;
            ItemProduct result;

            using (_unitOfWorkFactory.Create())
            {
                result = productRepository.Update(item);
            }
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void RepoCanDelete()
        {
            IItemProductRepository productRepository = new ItemProductRepository();
            IUnitOfWorkFactory _unitOfWorkFactory = new EFUnitOfWorkFactory();

            var item = productRepository.FindBy(a => a.Id == 1).First();


            using (_unitOfWorkFactory.Create())
            {
                productRepository.Remove(item);
            }


        }

        [TestMethod]
        public void RepoCanSoftDelete()
        {
            IItemProductRepository productRepository = new ItemProductRepository();
            IUnitOfWorkFactory _unitOfWorkFactory = new EFUnitOfWorkFactory();

            var item = productRepository.FindBy(a => a.Id == 2).First();


            using (_unitOfWorkFactory.Create())
            {
                productRepository.SoftDelete(item);
            }


        }
    }
}


