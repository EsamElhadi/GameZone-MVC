using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudOpeartion.Services
{
    public interface IDevicesService
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
