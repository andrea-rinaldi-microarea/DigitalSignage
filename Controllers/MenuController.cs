using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignage.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly BurgerChainContext _context;

        public MenuController(BurgerChainContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet("headers")]
        public ActionResult<List<ZcMenuHeader>> GetHeaders()
        {
            try
            {
                if (!_context.IsValid())
                    throw new InvalidOperationException("The DB connection is not properly set.");

                return _context.ZcMenuHeader.ToList();
            }
            catch (Exception e)
            {
                ContentResult err = Content(e.Message);
                err.StatusCode = 500;
                return err;
            }
        }
    }
}
