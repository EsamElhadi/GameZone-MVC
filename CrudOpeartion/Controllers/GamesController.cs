using CrudOpeartion.Data;
using CrudOpeartion.Models;
using CrudOpeartion.Services;
using CrudOpeartion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudOpeartion.Controllers
{
    
    public class GamesController : Controller
    {
        private readonly IDevicesService _devicesService;
        private readonly ICategoriesService _categoriesService;
        private readonly IGameService _gamesService;

        public GamesController(IDevicesService devicesService, ICategoriesService categoriesService, IGameService gamesService)
        {
            _devicesService = devicesService;
            _categoriesService = categoriesService;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAllGames();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetGameById(id);
            if (game is null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),

                Devices = _devicesService.GetSelectList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            await _gamesService.Create(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] 
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetGameById(id);
            if (game is null)
            {
                return NotFound();
            }

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CurrentCover = game.Cover,

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            var game = await _gamesService.Update(model);
            if (game is null) 
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
