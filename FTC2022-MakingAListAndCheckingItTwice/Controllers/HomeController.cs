using FTC2022_MakingAListAndCheckingItTwice.Data;
using FTC2022_MakingAListAndCheckingItTwice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FTC2022_MakingAListAndCheckingItTwice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersRolesService _usersRolesService;

        public HomeController(ILogger<HomeController> logger
                                , IUsersRolesService usersRolesService)
        {
            _logger = logger;
            _usersRolesService = usersRolesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> EnsureUsersAndRoles()
        {
            await _usersRolesService.EnsureUsersAndRoles();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}