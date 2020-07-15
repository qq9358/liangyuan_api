using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Auditing;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Orders;
using Egoal.Scenics;
using Egoal.Thirdparties.BigData.Dto;
using Egoal.ThirdPlatforms;
using Egoal.Threading.RateLimit;
using Egoal.Tickets;
using Egoal.TicketTypes;
using Egoal.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Thirdparties.BigData
{
    public class BigDataAppService : ApplicationService, IBigDataAppService
    {
        private readonly IRepository<ThirdPlatform, string> _thirdPlatformRepository;
        private readonly IRepository<Gate> _gateRepository;
        private readonly ITicketSaleBuyerRepository _ticketSaleBuyerRepository;
        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly ITradeRepository _tradeRepository;
        private readonly ITicketCheckRepository _ticketCheckRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<ApiLog, long> _apiLogRepository;
        private readonly INameCacheService _nameCacheService;
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly IRateLimiterManager _rateLimiterManager;
        private readonly ILogger _logger;

        public BigDataAppService(
            IRepository<ThirdPlatform, string> thirdPlatformRepository,
            IRepository<Gate> gateRepository,
            ITicketSaleBuyerRepository ticketSaleBuyerRepository,
            ITicketSaleRepository ticketSaleRepository,
            ITradeRepository tradeRepository,
            ITicketCheckRepository ticketCheckRepository,
            IOrderRepository orderRepository,
            IRepository<ApiLog, long> apiLogRepository,
            INameCacheService nameCacheService,
            IClientInfoProvider clientInfoProvider,
            IRateLimiterManager rateLimiterManager,
            ILogger<BigDataAppService> logger)
        {
            _thirdPlatformRepository = thirdPlatformRepository;
            _gateRepository = gateRepository;
            _ticketSaleBuyerRepository = ticketSaleBuyerRepository;
            _ticketSaleRepository = ticketSaleRepository;
            _tradeRepository = tradeRepository;
            _ticketCheckRepository = ticketCheckRepository;
            _orderRepository = orderRepository;
            _apiLogRepository = apiLogRepository;
            _nameCacheService = nameCacheService;
            _clientInfoProvider = clientInfoProvider;
            _rateLimiterManager = rateLimiterManager;
            _logger = logger;
        }

        public async Task<ResponseDto> ProcessRequestAsync(RequestDto request)
        {
            if (request == null)
            {
                throw new TmsException("数据出错或为空");
            }

            double interval = 5;
            if (!_rateLimiterManager.TryAcquire($"BigData:User:{request.username}", 1 / interval))
            {
                throw new TmsException("请求太频繁");
            }

            await CheckSignAsync(request);

            object responseBody = null;
            switch (request.method.ToLower())
            {
                case "query_device":
                    {
                        responseBody = await QueryDeviceAsync();
                        break;
                    }
                case "stat_tourist":
                    {
                        responseBody = await StatTouristAsync(request.request_body);
                        break;
                    }
                case "query_ticket":
                    {
                        responseBody = await QueryTicketSaleAsync(request.request_body);
                        break;
                    }
                case "stat_ticket":
                    {
                        responseBody = await StatTicketSaleAsync(request.request_body);
                        break;
                    }
                case "stat_flow":
                    {
                        responseBody = await StatTicketCheckAsync();
                        break;
                    }
                case "query_consume":
                    {
                        responseBody = await QueryTicketCheckAsync(request.request_body);
                        break;
                    }
                default:
                    {
                        throw new TmsException("无效请求");
                    }
            }

            var response = new ResponseDto();
            response.success = true;
            response.message = "成功";
            response.response_body = Base64Helper.Encode(responseBody.ToJson());
            return response;
        }

        private async Task CheckSignAsync(RequestDto request)
        {
            var thirdPlatform = await _thirdPlatformRepository.FirstOrDefaultAsync(t => t.Uid == request.username);
            if (thirdPlatform == null)
            {
                throw new TmsException("用户名不存在");
            }

            string sign = GenerateSign(request, thirdPlatform.Pwd);
            if (sign != request.sign)
            {
                throw new TmsException("签名错误");
            }
        }

        private string GenerateSign(RequestDto request, string key)
        {
            var stringToSign = $"{request.username}{request.method}{request.request_body}{key}";

            return MD5Helper.Encrypt(stringToSign);
        }

        private async Task<List<DeviceListDto>> QueryDeviceAsync()
        {
            var gates = await _gateRepository.GetAllListAsync();
            if (gates.IsNullOrEmpty())
            {
                throw new TmsException("无数据");
            }

            var devices = new List<DeviceListDto>();
            foreach (var gate in gates)
            {
                var device = new DeviceListDto();
                device.device_name = gate.Name;
                device.device_address = gate.TcpIp.IsNullOrEmpty() ? $"{gate.Pcid}_{gate.SpPort}" : gate.TcpIp;
                device.device_type = gate.GateTypeId?.ToString();
                device.device_io = gate.InOutFlag == true ? "入口" : "出口";
                device.device_status = gate.Status == 1 && gate.LastStatusUpdateTime >= DateTime.Now.AddMinutes(-1) ? "1" : "2";

                devices.Add(device);
            }

            return devices;
        }

        private async Task<List<StatTouristListDto>> StatTouristAsync(string requestBody)
        {
            var input = GetAndCheckInput<StatTouristInput>(requestBody);

            var statInput = new Tickets.Dto.StatTouristInput();
            statInput.StartCTime = input.start_date;
            statInput.EndCTime = input.end_date;
            statInput.StatByArea = input.stat_by_area;
            statInput.StatBySex = input.stat_by_sex;
            statInput.StatByNation = input.stat_by_nation;
            statInput.StatByAge = input.stat_by_age;

            var dataItems = await _ticketSaleBuyerRepository.StatTouristAsync(statInput);
            if (dataItems.IsNullOrEmpty())
            {
                throw new TmsException("无数据");
            }

            var items = new List<StatTouristListDto>();
            foreach (var dataItem in dataItems)
            {
                var item = new StatTouristListDto();
                item.date = dataItem.Date;
                item.area = dataItem.Area;
                item.sex = dataItem.Sex;
                item.nation = dataItem.Nation;
                item.age_range = dataItem.AgeRange;
                item.quantity = dataItem.Quantity.ToString();

                items.Add(item);
            }

            return items;
        }

        private async Task<QueryTicketSaleOutput> QueryTicketSaleAsync(string requestBody)
        {
            var input = GetAndCheckInput<QueryTicketSaleInput>(requestBody);

            var query = from a in _ticketSaleRepository.GetAll()
                        .Where(t => t.Ctime >= input.start_date && t.Ctime <= input.end_date)
                        .Where(t => t.CommitFlag == true)
                        .Where(t => t.TicketTypeTypeId != TicketTypeType.管理卡)
                        join b in _tradeRepository.GetAll() on a.TradeId equals b.Id
                        orderby a.Ctime
                        select new TicketSaleListDto
                        {
                            list_no = a.ListNo,
                            OrderListNo = a.OrderListNo,
                            TradeSource = b.TradeSource,
                            ota = b.Ota,
                            sale_time = a.Ctime,
                            group_name = a.CustomerName,
                            travel_date = a.Stime,
                            TicketTypeTypeId = a.TicketTypeTypeId,
                            product_name = a.TicketTypeName,
                            ticket_id = a.Id,
                            ticket_code = a.CardNo,
                            quantity = a.PersonNum,
                            price = a.ReaPrice,
                            total_money = a.ReaMoney,
                            status = a.TicketStatusName,
                            end_time = a.Etime,
                            ParkId = a.ParkId,
                            AreaId = b.AreaId,
                            HasTourist = a.TicketSaleBuyers.Any()
                        };

            var count = await _ticketSaleRepository.CountAsync(query);
            if (count <= 0)
            {
                throw new TmsException("无数据");
            }

            query = query.PageBy(new PagedInputDto { MaxResultCount = input.page_size, SkipCount = (input.page_index - 1) * input.page_size });
            var items = await _ticketSaleRepository.ToListAsync(query);
            foreach (var item in items)
            {
                item.trade_source = item.TradeSource.ToString();
                item.sale_type = item.group_name.IsNullOrEmpty() ? "散票" : "团票";
                item.travel_date = item.travel_date?.Substring(0, 10);
                item.product_type = item.TicketTypeTypeId?.ToString();
                item.park_name = _nameCacheService.GetParkName(item.ParkId);
                item.tourists = await GetTouristsAsync(item);
            }

            return new QueryTicketSaleOutput { total_quantity = count, tickets = items };
        }

        private async Task<List<TouristListDto>> GetTouristsAsync(TicketSaleListDto ticketSale)
        {
            if (ticketSale.HasTourist)
            {
                var ticketId = ticketSale.ticket_id.To<long>();

                var query = _ticketSaleBuyerRepository.GetAll()
                    .Where(t => t.TicketId == ticketId)
                    .Select(t => new TouristListDto
                    {
                        name = t.BuyerName,
                        mobile = t.Mobile,
                        cert_type = t.CertTypeName,
                        cert_no = t.CertNo,
                        tourist_source = t.Address
                    });

                return await _ticketSaleBuyerRepository.ToListAsync(query);
            }

            var tourist = new TouristListDto();
            tourist.tourist_source = _nameCacheService.GetAreaName(ticketSale.AreaId);
            if (!ticketSale.OrderListNo.IsNullOrEmpty())
            {
                var order = await _orderRepository.GetAll()
                    .Where(o => o.Id == ticketSale.OrderListNo)
                    .Select(o => new
                    {
                        o.YdrName,
                        o.Mobile,
                        o.CertTypeName,
                        o.CertNo,
                        o.JidiaoMobile,
                        o.JidiaoName
                    })
                    .FirstOrDefaultAsync();

                tourist.name = order.YdrName ?? order.JidiaoName;
                tourist.mobile = order.Mobile ?? order.JidiaoMobile;
                tourist.cert_type = order.CertTypeName;
                tourist.cert_no = order.CertNo;
            }

            var tourists = new List<TouristListDto>();
            if (!tourist.IsNullOrEmpty())
            {
                tourists.Add(tourist);
            }

            return tourists;
        }

        private async Task<List<StatTicketSaleListDto>> StatTicketSaleAsync(string requestBody)
        {
            var input = GetAndCheckInput<StatTicketSaleInput>(requestBody);

            var statInput = new Tickets.Dto.StatTicketSaleByTradeSourceInput();
            statInput.StartCTime = input.start_date;
            statInput.EndCTime = input.end_date;
            statInput.StatType = input.stat_type.ToString();

            var table = await _ticketSaleRepository.StatByTradeSourceAsync(statInput);
            if (table.IsNullOrEmpty())
            {
                throw new TmsException("无数据");
            }

            var items = new List<StatTicketSaleListDto>();
            foreach (DataRow row in table.Rows)
            {
                var item = StatTicketSaleListDto.FromDataRow(row);

                items.Add(item);
            }

            return items;
        }

        private async Task<List<StatTicketCheckListDto>> StatTicketCheckAsync()
        {
            var input = new Tickets.Dto.StatTicketCheckInInput();
            input.StartCTime = DateTime.Now.Date;
            input.EndCTime = DateTime.Now.Date.AddDays(1);

            var table = await _ticketCheckRepository.StatTicketCheckByGroundAndTimeAsync(input);
            if (table.IsNullOrEmpty())
            {
                throw new TmsException("无数据");
            }

            var items = new List<StatTicketCheckListDto>();
            foreach (DataRow row in table.Rows)
            {
                var item = StatTicketCheckListDto.FromDataRow(row);
                if (int.TryParse(item.ground_name, out int groundId))
                {
                    item.ground_name = _nameCacheService.GetGroundName(groundId);
                }

                items.Add(item);
            }

            return items;
        }

        private async Task<QueryTicketCheckOutput> QueryTicketCheckAsync(string requestBody)
        {
            var input = GetAndCheckInput<QueryTicketCheckInput>(requestBody);

            var query = _ticketCheckRepository.GetAll()
                .Where(t => t.Ctime >= input.start_date && t.Ctime <= input.end_date)
                .Where(t => t.CommitFlag == true)
                .Where(t => t.TicketTypeTypeId != TicketTypeType.管理卡)
                .OrderBy(t => t.Ctime)
                .Select(t => new TicketCheckListDto
                {
                    list_no = t.ListNo,
                    ticket_code = t.CardNo,
                    ticket_id = t.TicketId,
                    consume_quantity = t.CheckNum,
                    consume_time = t.Ctime,
                    GroundId = t.GroundId,
                    ParkId = t.ParkId
                });

            var count = await _ticketCheckRepository.CountAsync(query);
            if (count <= 0)
            {
                throw new TmsException("无数据");
            }

            query = query.PageBy(new PagedInputDto { MaxResultCount = input.page_size, SkipCount = (input.page_index - 1) * input.page_size });
            var items = await _ticketCheckRepository.ToListAsync(query);

            foreach (var item in items)
            {
                item.ground_name = _nameCacheService.GetGroundName(item.GroundId);
                item.park_name = _nameCacheService.GetParkName(item.ParkId);
            }

            return new QueryTicketCheckOutput { total_quantity = count, tickets = items };
        }

        private T GetAndCheckInput<T>(string requestBody) where T : InputBase
        {
            var input = Base64Helper.Decode(requestBody).JsonToObject<T>();
            if (input == null)
            {
                throw new TmsException("数据出错或为空");
            }
            input.Validate();

            return input;
        }

        public async Task AddApiLogAsync(RequestDto request, Exception exception = null)
        {
            try
            {
                var log = new ApiLog();
                log.ServiceName = "Egoal.Thirdparties.BigData.BigDataAppService";
                log.RequestContent = new
                {
                    request.username,
                    request.method,
                    request.sign,
                    RequestBody = Base64Helper.Decode(request.request_body)?.JsonToObject<object>()
                }.ToJson();
                log.ResponseContent = new
                {
                    message = "数据过多不记录"
                }.ToJson();
                if (exception != null)
                {
                    log.Exception = exception is TmsException ? exception.Message : exception.StackTrace;
                }
                log.ClientIpAddress = _clientInfoProvider.ClientIpAddress;
                log.ClientName = _clientInfoProvider.ComputerName;
                log.Ctime = DateTime.Now;

                await _apiLogRepository.InsertAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
        }
    }
}
