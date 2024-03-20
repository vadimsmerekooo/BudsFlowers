using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BudsFlowers.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly BudsContext _context;
        private IWebHostEnvironment _appEnvironment;
        public AdminController(BudsContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        [Route("admin/new")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("admin/orders")]
        public ActionResult Orders()
        {
            return View();
        }
        [Route("admin/flowers")]
        public ActionResult Flowers()
        {
            return View();
        }
        [Route("admin/toys")]
        public ActionResult Toys()
        {
            return View();
        }
        [Route("admin/candies")]
        public ActionResult Candies()
        {
            return View();
        }
        [Route("admin/other")]
        public ActionResult Other()
        {
            return View();
        }
        [Route("admin/categoryes")]
        public ActionResult Categoryes()
        {
            return View(_context.FlowerCategories.ToList());
        }
        [Route("admin/add-category")]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddCategoryModel(FlowerCategory model, IFormFile previewPhoto)
        {
            try
            {

                string idFileGuid = Guid.NewGuid().ToString();

                string path = $"/resources/img/{idFileGuid}_{previewPhoto.FileName}";

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await previewPhoto.CopyToAsync(fileStream);
                }

                model.PreviewPhotoPath = idFileGuid;

                await _context.FlowerCategories.AddAsync(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Categoryes));

            }
            catch
            {
                return View("AddCategory", model);
            }
        }
        [Route("admin/blog")]
        public ActionResult Blog()
        {
            return View();
        }
        [Route("admin/reviews")]
        public ActionResult Reviews()
        {
            return View();
        }
        [Route("admin/users")]
        public ActionResult Users()
        {
            return View();
        }
        [Route("admin/messages")]
        public ActionResult Messages()
        {
            return View();
        }

    }
}
