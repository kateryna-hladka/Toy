using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Toy.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Toy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (ToyContext toyContext = new())
            {
                List<Category> categories = [.. toyContext.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => c)];
/*
                var photo_brands = toyContext.Photos
                    .Join(inner: toyContext.PhotoBrands,
                    photo => photo.Id,
                    photoBrand => photoBrand.PhotoId,
                    (photo, photoBrand) => new { photo, photoBrand })
                    .Join(inner: toyContext.Brands,
                    combined => combined.photoBrand.BrandId,
                    brand => brand.Id,
                    ((combined, brand) => new Tuple<Photo, Brand>(combined.photo, brand))
                    )
                    .ToList();
                Console.WriteLine(photo_brands.GetType());*/
                /*
                select photo_url, is_Main, name from Photo
                Join Photo_Brand ON photo_id = Photo.id
                Join Brand ON Brand.id = Photo_Brand.id
                */

                Dictionary<string, object> result = new();
                result.Add("Category", categories);
                /*result.Add("Photo_Brand", photo_brands);*/

                return View(result);
            }
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
