using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Configuration;
using testNetJsonConfig.Models;

namespace testNetJsonConfig.Controllers
{
    public class HomeController : Controller
    {
        private readonly RedisConfig _redisConfig;
        private readonly IConfiguration _configuration;

        public HomeController(RedisConfig redisConfig, IConfiguration configuration)
        {
            _redisConfig = redisConfig;
            _configuration = configuration;
        }
        
        public ActionResult Index()
        {
            return Json(new
            {
                Redis = _redisConfig,
                RedisIp = _configuration["Redis:Ip"]                
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}