using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystemTest
{
    [TestClass]
    public class RepositoryTest
    {
        private Repository repository;

        public RepositoryTest()
        {
            ContentGenerator generator = new ContentGenerator();
            repository = new Repository(generator.Create());
        }


        [TestMethod]
        public void CheckInitialState()
        {
            Assert.IsTrue(repository.GetAllClients().Count.Equals(2));
            Assert.IsTrue(repository.GetAllEvents().Count.Equals(2));
            Assert.IsTrue(repository.GetAllStates().Count.Equals(2));
        }

        //ClientTest

        [TestMethod]
        public void AddClients()
        {
            Client client = new Client(6, "John", "Watson");
            repository.AddClient(client);
            Assert.IsTrue(repository.GetClientById(6).Equals(client));
        }

        [TestMethod]
        public void RemoveClient()
        {   
            Client client1 = repository.GetClientById(1);
            repository.DeleteClient(client1);
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.GetClientById(1));
          
            Client client2 = new Client(3, "K", "M");
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.DeleteClient(client2));
        }


        [TestMethod]
        public void GetAllClientsIds()
        {
            List<int> idList = repository.GetAllClients().Select(c => c.Id).ToList();
            CollectionAssert.AreEqual(repository.GetAllClientsIds(), idList);
        }

        //ProductTest


        [TestMethod]
        public void AddProduct()
        {
            Product sofa = new Product(10, 25, Category.furniture);
            repository.AddProduct(sofa);
            Assert.IsTrue(repository.GetProductById(10).Equals(sofa));
        }


        [TestMethod] 
        public void RemoveProduct()
        {
            repository.DeleteProduct(2);
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.GetProductById(2));
        }
        [TestMethod]
        public void NoSuchProductId()
        {
            Assert.IsTrue(repository.NoSuchProductId(2147483647));
            Assert.IsFalse(repository.NoSuchProductId(1));
        }

        [TestMethod]
        public void GetAllProducts()
        {
            List<int> idListFromProducts = repository.GetAllProducts().Select(p => p.Id).ToList();
            List<int> idList = repository.GetAllProductIds().ToList();
            CollectionAssert.AreEqual(idListFromProducts, idList);
        }

        //EventTest


        [TestMethod]
        public void CheckClientPurchaseEvents()
        {   
            Client client = new Client(9, "Sherlock", "Holmes");
            Product product = new Product(15, 90, Category.books);
            State state = new State(product);
            EventPurchase eventPurchase = new EventPurchase(state, client);
            repository.AddEvent(eventPurchase);
            Assert.IsTrue(repository.GetAllEvents().Contains(eventPurchase));
            repository.DeleteEvent(eventPurchase);
            Assert.IsFalse(repository.GetAllEvents().Contains(eventPurchase));
        }

        //StateTest


        [TestMethod]
        public void CheckStates()
        {
            Product product = new Product(69, 420, Category.electronics);
            State state = new State(product);
            Assert.ThrowsException<Exception>(() => repository.DeleteState(state));
            Assert.IsTrue(repository.NoSuchState(state));
            repository.AddState(state);
            Assert.IsTrue(repository.GetAllStates().Contains(state));
            repository.DeleteState(state);
            Assert.IsFalse(repository.GetAllStates().Contains(state));
        }       
    }
}
