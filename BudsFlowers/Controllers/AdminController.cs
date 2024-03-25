using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BudsFlowers.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly BudsContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _appEnvironment;
        public AdminController(BudsContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
        public async Task<ActionResult> Flowers(TypeCategory type)
        {
            FlowersCategoryViewModel model = new FlowersCategoryViewModel()
            {
                Flowers = await _context.Flowers.Include(c => c.Category).Where(t => t.TypeCategory == type).OrderBy(a => a.Article).ToListAsync(),
                TypeCategory = type
            };
            return View(model);
        }
        [Route("admin/flowers/add")]
        public async Task<IActionResult> AddFlower(TypeCategory type)
        {
            List<FlowerCategory> categories = await _context.FlowerCategories.Where(t => t.TypeCategory == type).ToListAsync();
            long lastArticle = _context.Flowers.OrderByDescending(a => a.Article).First().Article;
            ViewBag.Categories = new SelectList(categories, "Id", "Title");            
            return View(new Flower() { TypeCategory = type, Categories = categories, Article = lastArticle + 1 });
        }
        [Route("admin/flowers/add/model")]
        [HttpPost]
        public async Task<IActionResult> AddFlowerModel(Flower model, IFormFile previewPhoto)
        {
            try
            {
                List<FlowerCategory> categories = await _context.FlowerCategories.Where(t => t.TypeCategory == model.TypeCategory).ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Title");
                if (previewPhoto is null)
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Выберите фото!");
                    return View("AddFlower", model);
                }
                if (await _context.Flowers.AnyAsync(c => c.Title.Equals(model.Title)))
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка {model.TypeCategory} {model.Title} уже добавлены!");
                    StatusMessage = $"Ошибка {model.TypeCategory} {model.Title} уже добавлены!";
                    return View("AddFlower", model);
                }
                if (await _context.Flowers.AnyAsync(f => f.Article.Equals(model.Article)))
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Артикул уже использован!");
                    StatusMessage = $"Ошибка Артикул уже использован!";
                    return View("AddFlower", model);
                }
                FlowerCategory category = await _context.FlowerCategories.FirstOrDefaultAsync(c => c.Id == model.CategorySelectId);
                if (category == null)
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка Выбранная категория не найдена!");
                    StatusMessage = $"Ошибка Выбранная категория не найдена!";
                    return View("AddFlower", model);
                }
                string idFileGuid = Guid.NewGuid().ToString();

                string path = $"/resources/img/flowers/{idFileGuid}_{previewPhoto.FileName}";

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await previewPhoto.CopyToAsync(fileStream);
                }
                model.PhotoPath = path;
                model.Category = category;
                model.Categories = categories;

                await _context.Flowers.AddAsync(model);
                await _context.SaveChangesAsync();
                StatusMessage = $"{model.TypeCategory} {model.Title} успешно добавлены!";


                return RedirectToAction(nameof(Flowers), new { type = model.TypeCategory } );
            }
            catch
            {
                StatusMessage = $"Ошибка {model.TypeCategory} не добавлены!";
                return View("AddFlowers", model);
            }
        }

        [Route("admin/flowers/{id}/edit")]
        public async Task<IActionResult> EditFlower(string id)
        {
            Flower flower = await _context.Flowers.Include(c => c.Category).FirstOrDefaultAsync(f => f.Id == id);
            if (flower is null)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка {flower.TypeCategory} не найдены!");
                StatusMessage = $"Ошибка {flower.TypeCategory} не найдены!";
                return RedirectToAction("Flowers");
            }
            flower.CategorySelectId = flower.Category.Id;
            ViewBag.Categories = new SelectList(await _context.FlowerCategories.ToListAsync(), "Id", "Title");
            return View(flower);
        }
        [HttpPost]
        public async Task<IActionResult> EditFlowerModel(Flower model, IFormFile previewPhoto)
        {
            try
            {
                Flower flower = await _context.Flowers.FirstOrDefaultAsync(c => c.Id == model.Id);
                FlowerCategory category = await _context.FlowerCategories.FirstOrDefaultAsync(c => c.Id == model.CategorySelectId);
                if (await _context.Flowers.AnyAsync(c => c.Title.Equals(model.Title) && c.Id != model.Id))
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка  {model.TypeCategory}  {model.Title} уже добавлены!");
                    StatusMessage = $"Ошибка  {model.TypeCategory}  {model.Title} уже добавлены!";
                    return View("EditFlower", model);
                }
                if (category != null)
                {
                    flower.Category = category;
                }
                flower.Title = model.Title;
                flower.Article = model.Article;
                flower.Price = model.Price;
                flower.Sale = model.Sale;
                flower.Compound = model.Compound;
                flower.IsPopular = model.IsPopular;
                flower.IsInStock = model.IsInStock;
                flower.Status = model.Status;


                if (previewPhoto != null)
                {
                    if (System.IO.File.Exists(_appEnvironment.WebRootPath + flower.PhotoPath))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + flower.PhotoPath);
                    }

                    string idFileGuid = Guid.NewGuid().ToString();
                    string path = $"/resources/img/flowers/{idFileGuid}_{previewPhoto.FileName}";

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await previewPhoto.CopyToAsync(fileStream);
                    }
                    flower.PhotoPath = path;
                }


                _context.Flowers.Update(flower);
                await _context.SaveChangesAsync();
                StatusMessage = $"{flower.TypeCategory} {flower.Title} успешно обновлены!";

                return RedirectToAction(nameof(Flowers), new { type = model.TypeCategory });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Ошибка Позиция не обновлена!");
                StatusMessage = $"Ошибка Позиция не обновлена!";
                return View("EditFlower", model);
            }
        }

        [Route("admin/flower/{id}/change-status")]
        public async Task<IActionResult> ChangeStatusFlower(string id)
        {
            Flower flower = await _context.Flowers.FirstOrDefaultAsync(c => c.Id == id);
            if (flower == null)
            {
                StatusMessage = $"Ошибка Позиция не найдены!";
                return RedirectToAction(nameof(Flowers));
            }

            if (flower.Status is TypeStatus.Не_опубликовано)
            {
                flower.Status = TypeStatus.Опубликовано;
            }
            else
            {
                flower.Status = TypeStatus.Не_опубликовано;
            }


            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();

            StatusMessage = $"{flower.TypeCategory} {flower.Title}  успешно обновлены!";

            return RedirectToAction(nameof(Flowers), new { type = flower.TypeCategory });
        }
        [Route("admin/flowers/{id}/delete")]
        public async Task<IActionResult> DeleteFlower(string id)
        {
            Flower flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == id);
            if (flower is null)
            {
                StatusMessage = "Ошибка Позиция не найдена!";
                return RedirectToAction(nameof(Flowers));
            }

            _context.Flowers.Remove(flower);
            await _context.SaveChangesAsync();

            StatusMessage = $"{flower.TypeCategory} успешно удалены.";
            return RedirectToAction(nameof(Flowers), new { type = flower.TypeCategory });
        }
        #endregion


        #region Category
        [Route("admin/categoryes")]
        public async Task<IActionResult> Categoryes()
        {
            return View(await _context.FlowerCategories.Include(f => f.Flowers).ToListAsync());
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

                string path = $"/resources/img/categoryes/{idFileGuid}_{previewPhoto.FileName}";

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
                    string path = $"/resources/img/categoryes/{idFileGuid}_{previewPhoto.FileName}";

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
        public async Task<IActionResult> Users()
        {
            return View(await _userManager.Users.ToListAsync());
        }


        [Route("admin/user/{userId}/edit")]
        public async Task<IActionResult> UserEdit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.ToListAsync();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UserEditModel(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("Users");
            }

            return NotFound();
        }
        #endregion


        #region Roles
        [Route("admin/roles")]
        public async Task<IActionResult> Roles()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }
        [Route("admin/roles/create")]
        public IActionResult RoleCreate() => View();
        [HttpPost]
        public async Task<IActionResult> RoleCreateModel(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("RoleCreate", name);
        }
        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Roles");
        }
        #endregion

        #region Messages
        [Route("admin/messages")]
        public async Task<IActionResult> Messages()
        {

            return View(await _context.Messages.ToListAsync());
        }
        #endregion

    }
}
