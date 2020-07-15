using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.AutoMapper;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Scenics.Dto;
using Egoal.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class GateAppService : ApplicationService, IGateAppService
    {
        private readonly IRepository<Gate> _gateRepository;

        public GateAppService(IRepository<Gate> gateRepository)
        {
            _gateRepository = gateRepository;
        }

        public async Task<RegistGateOutput> GetOrRegistGateAsync(RegistGateInput input, GateType gateType)
        {
            var gate = await _gateRepository.FirstOrDefaultAsync(g => g.IdentifyCode == input.IdentifyCode);
            if (gate == null)
            {
                var maxId = await _gateRepository.MaxAsync(_gateRepository.GetAll(), g => g.Id);

                gate = input.MapTo<Gate>();
                gate.Id = maxId + 1;
                gate.SortCode = gate.Id.ToString();
                gate.GateTypeId = gateType;
                gate.UseFlag = gateType == GateType.手机;
                gate.InOutFlag = true;
                await _gateRepository.InsertAndGetIdAsync(gate);
            }

            var output = new RegistGateOutput();
            output.Id = gate.Id;
            output.Name = gate.Name;
            output.UseFlag = gate.UseFlag == true;

            return output;
        }

        public async Task ChangeLocationAsync(ChangeGateLocationInput input)
        {
            var gate = await _gateRepository.FirstOrDefaultAsync(input.Id);
            if (gate == null)
            {
                throw new UserFriendlyException("通道不存在");
            }

            input.MapTo(gate);
        }

        public async Task<List<ComboboxItemDto<int>>> GetGateComboboxItemsAsync(int? gateGroupId)
        {
            var query = _gateRepository.GetAll()
                .WhereIf(gateGroupId.HasValue, g => g.GateGroupId == gateGroupId)
                .OrderBy(g => g.SortCode)
                .Select(g => new ComboboxItemDto<int>
                {
                    Value = g.Id,
                    DisplayText = g.Name
                });

            return await _gateRepository.ToListAsync(query);
        }
    }
}
