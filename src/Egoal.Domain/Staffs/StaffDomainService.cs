using Egoal.Authorization;
using Egoal.Cryptography;
using Egoal.Domain.Services;
using Egoal.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class StaffDomainService : DomainService, IStaffDomainService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IRoleRepository _roleRepository;

        public StaffDomainService(
            IStaffRepository staffRepository,
            IRoleRepository roleRepository)
        {
            _staffRepository = staffRepository;
            _roleRepository = roleRepository;
        }

        public async Task<int?> GetRoleIdAsync(int id)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync(id);
            if (staff == null || staff.HasLeft())
            {
                return null;
            }

            return await _staffRepository.GetRoleIdAsync(id);
        }

        public async Task<List<string>> GetPermissionsAsync(int id, SystemType? systemType)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync(id);
            if (staff == null || staff.HasLeft())
            {
                return new List<string>();
            }

            var roleId = await _staffRepository.GetRoleIdAsync(id);
            if (!roleId.HasValue)
            {
                throw new UserFriendlyException("员工未指定角色");
            }

            return await _roleRepository.GetPermissionsAsync(roleId.Value, systemType);
        }

        public async Task<Staff> LoginAsync(string uid, string password)
        {
            var staff = await _staffRepository.FirstOrDefaultAsync(s => s.Uid == uid);
            if (staff == null)
            {
                throw new UserFriendlyException("用户名无效");
            }

            if (staff.HasLeft())
            {
                throw new UserFriendlyException("已离职");
            }

            ValidatePassword(staff, password);

            return staff;
        }

        public void ValidatePassword(Staff staff, string password)
        {
            var pwd = SHAHelper.SHA512Encrypt(password, staff.Salt);
            var pwd1 = SHAHelper.SHA512Encrypt(pwd, staff.Id.ToString());
            if (staff.Pwd != pwd && staff.Pwd != pwd1)
            {
                throw new UserFriendlyException("密码不正确");
            }
        }
    }
}
