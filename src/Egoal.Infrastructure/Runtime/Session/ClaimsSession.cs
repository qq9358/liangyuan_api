using Egoal.Runtime.Security;
using System;
using System.Linq;

namespace Egoal.Runtime.Session
{
    public class ClaimsSession : ISession
    {
        public int? StaffId
        {
            get
            {
                var staffIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.StaffId);
                if (string.IsNullOrEmpty(staffIdClaim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(staffIdClaim.Value, out int _staffId))
                {
                    return null;
                }

                return _staffId;
            }
        }

        public int? RoleId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.RoleId);
                if (string.IsNullOrEmpty(claim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(claim.Value, out int roleId))
                {
                    return null;
                }

                return roleId;
            }
        }

        public int? SearchGroupId
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.SearchGroupId);
                if (string.IsNullOrEmpty(claim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(claim.Value, out int searchGroupId))
                {
                    return null;
                }

                return searchGroupId;
            }
        }

        public int? PcId
        {
            get
            {
                var pcIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.PcId);
                if (string.IsNullOrEmpty(pcIdClaim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(pcIdClaim.Value, out int _pcId))
                {
                    return null;
                }

                return _pcId;
            }
        }

        public int? SalePointId
        {
            get
            {
                var salePointIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.SalePointId);
                if (string.IsNullOrEmpty(salePointIdClaim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(salePointIdClaim.Value, out int _salePointId))
                {
                    return null;
                }

                return _salePointId;
            }
        }

        public int? ParkId
        {
            get
            {
                var parkIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.ParkId);
                if (string.IsNullOrEmpty(parkIdClaim?.Value))
                {
                    return null;
                }

                if (!int.TryParse(parkIdClaim.Value, out int _parkId))
                {
                    return null;
                }

                return _parkId;
            }
        }

        public Guid? MemberId
        {
            get
            {
                var memberIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.MemberId);
                if (string.IsNullOrEmpty(memberIdClaim?.Value))
                {
                    return null;
                }

                if (!Guid.TryParse(memberIdClaim.Value, out Guid _memberId))
                {
                    return null;
                }

                return _memberId;
            }
        }

        public Guid? CustomerId
        {
            get
            {
                var customerIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.CustomerId);
                if (string.IsNullOrEmpty(customerIdClaim?.Value))
                {
                    return null;
                }

                if (!Guid.TryParse(customerIdClaim.Value, out Guid _customerId))
                {
                    return null;
                }

                return _customerId;
            }
        }

        public Guid? GuiderId
        {
            get
            {
                var IdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == TmsClaimTypes.GuiderId);
                if (string.IsNullOrEmpty(IdClaim?.Value))
                {
                    return null;
                }

                if (!Guid.TryParse(IdClaim.Value, out Guid _id))
                {
                    return null;
                }

                return _id;
            }
        }

        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsSession(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }
    }
}
