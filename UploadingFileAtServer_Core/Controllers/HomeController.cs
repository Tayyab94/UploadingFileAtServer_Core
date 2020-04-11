using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UploadingFileAtServer_Core.Models;

namespace UploadingFileAtServer_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostEnvironment _environment;
        private readonly string _dir;
        public HomeController(ILogger<HomeController> logger, IHostEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
            _dir = _environment.ContentRootPath;
        }

        public IActionResult UploadFile_JaveScript()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public  IActionResult SingleFileUpoad(IFormFile file)
        {
      
            using(var fileStreem=new FileStream(Path.Combine(_dir,file.FileName),FileMode.Create, FileAccess.Write))
            {

                file.CopyTo(fileStreem);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult MultiPleFilesUpload(IEnumerable<IFormFile> files)
        {
            int i = 0;
            foreach (var item in files)
            {
                using (var fileStreem = new FileStream(Path.Combine(_dir,$"Files_{i}.{item.ContentType}"), FileMode.Create, FileAccess.Write))
                {
                    item.CopyTo(fileStreem);
                }
         
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
