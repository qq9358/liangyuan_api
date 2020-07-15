using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.AutoMapper;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Scenics.Dto;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class PcAppService : ApplicationService, IPcAppService
    {
        private readonly IPcRepository _pcRepository;
        private readonly IRepository<SalePoint> _salePointRepository;

        public PcAppService(
            IPcRepository pcRepository,
            IRepository<SalePoint> salePointRepository)
        {
            _pcRepository = pcRepository;
            _salePointRepository = salePointRepository;
        }

        public async Task<RegistPcOutput> RegistHandsetAsync(RegistPcInput input)
        {
            var pc = await _pcRepository.FirstOrDefaultAsync(p => p.IdentifyCode == input.IdentifyCode);
            if (pc == null)
            {
                pc = input.MapTo<Pc>();

                await _pcRepository.InsertAndGetIdAsync(pc);
            }

            var output = new RegistPcOutput();
            output.Id = pc.Id;
            output.Name = pc.Name;
            output.PermitFlag = pc.PermitFlag == true;

            return output;
        }

        public async Task<ChangePcLocationOutput> ChangeLocationAsync(ChangePcLocationInput input)
        {
            var pc = await _pcRepository.FirstOrDefaultAsync(input.Id);
            if (pc == null)
            {
                throw new UserFriendlyException("主机不存在");
            }

            input.MapTo(pc);

            var parkId = await _salePointRepository.GetAll()
                .Where(s => s.Id == input.SalePointId)
                .Select(s => s.ParkId)
                .FirstOrDefaultAsync();

            var output = new ChangePcLocationOutput();
            output.PcId = pc.Id;
            output.SalePointId = pc.SalePointId;
            output.ParkId = parkId;

            return output;
        }

        public async Task<List<ComboboxItemDto<int>>> GetCashPcComboboxItemsAsync()
        {
            var pcs = await _pcRepository.GetCashPcComboboxItemsAsync();

            var defaultPcs = typeof(DefaultPc).ToComboboxItems();

            return pcs.Union(defaultPcs, new ComboboxItemDtoComparer<int>()).ToList();
        }
    }
}
