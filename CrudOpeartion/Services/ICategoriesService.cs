using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudOpeartion.Services
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
