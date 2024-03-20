using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [TempData]
        public string StatusMessage { get; set; }


        [Route("admin/new")]
        public ActionResult Index()
        {
            return View();
        }
        #region Orders
        [Route("admin/orders")]
        public ActionResult Orders()
        {
            return View();
        }
        #endregion

        #region Flowers
        [Route("admin/flowers")]
        public async Task<ActionResult> Flowers()
        {
            return View(await _context.Flowers.ToListAsync());
        }
        [Route("admin/flowers/add")]
        public async Task<IActionResult> AddFlower()
        {
            Flower model = new Flower();
            List<SelectListItem> items = new List<SelectListItem>();
            List<FlowerCategory> categories = await _context.FlowerCategories.ToListAsync();
            foreach (var item in categories)
            {
                items.Add(new SelectListItem() { Text = item.Title, Value = item.Id});
            }
            model.Categories = items;
            return View(model);
        }
        [Route("admin/flowers/{id}/edit")]
        public async Task<IActionResult> EditFlower(string id)
        {

            return View();
        }
        [Route("admin/flowers/{id}/delete")]
        public async Task<IActionResult> DeleteFlower(string id)
        {
            Flower flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == id);
            if(flower is null)
            {
                StatusMessage = "Ошибка Цветы не найдены!";
                return RedirectToAction(nameof(Flowers));
            }

            _context.Flowers.Remove(flower);
            await _context.SaveChangesAsync();

            StatusMessage = "Цветы успешно удалены.";
            return RedirectToAction(nameof(Flowers));
        }
        #endregion

        #region Toys
        [Route("admin/toys")]
        public ActionResult Toys()
        {
            return View();
        }
        #endregion

        #region Candies
        [Route("admin/candies")]
        public ActionResult Candies()
        {
            return View();
        }
        #endregion]

        #region Other
        [Route("admin/other")]
        public ActionResult Other()
        {
            return View();
        }
        #endregion

        #region Category
        [Route("admin/categoryes")]
        public async Task<IActionResult> Categoryes()
        {
            return View(await _context.FlowerCategories.ToListAsync());
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
                if (previewPhoto is null)
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Выберите фото!");
                    return View("AddCategory", model);
                }
                if (await _context.FlowerCategories.AnyAsync(c => c.Title.Equals(model.Title)))
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Категория {model.Title} уже добавлена!");
                    StatusMessage = $"Ошибка Категория {model.Title} уже добавлена!";
                    return View("AddCategory", model);
                }
                string idFileGuid = Guid.NewGuid().ToString();

                string path = $"/resources/img/{idFileGuid}_{previewPhoto.FileName}";

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await previewPhoto.CopyToAsync(fileStream);
                }

                model.PreviewPhotoPath = path;

                await _context.FlowerCategories.AddAsync(model);
                await _context.SaveChangesAsync();
                StatusMessage = "Категория успешно добавлена!";

                return RedirectToAction(nameof(Categoryes));

            }
            catch
            {
                StatusMessage = "Ошибка Категория не добавлена!";
                return View("AddCategory", model);
            }
        }

        [Route("admin/category/{id}/change-status")]
        public async Task<IActionResult> ChangeStatusCategory(string id)
        {
            FlowerCategory category = await _context.FlowerCategories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                StatusMessage = "Ошибка Категория не найдена!";
                return RedirectToAction(nameof(Categoryes));
            }

            if (category.TypeStatus is TypeStatus.Не_опубликовано)
            {
                category.TypeStatus = TypeStatus.Опубликовано;
            }
            else
            {
                category.TypeStatus = TypeStatus.Не_опубликовано;
            }


            _context.FlowerCategories.Update(category);
            await _context.SaveChangesAsync();

            StatusMessage = "Категория успешно обновлена!";

            return RedirectToAction(nameof(Categoryes));
        }
        [Route("admin/category/{id}/edit")]
        public async Task<IActionResult> EditCategory(string id)
        {
            return View(await _context.FlowerCategories.FirstOrDefaultAsync(c => c.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> EditCategoryModel(FlowerCategory model, IFormFile previewPhoto)
        {
            try
            {
                FlowerCategory category = await _context.FlowerCategories.FirstOrDefaultAsync(c => c.Id == model.Id);
                if (await _context.FlowerCategories.AnyAsync(c => c.Title.Equals(model.Title) && c.Id != model.Id))
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Категория {model.Title} уже добавлена!");
                    StatusMessage = $"Ошибка Категория {model.Title} уже добавлена!";
                    return View("EditCategory", model);
                }
                category.Title = model.Title;
                category.TypeStatus = model.TypeStatus;
                category.TypeCategory = model.TypeCategory;

                if (previewPhoto != null)
                {
                    if (System.IO.File.Exists(_appEnvironment.WebRootPath + category.PreviewPhotoPath))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + category.PreviewPhotoPath);
                    }

                    string idFileGuid = Guid.NewGuid().ToString();
                    string path = $"/resources/img/{idFileGuid}_{previewPhoto.FileName}";

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await previewPhoto.CopyToAsync(fileStream);
                    }
                    category.PreviewPhotoPath = path;
                }


                _context.FlowerCategories.Update(category);
                await _context.SaveChangesAsync();
                StatusMessage = "Категория успешно обновлена!";

                return RedirectToAction(nameof(Categoryes));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Ошибка Категория не обновлена!");
                StatusMessage = "Ошибка Категория не обновлена!";
                return View("EditCategory", model);
            }
        }

        [Route("admin/category/{id}/delete")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            FlowerCategory category = await _context.FlowerCategories.Include(f => f.Flowers).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null || category.Flowers.Count != 0)
            {
                StatusMessage = "Ошибка при удалении категории!";
                return RedirectToAction(nameof(Categoryes));
            }
            _context.FlowerCategories.Remove(category);
            await _context.SaveChangesAsync();
            StatusMessage = "Категория успешно удалена!";

            return RedirectToAction(nameof(Categoryes));
        }
        #endregion

        #region Blog
        [Route("admin/blog")]
        public ActionResult Blog()
        {
            return View();
        }
        #endregion

        #region Reviews
        [Route("admin/reviews")]
        public ActionResult Reviews()
        {
            return View();
        }
        #endregion

        #region Users
        [Route("admin/users")]
        public ActionResult Users()
        {
            return View();
        }
        #endregion

        #region Messages
        [Route("admin/messages")]
        public ActionResult Messages()
        {
            return View();
        }
        #endregion

    }
}
