using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using WExporter.Entities.SampleConcrete;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        private ISampleRepository _sampleRepository;
        private IDapperSampleRepository _dapperSampleRepository;

        public HomeController(ISampleRepository sampleRepository, IDapperSampleRepository dapperSampleRepository)
        {
            _sampleRepository = sampleRepository;
            _dapperSampleRepository = dapperSampleRepository;
        }

        public IActionResult Index()
        {
            var list = _dapperSampleRepository.GetAllPaged(orderColumnName: "Name");
            var list2 = _dapperSampleRepository.GetAllPaged(1);
            var entity= _sampleRepository.GetFirst(x=>x.Id == 5);
            entity.Name = "BLABLA321123";
            _sampleRepository.Update(entity);
            return View();
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
