using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Egoal.DynamicCodes
{
    public class DynamicCodeService : IDynamicCodeService
    {
        private readonly static SemaphoreSlim listNoLocker = new SemaphoreSlim(1);
        private readonly static SemaphoreSlim ticketCodeLocker = new SemaphoreSlim(1);

        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DynamicCodeService(
            IServiceProvider serviceProvider,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _serviceProvider = serviceProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<string> GenerateListNoAsync(string listNoType)
        {
            try
            {
                await listNoLocker.WaitAsync();

                return await GenerateListNoAsync(listNoType, ListNo.DefaultListNoLength);
            }
            finally
            {
                listNoLocker.Release();
            }
        }

        public async Task<string> GenerateWxTicketCodeAsync()
        {
            string prefix = "WX";

            return await GenerateTicketCodeAsync(prefix);
        }

        public async Task<string> GenerateParkTicketCodeAsync(int parkId)
        {
            string prefix = GenerateParkPrefix(parkId);

            return await GenerateTicketCodeAsync(prefix);
        }

        private string GenerateParkPrefix(int parkId)
        {
            StringBuilder prefix = new StringBuilder("J");
            if (parkId > 0 && parkId < 10)
            {
                prefix.Append(parkId);
            }
            else if (parkId >= 10 && parkId <= 35)
            {
                prefix.Append(Convert.ToChar(parkId + ('A' - 10)));
            }
            else
            {
                prefix.Append("0");
            }
            return prefix.ToString();
        }

        public async Task<string> GenerateTicketCodeAsync(string prefix)
        {
            try
            {
                await ticketCodeLocker.WaitAsync();

                var code = await GenerateListNoAsync(prefix, ListNo.DefaultTicketCodeLength);

                return $"{code}{RandomHelper.CreateRandomNumber(3)}";
            }
            finally
            {
                ticketCodeLocker.Release();
            }
        }

        private async Task<string> GenerateListNoAsync(string prefix, int length)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var listNoRepository = _serviceProvider.GetRequiredService<IRepository<ListNo, long>>();

                string date = DateTime.Now.Date.ToString("yyMMdd");

                var listNo = await listNoRepository.FirstOrDefaultAsync(o => o.ListNoTypeID == prefix && o.ListDate == date);
                if (listNo == null)
                {
                    listNo = new ListNo();
                    listNo.Id = 1;
                    listNo.ListNoTypeID = prefix;
                    listNo.ListDate = date;
                    listNo.Lens = length;

                    await listNoRepository.InsertAsync(listNo);
                }
                else
                {
                    listNo.Id++;
                }

                string id = listNo.Id.ToString();
                if (id.Length < listNo.Lens)
                {
                    id = id.PadLeft(listNo.Lens, '0');
                }
                else
                {
                    id = id.Substring(id.Length - listNo.Lens, listNo.Lens);
                }

                await uow.CompleteAsync();

                return $"{prefix}{date}{id}";
            }
        }
    }
}
