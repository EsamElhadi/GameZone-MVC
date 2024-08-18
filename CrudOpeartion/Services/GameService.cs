
using CrudOpeartion.Data;
using CrudOpeartion.Models;
using CrudOpeartion.Services;
using CrudOpeartion.Settings;
using CrudOpeartion.ViewModels;
using Microsoft.EntityFrameworkCore;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _imagePath;

    public GameService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
    }


    public IEnumerable<Game> GetAllGames() 
    { 
        var games = _context.Games
            .Include(g => g.Category)
            .Include(g => g.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .ToList();
        return games;
    }    
    
    public Game? GetGameById(int id) 
    { 
        var games = _context.Games
            .Include(g => g.Category)
            .Include(g => g.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .SingleOrDefault(g => g.Id == id);
        return games;
    }

    public async Task Create(CreateGameFormViewModel model)
    {
        var coverName = await SaveCover(model.Cover);

        Game game = new()
        {
            Name = model.Name,
            Description= model.Description,
            CategoryId= model.CategoryId,
            Cover = coverName,
            Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d}).ToList(),
        };
        _context.Games.Add(game);
        _context.SaveChanges();
    }

    public async Task<Game?> Update(EditGameFormViewModel model)
    {
        var game = _context.Games
            .Include(g => g.Devices)
            .SingleOrDefault(g => g.Id == model.Id);
        if (game == null)
        {
            return null;
        }
        var hasNewCover = model.Cover is not null;
        var oldCover = game.Cover;


        game.Name = model.Name;
        game.Description = model.Description;
        game.CategoryId = model.CategoryId;
        game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

        if (hasNewCover)
        {
            game.Cover = await SaveCover(model.Cover!);
        }

        var effectedRows = _context.SaveChanges();

        if (effectedRows > 0)
        {
            if (hasNewCover)
            {
                var cover = Path.Combine(_imagePath, oldCover);
                File.Delete(cover);
            }
            return game;
        }
        else
        {
                var cover = Path.Combine(_imagePath, game.Cover);
                File.Delete(cover);

            return null;
        }
    }

    public async Task<string> SaveCover(IFormFile cover)
    {
        var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

        var path = Path.Combine(_imagePath, coverName);

        using var stream = File.Create(path);
        await cover.CopyToAsync(stream);
    
        return coverName;
    }

    public bool Delete(int id)
    {
        var isDeleted = false;

        var game = _context.Games.Find(id);

        if (game is null)
            return isDeleted;

        _context.Remove(game);

        var effectedRows = _context.SaveChanges();

        if (effectedRows > 0)
        {
            isDeleted = true;

            var cover = Path.Combine(_imagePath, game.Cover);
            File.Delete(cover);
        }

        return isDeleted;
    }
}
