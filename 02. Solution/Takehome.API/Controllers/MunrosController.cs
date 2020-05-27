using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public IActionResult GetMunros([FromQuery] GetMunrosInputVM gmivm)
        {
            try
            {
                //10. Fetch all rows
                IEnumerable<Munro> munrosFromRepo = _munroLibraryRepository.GetMunros();
                IEnumerable<MunroVM> retVal = munrosFromRepo
                    //20. Ignore all rows where Hill_Category is not set
                    .Where(m => m.Hill_Category != Hill_Categories.None)
                    //30. Filter starts here
                    //31. Filter by Hill_Category (BITWISE &)
                    .Where(m => (gmivm.filterBy & m.Hill_Category) == m.Hill_Category)
                    //32. Filter by minHeight
                    .Where(m => gmivm.minHeight == 0 || m.pHeight__m_ >= gmivm.minHeight)
                    //33. Filter by maxHeight
                    .Where(m => gmivm.maxHeight == 0 || m.pHeight__m_ <= gmivm.maxHeight)
                    //40. Select only the columns we're interested in
                    .Select(m => new MunroVM()
                    {
                        name = m.pName,
                        height = m.pHeight__m_,
                        hill_category = m.pPost_1997,
                        grid_reference = m.pGrid_Ref
                    });

                //50. Dynamic Order By
                bool isFirst = true;
                foreach (SortColumnVM scvm in gmivm.SortColumns)
                {
                    if (scvm.DirectionReverse)
                    {
                        if (isFirst)
                            retVal = retVal.OrderByDescending(x => GetColumnValue(x, scvm.ColumnName));
                        else
                            retVal = ((IOrderedEnumerable<MunroVM>)retVal).ThenByDescending(x => GetColumnValue(x, scvm.ColumnName));
                    }
                    else
                    {
                        if (isFirst)
                            retVal = retVal.OrderBy(x => GetColumnValue(x, scvm.ColumnName));
                        else
                            retVal = ((IOrderedEnumerable<MunroVM>)retVal).ThenBy(x => GetColumnValue(x, scvm.ColumnName));
                    }
                    isFirst = false;
                }

                if (retVal.Count() == 0)
                {
                    //60. http 404 ERROR if no rows found
                    return NotFound("No data");
                }
                else if (gmivm.rowsCount > 0)
                {
                    //70. http 200 OK with the requested number of rows
                    return Ok(retVal.Take(gmivm.rowsCount));
                }
                else
                {
                    //80. http 200 OK with the all rows
                    return Ok(retVal);
                }
            }
            catch (FileNotFoundException ex)
            {
                //90. Not able to locate the CSV file
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //99. Unhandled errors
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private object GetColumnValue(object inputObject, string propertyName)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //get the named property value from the object
            return propertyInfo.GetValue(inputObject);
        }
    }
}