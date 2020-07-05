using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.WebUI.Tests.Mocks;
using MyShop.Services;
using System.Linq;
using MyShop.WebUI.Controllers;
using System.Web.UI;
using System.Web.Mvc;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //create mock repositories
            //set up

            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();

            IRepository<Order> orders = new MockContext<Order>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService,orderService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
           // basketService.AddtoBasket(httpContext,"1");
           
            //ACT
            controller.AddToBasket("1");
            Basket basket = baskets.Collection().FirstOrDefault();


            //assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count());
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }
        [TestMethod]
        public void CanGetSummaryModel()
        {
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();


            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });
            products.Insert(new Product() { Id = "3", Price = 15.00m });
            products.Insert(new Product() { Id = "4", Price = 20.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "3", Quantity = 3 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "4", Quantity = 4 });
            baskets.Insert(basket);


            IBasketService basketService = new BasketService(products, baskets);
           
            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService,orderService);
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);


            var results = controller.BasketSummary() as PartialViewResult;

            var basketSummary = (BasketSumaryViewModel)results.ViewData.Model;

            Assert.AreEqual(10, basketSummary.BasketCount);
            Assert.AreEqual(150.00m, basketSummary.BasketTotal);
        }
        [TestMethod]
        public void CanCheckOutandCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { Id = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { Id = "2", Quantity = 1, BasketId = basket.Id });
            baskets.Insert(basket);
            IBasketService basketService = new BasketService(products,baskets);
            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService);

            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);


            //act
            Order order = new Order();
            controller.Checkout(order);

            //Assert

            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);
            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(0, orderInRep.OrderItems.Count);


        }
    }
}
