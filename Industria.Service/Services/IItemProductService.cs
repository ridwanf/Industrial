using System.Collections.Generic;
using System.Threading.Tasks;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Service.Services
{
    public interface IItemProductService
    {
        Task<List<ItemProductModel>> GetAllAsync();

        List<ItemProductModel> GetAll();
        Task<ItemProductModel> FindByIdAsync(int id);
        Task<List<ItemProductModel>> GetAllAsync(int skip, int pageSize);
        List<ItemProductModel> GetAll(int page, int pageSize);
        Task<ItemProductModel> CreateAsync(ItemProductModel item);

        Task<ItemProductModel> EditAsync(ItemProductModel item);

        Task<ItemProductModel> DeleteAsync(int id);
        IEnumerable<ItemProductModel> Search(string searchWord, int i, int pageSize, out int total);
    }
}