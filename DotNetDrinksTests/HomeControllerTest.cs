using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewASP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDrinksTests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexReturnsResult() 
        {
            //Arrange

            var controller = new HomeController(null);
            //Act
            var result = controller.Index();

            //Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PrivacyLoadsPrivacyView() 
        {
            //Arrange

            var controller = new HomeController(null);
            //Act
            //这里本来是返回一个actionresult（IActionResult），但是却return了一个view，所以我们这里转换一下。
            var result = (ViewResult)controller.Privacy();

            //Assert
            Assert.AreEqual("Privacy", result.ViewName);
        }


    }




}
