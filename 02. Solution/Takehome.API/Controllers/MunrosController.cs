using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Takehome.API.Models;
using Takehome.API.Services;
using Takehome.API.VirtualModels;

namespace Takehome.API.Controllers
{
    [ApiController]
    [Route("api/munros")]
    public class MunrosController : ControllerBase
    {
        private readonly IMunroLibraryRepository _munroLibraryRepository;

        public MunrosController(IMunroLibraryRepository munroLibraryRepository)
        {
            _munroLibraryRepository = munroLibraryRepository ??
                throw new ArgumentNullException(nameof(munroLibraryRepository));
        }

        [HttpGet()]
        public IActionResult GetMunros()
        {
            IEnumerable<Munro> munrosFromRepo = _munroLibraryRepository.GetMunros();
            IEnumerable<MunroVM> retVal = munrosFromRepo
                .Where(m => m.Hill_Category != Hill_Categories.None)
                .Select(m => new MunroVM()
            {
                Name = m.pName,
                Height = m.pHeight__m_,
                Hill_Category = m.pPost_1997,
                Grid_Reference = m.pGrid_Ref
            });

            return new JsonResult(retVal);
        }
    }
}