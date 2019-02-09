using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DigitalSignage.Models;
using DigitalSignage.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly BurgerChainContext _context;
        private readonly TbServices _tbServices;

        public MenuController(
            BurgerChainContext context,
            TbServices tbServices
        )
        {
            _context = context;
            _tbServices = tbServices;
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

        [HttpGet("imageURL")]
        public ActionResult<object> GetImageURL(string nmspace)
        {     
            string root = "src"; //@@TODO production?
            try
            {
                if  (
                        System.IO.File.Exists(Path.Combine(root,"cache",nmspace)) ||
                        _tbServices.downloadImage(nmspace, Path.Combine(root,"cache"))
                    )
                    return new { url = "/cache/" + nmspace };
                else
                    return new { url = "/assets/no-image.jpg" };
            }
            catch (Exception e)
            {
                ContentResult err = Content(e.Message);
                err.StatusCode = 500;
                return err;
            }
        }

        [HttpGet("imageDownload")]
        public ActionResult<bool> ImageDownload(string nmspace)
        {   
            string root = "src"; //@@TODO production?
            try
            {
                _tbServices.downloadImage(nmspace, Path.Combine(root,"cache"));
            }
            catch (Exception e)
            {
                ContentResult err = Content(e.Message);
                err.StatusCode = 500;
                return err;
            }
            return true;  
        }

    }
}
