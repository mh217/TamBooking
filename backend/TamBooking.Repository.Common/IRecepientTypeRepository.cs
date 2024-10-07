using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IRecepientTypeRepository
    {
        public Task<List<RecepientType>> GetRecepientTypesAsync();
    }
}