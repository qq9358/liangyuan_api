using Egoal.Common;
using Egoal.Customers;
using Egoal.Domain.Repositories;
using Egoal.Members;
using Egoal.Payment;
using Egoal.Scenics;
using Egoal.Stadiums;
using Egoal.Staffs;
using Egoal.TicketTypes;
using Egoal.Trades;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Egoal.Caches
{
    public class NameCacheService : INameCacheService
    {
        private readonly TimeSpan _slidingExpiration = TimeSpan.FromMinutes(10);

        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;

        public NameCacheService(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
        }

        public string GetMemberName(Guid? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"MemberName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<Member, Guid>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetCustomerName(Guid? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"CustomerName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<Member, Guid>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetStaffName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"StaffName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultStaff.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<IStaffRepository>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetExplainerTimeslotName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"ExplainerTimeslotName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<ExplainerTimeslot>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetAgeRangeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"AgeRangeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<AgeRange>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetKeYuanTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"KeYuanTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<KeYuanType>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetAreaName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"AreaName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<Area>>();

                var area = repository.FirstOrDefault(id.Value);
                var areaName = area.Name;
                while (area.Pid.HasValue && area.Pid != 0)
                {
                    area = repository.FirstOrDefault(area.Pid.Value);
                    areaName = $"{area.Name}{areaName}";
                }

                return areaName;
            });
        }

        public string GetGateName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"GateName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<Gate>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetGateGroupName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"GateGroupName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<GateGroup>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetGroundName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"GroundName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<Ground>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetParkName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"ParkName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultPark.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<IRepository<Park>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetSalePointName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"SalePointName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultSalePoint.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<IRepository<SalePoint>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetPcName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"PcName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultPc.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<IPcRepository>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetCertTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"CertTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<CertType>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetCustomerTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"CustomerTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<CustomerType>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetTicketTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"TicketTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultTicketType.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<ITicketTypeRepository>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetTicketTypeDisplayName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"TicketTypeDisplayName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                if (id.Value < 0)
                {
                    return DefaultTicketType.GetName(id.Value);
                }

                var repository = _serviceProvider.GetRequiredService<ITicketTypeRepository>();
                var ticketType = repository.FirstOrDefault(id.Value);

                return ticketType?.GetDisplayName();
            });
        }

        public string GetTicketTypeClassName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"TicketTypeClassName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<TicketTypeClass>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetTradeTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"TradeTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<TradeType>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetPayTypeName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"PayTypeName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<PayType>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public PayType GetPayType(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }

            return _memoryCache.GetOrCreate($"PayType:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<PayType>>();

                return repository.GetAll().Where(o => o.Id == id).FirstOrDefault();
            });
        }

        public string GetChangCiName(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"ChangCiName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<ChangCi>>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }

        public string GetChangCiTimeRange(int? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"ChangCiTimeRange:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<IRepository<ChangCi>>();

                var changCi = repository.GetAll().Where(c => c.Id == id).Select(c => new { c.Stime, c.Etime }).FirstOrDefault();
                if (changCi == null)
                {
                    return string.Empty;
                }

                return $"{changCi.Stime}-{changCi.Etime}";
            });
        }

        public string GetSeatName(long? id)
        {
            if (!id.HasValue)
            {
                return string.Empty;
            }

            return _memoryCache.GetOrCreate($"SeatName:{id}", entry =>
            {
                entry.SlidingExpiration = _slidingExpiration;

                var repository = _serviceProvider.GetRequiredService<ISeatRepository>();

                return repository.GetAll().Where(o => o.Id == id).Select(o => o.Name).FirstOrDefault();
            });
        }
    }
}
