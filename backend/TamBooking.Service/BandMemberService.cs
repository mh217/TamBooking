using Microsoft.AspNetCore.Http;
using TamBooking.Model;
using TamBooking.Repository;
using TamBooking.Repository.Common;
using TamBooking.Service.Common;

namespace TamBooking.Service
{
    public class BandMemberService : IBandMemberService
    {
        private readonly IBandMemberRepository _memberRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BandMemberService(IBandMemberRepository memberRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _memberRepository = memberRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateBandMemeberAsync(BandMember member)
        {
            var bandId = Guid.Parse(_jwtService.GetCurrentUserClaims().Id);
            member.BandId = bandId;
            var currentUserId = bandId;
            member.CreatedByUserId = currentUserId;
            member.UpdatedByUserId = currentUserId;
            await _memberRepository.CreateBandMemberAsync(member);
        }

        public async Task<List<BandMember>> GetBandMemberAsync(Guid bandId)
        {
            return await _memberRepository.GetBandMemberAsync(bandId);
        }

        public async Task<bool> DeleteBandMemberAsync(Guid id)
        {
            var isBandMemberDeleted = await _memberRepository.DeleteBandMemberAsync(id);
            return isBandMemberDeleted;
        }
    }
}