using Egoal.Authorization;
using Egoal.Domain.Uow;
using Egoal.Runtime.Session;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Egoal.Mvc.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement, string[]>
    {
        private readonly ISession _session;
        private readonly IPermissionChecker _permissionChecker;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PermissionHandler(
            ISession session,
            IPermissionChecker permissionChecker,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _session = session;
            _permissionChecker = permissionChecker;
            _unitOfWorkManager = unitOfWorkManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, string[] permissions)
        {
            var roleId = _session.RoleId;
            if (!roleId.HasValue)
            {
                return;
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                foreach (var permission in permissions)
                {
                    if (await _permissionChecker.IsGrantedAsync(roleId.Value, permission))
                    {
                        context.Succeed(requirement);

                        break;
                    }
                }

                await uow.CompleteAsync();
            }
        }
    }
}
