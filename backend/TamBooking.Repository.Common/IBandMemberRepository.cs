using TamBooking.Model;

namespace TamBooking.Repository.Common
{
    public interface IBandMemberRepository
    {
        public Task CreateBandMemberAsync(BandMember member);

        public Task<List<BandMember>> GetBandMemberAsync(Guid id);

        public Task<bool> DeleteBandMemberAsync(Guid id);
    }
}