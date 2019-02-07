using System;
using System.Collections.Generic;
using System.Globalization;
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
        [HttpGet("todayItems")]
        public ActionResult<List<MenuItem>> GetTodayItems(string date)
        {                                              
            try
            {                                              //2011-10-05T14:48:00.000Z
                DateTime dtDate = DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm:ss.fffZ",System.Globalization.CultureInfo.InvariantCulture);            
                int day = (int)dtDate.DayOfWeek; 
                string menuCode = "19KW06"; //@@todo some logic to extract menu code and day

                if (!_context.IsValid())
                    throw new InvalidOperationException("The DB connection is not properly set.");

                var todayItems = _context.ZcMenuDetail.Where(i => i.Day == day && i.MenuheaderCode == menuCode)
                    .Select(i => new MenuItem {
                        MenuId = i.MenuId,
                        Description = i.Description,
                        SalesPrice = i.SalesPrice,
                        Picture = i.Picture
                    }).ToList();
                
                return todayItems;
            }
            catch (Exception e)
            {
                ContentResult err = Content(e.Message);
                err.StatusCode = 500;
                return err;
            }
        }

        // GET api/values
        [HttpGet("weekMenu")]
        public ActionResult<WeeklyMenu> GetWeekMenu(string date)
        {                                              
            try
            {                                             
                string menuCode = "19KW06"; //@@todo some logic to extract menu code from the date

                if (!_context.IsValid())
                    throw new InvalidOperationException("The DB connection is not properly set.");

                var weekMenu = _context.ZcMenuHeader.Where(h => h.MenuheaderCode == menuCode)
                    .Select(w => new WeeklyMenu {
                        Background = w.Background
                    }).Single();
                weekMenu.Days = new DailyMenu[7];
                for (int day = 0; day <= 6; day++)
                {
                    DailyMenu todayMenu = new DailyMenu();

                    var cultureInfo = new CultureInfo( "de-DE" );
                    var dateTimeInfo = cultureInfo.DateTimeFormat;
                    todayMenu.name = dateTimeInfo.GetDayName((DayOfWeek) day );

                    todayMenu.Items = _context.ZcMenuDetail.Where(i => i.Day == day && i.MenuheaderCode == menuCode)
                        .Select(i => new MenuItem {
                            MenuId = i.MenuId,
                            Description = i.Description,
                            SalesPrice = i.SalesPrice,
                            Picture = i.Picture
                        }).ToArray();
                    weekMenu.Days[day] = todayMenu;
                }
                
                return weekMenu;
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
