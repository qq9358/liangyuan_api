using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Authorization;
using Egoal.Extensions;
using Egoal.Runtime.Security;
using Egoal.Runtime.Session;
using Egoal.Scenics;
using Egoal.Staffs.Dto;
using Egoal.UI;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class StaffAppService : ApplicationService, IStaffAppService
    {
        private readonly ISignInAppService _signInAppService;
        private readonly IStaffDomainService _staffDomainService;
        private readonly IStaffRepository _staffRepository;
        private readonly IPcRepository _pcRepository;
        private readonly ISession _session;

        public StaffAppService(
            ISignInAppService signInAppService,
            IStaffDomainService staffDomainService,
            IStaffRepository staffRepository,
            IPcRepository pcRepository,
            ISession session)
        {
            _signInAppService = signInAppService;
            _staffDomainService = staffDomainService;
            _staffRepository = staffRepository;
            _pcRepository = pcRepository;
            _session = session;
        }

        public async Task<LoginOutput> LoginAsync(LoginInput input, SystemType systemType)
        {
            if (input.PcId.HasValue)
            {
                var pc = await _pcRepository.FirstOrDefaultAsync(input.PcId.Value);
                if (pc == null)
                {
                    throw new UserFriendlyException("主机未注册");
                }

                if (pc.PermitFlag != true)
                {
                    throw new UserFriendlyException("主机未授权");
                }
            }
            else
            {
                if (systemType == SystemType.安卓手持机系统)
                {
                    throw new UserFriendlyException("PcId不能为空");
                }
            }

            var staff = await _staffDomainService.LoginAsync(input.UserName, input.Password);

            var output = new LoginOutput();

            output.Staff = new StaffDto
            {
                Id = staff.Id,
                Name = staff.Name,
                ParkId = staff.ParkId
            };

            var claims = new List<Claim>();
            claims.Add(new Claim(TmsClaimTypes.StaffId, staff.Id.ToString()));
            var roleId = await _staffDomainService.GetRoleIdAsync(staff.Id);
            if (roleId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.RoleId, roleId.Value.ToString()));
            }
            if (staff.TicketTypeSearchGroupId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.SearchGroupId, staff.TicketTypeSearchGroupId.Value.ToString()));
            }
            if (staff.ParkId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.ParkId, staff.ParkId.Value.ToString()));
            }
            if (input.PcId.HasValue)
            {
                claims.Add(new Claim(TmsClaimTypes.PcId, input.PcId.Value.ToString()));
            }
            output.Token = _signInAppService.CreateToken(claims);

            output.Permissions = await _staffDomainService.GetPermissionsAsync(staff.Id, systemType);

            return output;
        }

        public async Task EditPasswordAsync(EditPasswordInput input)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync(s => s.Id == _session.StaffId);

            _staffDomainService.ValidatePassword(staff, input.OldPassword);

            staff.EditPassword(input.Password);
        }

        public async Task<List<ComboboxItemDto<int>>> GetCashierComboboxItemsAsync()
        {
            var cashiers = await _staffRepository.GetCashierComboboxItemsAsync();

            var adminId = 1;
            if (!cashiers.Any(c => c.Value == adminId))
            {
                var admin = await _staffRepository.FirstOrDefaultAsync(adminId);
                cashiers.Add(new ComboboxItemDto<int> { Value = adminId, DisplayText = admin.Name });
            }

            var defaultStaffs = typeof(DefaultStaff).ToComboboxItems();

            return cashiers.Union(defaultStaffs, new ComboboxItemDtoComparer<int>()).ToList();
        }

        public async Task<List<ComboboxItemDto<int>>> GetSalesManComboboxItemsAsync()
        {
            return await _staffRepository.GetSalesManComboboxItemsAsync();
        }
    }
}
