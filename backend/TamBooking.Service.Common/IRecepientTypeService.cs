using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IRecepientTypeService
    {
        public Task<List<RecepientType>> GetRecepientTypesAsync();
    }
}