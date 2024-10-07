using TamBooking.Model;

namespace TamBooking.Service.Common
{
    public interface IBandMemberService
    {
        public Task CreateBandMemeberAsync(BandMember member);

        public Task<List<BandMember>> GetBandMemberAsync(Guid id);

        public Task<bool> DeleteBandMemberAsync(Guid id);
    }
}