using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Caches;
using Egoal.Cryptography;
using Egoal.Domain.Repositories;
using Egoal.Excel;
using Egoal.Extensions;
using Egoal.Invoice;
using Egoal.Net.Http;
using Egoal.Payment;
using Egoal.Runtime.Session;
using Egoal.Scenics;
using Egoal.Tickets.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public class TicketSaleQueryAppService : ApplicationService, ITicketSaleQueryAppService
    {
        private readonly ScenicOptions _scenicOptions;

        private readonly ITicketSaleRepository _ticketSaleRepository;
        private readonly ITicketSaleBuyerRepository _ticketSaleBuyerRepository;
        private readonly ITicketSaleSeatRepository _ticketSaleSeatRepository;
        private readonly ITicketCheckRepository _ticketCheckRepository;
        private readonly ITicketCheckDayStatRepository _ticketCheckDayStatRepository;
        private readonly IRepository<TicketReprintLog, long> _ticketReprintLogRepository;
        private readonly ITicketExchangeHistoryRepository _ticketExchangeHistoryRepository;
        private readonly ITicketConsumeRepository _ticketConsumeRepository;
        private readonly IRepository<InvoiceInfo> _invoiceRepository;
        private readonly INameCacheService _nameCacheService;
        private readonly IInvoiceService _invoiceService;
        private readonly IScenicAppService _scenicAppService;
        private readonly IPayTypeAppService _payTypeAppService;
        private readonly ITicketSaleDomainService _ticketSaleDomainService;
        private readonly ISession _session;

        public TicketSaleQueryAppService(
            IOptions<ScenicOptions> scenicOptions,
            ITicketSaleRepository ticketSaleRepository,
            ITicketSaleBuyerRepository ticketSaleBuyerRepository,
            ITicketSaleSeatRepository ticketSaleSeatRepository,
            ITicketCheckRepository ticketCheckRepository,
            ITicketCheckDayStatRepository ticketCheckDayStatRepository,
            IRepository<TicketReprintLog, long> ticketReprintLogRepository,
            ITicketExchangeHistoryRepository ticketExchangeHistoryRepository,
            ITicketConsumeRepository ticketConsumeRepository,
            IRepository<InvoiceInfo> invoiceRepository,
            INameCacheService nameCacheService,
            IInvoiceService invoiceService,
            IScenicAppService scenicAppService,
            IPayTypeAppService payTypeAppService,
            ITicketSaleDomainService ticketSaleDomainService,
            ISession session)
        {
            _scenicOptions = scenicOptions.Value;

            _ticketSaleRepository = ticketSaleRepository;
            _ticketSaleBuyerRepository = ticketSaleBuyerRepository;
            _ticketSaleSeatRepository = ticketSaleSeatRepository;
            _ticketCheckRepository = ticketCheckRepository;
            _ticketCheckDayStatRepository = ticketCheckDayStatRepository;
            _ticketReprintLogRepository = ticketReprintLogRepository;
            _ticketExchangeHistoryRepository = ticketExchangeHistoryRepository;
            _ticketConsumeRepository = ticketConsumeRepository;
            _invoiceRepository = invoiceRepository;
            _nameCacheService = nameCacheService;
            _invoiceService = invoiceService;
            _scenicAppService = scenicAppService;
            _payTypeAppService = payTypeAppService;
            _ticketSaleDomainService = ticketSaleDomainService;
            _session = session;
        }

        public List<ComboboxItemDto<int>> GetTicketStatusComboboxItems()
        {
            var items = typeof(TicketStatus).ToComboboxItems();

            items.RemoveAll(item => item.Value == (int)TicketStatus.过期);

            return items;
        }

        public async Task<byte[]> QueryTicketSalesToExcelAsync(QueryTicketSaleInput input)
        {
            input.ShouldPage = false;

            var result = await QueryTicketSalesAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Items, "售票查询", string.Empty);
        }

        public async Task<PagedResultDto<TicketSaleListDto>> QueryTicketSalesAsync(QueryTicketSaleInput input)
        {
            var result = await _ticketSaleRepository.QueryTicketSalesAsync(input);

            foreach (var ticket in result.Items)
            {
                ticket.ValidFlagName = ticket.ValidFlag == true ? "有效" : "无效";
                ticket.PhotoBindFlagName = ticket.PhotoBindFlag == true ? "已登记" : "未登记";
                ticket.TradeSourceName = ticket.TradeSource.ToString();
                if (ticket.FingerStatusID == FingerStatus.已登记)
                {
                    ticket.FingerprintNum = await _ticketSaleRepository.GetFingerprintQuantityAsync(ticket.Id);
                    ticket.UnBindFingerprintNum = ticket.PersonNum - ticket.FingerprintNum;
                }
                if (ticket.PhotoBindFlag == true)
                {
                    ticket.PhotoBindTime = (await _ticketSaleRepository.GetFacePhotoBindTimeAsync(ticket.Id)).ToDateTimeString();
                }
                if (ticket.CashierId.HasValue && ticket.CashierName.IsNullOrEmpty())
                {
                    ticket.CashierName = _nameCacheService.GetStaffName(ticket.CashierId.Value);
                }
                if (ticket.CertNo.IsNullOrEmpty() && !input.CertNo.IsNullOrEmpty())
                {
                    ticket.CertNo = input.CertNo;
                }
                else
                {
                    if(ticket.CertNo.Length > 18)
                    {
                        ticket.CertNo = DES3Helper.Decrypt(ticket.CertNo);
                    }
                }
            }

            if (result.TotalCount > 0)
            {
                var totalRow = new TicketSaleListDto();
                totalRow.RowNum = "合计";
                totalRow.PersonNum = result.Items.Sum(t => t.PersonNum);
                totalRow.RealMoney = result.Items.Sum(t => t.RealMoney);
                totalRow.TicMoney = result.Items.Sum(t => t.TicMoney);
                result.Items.Add(totalRow);
            }

            return result;
        }

        public async Task<List<TicketSaleSimpleDto>> GetOrderTicketSalesAsync(string listNo)
        {
            var ticketDtos = new List<TicketSaleSimpleDto>();

            var tickets = await _ticketSaleRepository.GetAllListAsync(t => t.OrderListNo == listNo && t.TicketStatusId != TicketStatus.已退);
            foreach (var ticket in tickets)
            {
                var ticketDto = new TicketSaleSimpleDto();
                
                ticketDto.TicketCode = ticket.TicketCode;
                if(ticket.Tkid == TicketKind.二代证)
                {
                    ticketDto.ShowTicketCode = TicketKind.二代证.ToString();
                }
                else
                {
                    ticketDto.ShowTicketCode = ticket.TicketCode;
                }
                ticketDto.TicketStatusName = ticket.TicketStatusName;
                ticketDto.TicketTypeId = ticket.TicketTypeId.Value;
                ticketDto.TicketTypeName = ticket.TicketTypeName;
                ticketDto.Quantity = ticket.PersonNum.Value;
                ticketDto.SurplusQuantity = await _ticketSaleDomainService.GetSurplusNumAsync(ticket);
                ticketDto.TotalNum = ticket.TotalNum.Value;
                ticketDto.Etime = ticket.Etime;
                ticketDto.OrderDetailId = ticket.OrderDetailId;
                ticketDto.IsUsable = await _ticketSaleDomainService.IsUsableAsync(ticket);
                ticketDto.AllowRefund = await _ticketSaleDomainService.AllowRefundAsync(ticket);
                ticketDto.IsActive = await _ticketSaleDomainService.IsActiveAsync(ticket);

                ticketDtos.Add(ticketDto);
            }

            return ticketDtos;
        }

        public async Task<List<TouristDto>> GetOrderTouristsAsync(string listNo)
        {
            var query = from tourist in _ticketSaleBuyerRepository.GetAll()
                        where tourist.OrderListNo == listNo
                        select new TouristDto
                        {
                            Name = tourist.BuyerName,
                            CertNo = tourist.CertNo,
                            Birthday = tourist.Birthday
                        };

            return await _ticketSaleBuyerRepository.ToListAsync(query);
        }

        public async Task<TicketSaleFullDto> GetTicketFullInfoAsync(string ticketCode)
        {
            var ticketSale = await _ticketSaleRepository.GetAll()
                .AsNoTracking()
                .Include(t => t.TicketGrounds)
                .FirstOrDefaultAsync(t => t.TicketCode == ticketCode && t.TicketStatusId != TicketStatus.已退);

            if (ticketSale == null)
            {
                throw new UserFriendlyException("暂无数据");
            }

            var ticketSaleDto = new TicketSaleFullDto();
            ticketSaleDto.TicketCode = ticketSale.TicketCode;
            ticketSaleDto.ListNo = ticketSale.ListNo;
            ticketSaleDto.TicketStatusName = ticketSale.TicketStatusId.ToString();
            ticketSaleDto.TicketTypeName = _nameCacheService.GetTicketTypeName(ticketSale.TicketTypeId);
            ticketSaleDto.ReaPrice = ticketSale.ReaPrice.Value;
            ticketSaleDto.RealMoney = ticketSale.ReaMoney.Value;
            ticketSaleDto.PayTypeName = _nameCacheService.GetPayTypeName(ticketSale.PayTypeId);
            ticketSaleDto.Quantity = ticketSale.PersonNum.Value;
            ticketSaleDto.SurplusQuantity = await _ticketSaleDomainService.GetSurplusNumAsync(ticketSale);
            ticketSaleDto.TotalNum = ticketSale.TotalNum.Value;
            ticketSaleDto.Stime = ticketSale.Stime;
            ticketSaleDto.Etime = ticketSale.Etime;
            ticketSaleDto.CustomerName = _nameCacheService.GetCustomerName(ticketSale.CustomerId);
            ticketSaleDto.CashierName = _nameCacheService.GetStaffName(ticketSale.CashierId);
            ticketSaleDto.SalePointName = _nameCacheService.GetSalePointName(ticketSale.SalePointId);
            ticketSaleDto.Ctime = ticketSale.Ctime;

            foreach (var ticketGround in ticketSale.TicketGrounds)
            {
                var ticketGroundDto = new TicketGroundDto();
                ticketGroundDto.Id = ticketGround.Id;
                ticketGroundDto.GroundName = _nameCacheService.GetGroundName(ticketGround.GroundId);
                ticketGroundDto.ChangCiName = _nameCacheService.GetChangCiName(ticketGround.ChangCiId);
                ticketGroundDto.SurplusNum = ticketGround.SurplusNum.Value;
                ticketGroundDto.Stime = ticketGround.Stime;
                ticketGroundDto.Etime = ticketGround.Etime;

                ticketSaleDto.Grounds.Add(ticketGroundDto);
            }

            var seats = await GetTicketSeatsAsync(new GetTicketSeatsInput { TicketId = ticketSale.Id });
            foreach (var seat in seats)
            {
                seat.GroundName = _nameCacheService.GetGroundName(seat.GroundId);
                seat.ChangCiName = _nameCacheService.GetChangCiName(seat.ChangCiId);
                seat.SeatName = _nameCacheService.GetSeatName(seat.SeatId);

                ticketSaleDto.Seats.Add(seat);
            }

            return ticketSaleDto;
        }

        public async Task<List<TicketSaleSeatDto>> GetTicketSeatsAsync(GetTicketSeatsInput input)
        {
            return await _ticketSaleSeatRepository.GetTicketSeatsAsync(input);
        }

        public async Task<decimal> GetOrderRealMoneyAsync(string listNo)
        {
            return await _ticketSaleRepository.GetOrderRealMoneyAsync(listNo);
        }

        public async Task<string> DownloadInvoiceAsync(DownloadInvoiceInput input)
        {
            var invoice = await _invoiceRepository
                .GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ListNo == input.ListNo);

            if (invoice == null)
            {
                throw new UserFriendlyException($"订单：{input.ListNo}未开发票");
            }

            DownloadRequest downloadRequest = new DownloadRequest();
            downloadRequest.FPQQLSH = input.ListNo;
            downloadRequest.FP_DM = invoice.FPDM;
            downloadRequest.FP_HM = invoice.FPHM;
            downloadRequest.JSHJ = invoice.JE;
            downloadRequest.KPRQ = invoice.CreateTime.To<DateTime>();

            var response = await _invoiceService.DownloadAsync(downloadRequest);

            if (response == null)
            {
                throw new UserFriendlyException("发票下载失败");
            }

            return response.FP_URL;
        }

        public async Task<PagedResultDto<MemberTicketSaleDto>> GetMemberTicketsForMobileAsync(GetMemberTicketsInput input)
        {
            var now = DateTime.Now;

            var query = _ticketSaleRepository.GetAll()
                .Where(t =>
                t.TicketStatusId != TicketStatus.作废 &&
                Convert.ToDateTime(t.Etime) >= now &&
                t.SurplusNum > 0 &&
                t.TicketTypeId != DefaultTicketType.电子会员卡);

            if (_session.GuiderId.HasValue)
            {
                if (_session.MemberId.HasValue)
                {
                    query = query.Where(t => (t.MemberId == _session.MemberId.Value && t.GuiderId == null) || t.GuiderId == _session.GuiderId.Value);
                }
                else
                {
                    query = query.Where(t => t.GuiderId == _session.GuiderId.Value);
                }
            }
            else if (_session.MemberId.HasValue)
            {
                query = query.Where(t => t.MemberId == _session.MemberId.Value && t.GuiderId == null);
            }

            var count = await _ticketSaleRepository.CountAsync(query);

            query = query.OrderByDescending(t => t.Ctime).PageBy(input);

            var ticketQuery = from ticket in query
                              select new
                              {
                                  ticket.TicketCode,
                                  ticket.TicketTypeName,
                                  StatusName = ticket.TicketStatusName,
                                  StartDate = ticket.Stime,
                                  EndDate = ticket.Etime,
                                  CTime = ticket.Ctime
                              };
            var list = await _ticketSaleRepository.ToListAsync(ticketQuery);

            var tickets = list.Select(t => new MemberTicketSaleDto
            {
                TicketCode = t.TicketCode,
                TicketTypeName = t.TicketTypeName,
                StatusName = t.StatusName,
                StartDate = t.StartDate.Substring(0, 10),
                EndDate = t.EndDate.Substring(0, 10),
                CTime = t.CTime.Value.ToDateTimeString()
            }).ToList();

            return new PagedResultDto<MemberTicketSaleDto>(count, tickets);
        }

        public async Task<CheckTicketOutput> GetLastCheckTicketInfoAsync(GetLastCheckTicketInfoInput input)
        {
            var startCtime = DateTime.Now.Date;
            var endCtime = DateTime.Now;

            var output = await _ticketCheckRepository.GetAll()
                .Where(t => t.Ctime >= startCtime && t.Ctime <= endCtime)
                .Where(t => t.CheckerId == input.StaffId)
                .Where(t => t.GateId == input.GateId)
                .OrderByDescending(t => t.Ctime)
                .Select(t => new CheckTicketOutput
                {
                    CardNo = t.CardNo,
                    TicketTypeId = t.TicketTypeId,
                    Stime = t.Stime,
                    Etime = t.Etime,
                    TotalNum = t.TotalNum,
                    SurplusNum = t.SurplusNum,
                    GroundId = t.GroundId,
                    CheckerId = t.CheckerId,
                    CheckTime = t.Ctime
                })
                .FirstOrDefaultAsync();

            if (output == null)
            {
                throw new UserFriendlyException("暂无数据");
            }

            output.TicketTypeName = _nameCacheService.GetTicketTypeName(output.TicketTypeId);
            output.GroundName = _nameCacheService.GetGroundName(output.GroundId);
            output.CheckerName = _nameCacheService.GetStaffName(output.CheckerId);

            return output;
        }

        public async Task<DynamicColumnResultDto> StatTouristByAgeRangeAsync(StatTouristByAgeRangeInput input)
        {
            var data = await _ticketSaleBuyerRepository.StatTouristByAgeRangeAsync(input);
            data.ColumnSum();

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTouristByAreaAsync(StatTouristByAreaInput input)
        {
            var data = await _ticketSaleBuyerRepository.StatTouristByAreaAsync(input);
            data.ColumnSum();

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTouristBySexAsync(StatTouristBySexInput input)
        {
            var data = await _ticketSaleBuyerRepository.StatTouristBySexAsync(input);

            decimal total = 0;
            foreach (DataRow row in data.Rows)
            {
                decimal.TryParse(row["人数"].ToString(), out decimal value);
                total += value;
            }

            DataColumn newColumn = new DataColumn();
            newColumn.ColumnName = "比例";
            data.Columns.Add(newColumn);
            foreach (DataRow row in data.Rows)
            {
                decimal.TryParse(row["人数"].ToString(), out decimal value);
                if (value > 0)
                {
                    row["比例"] = $"{(value / total * 100).ToString("F2")}%";
                }
            }

            return new DynamicColumnResultDto(data);
        }

        public async Task<PagedResultDto<TicketCheckListDto>> QueryTicketChecksAsync(QueryTicketCheckInput input)
        {
            var result = await _ticketCheckRepository.QueryTicketChecksAsync(input);

            if (result.TotalCount > 0)
            {
                var total = new TicketCheckListDto();
                total.RowNum = "合计";
                total.CheckNum = result.Items.Sum(t => t.CheckNum);
                result.Items.Add(total);
            }

            return result;
        }

        /// <summary>
        /// 检票入园统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DynamicColumnResultDto> StatTicketCheckInAsync(StatTicketCheckInInput input)
        {
            if (_scenicOptions.IsMultiPark)
            {
                return await StatTicketCheckInByParkAsync(input);
            }

            DataTable data = null;
            if (input.StatType == 1)
            {
                data = await _ticketCheckRepository.StatTicketCheckInByTimeAsync(input);
                for (int i = 1; i < data.Columns.Count - 1; i++)
                {
                    if (int.TryParse(data.Columns[i].ColumnName, out int value))
                    {
                        data.Columns[i].ColumnName = $"{data.Columns[i].ColumnName}-{(value + 1).ToString().PadLeft(2, '0')}";
                    }
                }
                data.ColumnSum();
            }
            else
            {
                data = await _ticketCheckRepository.StatTicketCheckInAsync(input);
                if (input.StatType == 2)
                {
                    string[] weeks = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
                    var rows = data.Rows.Cast<DataRow>().OrderBy(row => Array.IndexOf(weeks, row["星期"].ToString()));
                    var newTable = data.Clone();
                    foreach (var row in rows)
                    {
                        newTable.ImportRow(row);
                    }
                    data = newTable;
                }
            }

            return new DynamicColumnResultDto(data);
        }

        private async Task<DynamicColumnResultDto> StatTicketCheckInByParkAsync(StatTicketCheckInInput input)
        {
            DataTable data = null;
            if (input.StatType == 1)
            {
                data = await _ticketCheckRepository.StatTicketCheckInByParkAndTimeAsync(input);
                for (int i = 1; i < data.Columns.Count - 1; i++)
                {
                    if (int.TryParse(data.Columns[i].ColumnName, out int value))
                    {
                        data.Columns[i].ColumnName = $"{data.Columns[i].ColumnName}-{(value + 1).ToString().PadLeft(2, '0')}";
                    }
                }
                data.ColumnSum();
            }
            else
            {
                data = await _ticketCheckRepository.StatTicketCheckInByParkAsync(input);
                data.ColumnSum();
            }

            data.Columns["景点"].ReadOnly = false;
            foreach (DataRow row in data.Rows)
            {
                row["景点"] = _nameCacheService.GetParkName(Convert.ToInt32(row["景点"]));
            }

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckInByDateAndParkAsync(StatTicketCheckInInput input)
        {
            var parks = await _scenicAppService.GetParkComboboxItemsAsync();

            var table = await _ticketCheckRepository.StatTicketCheckInByDateAndParkAsync(input, parks);

            foreach (DataColumn column in table.Columns)
            {
                if (int.TryParse(column.ColumnName, out int parkId))
                {
                    column.ColumnName = _nameCacheService.GetParkName(parkId);
                }
            }

            table.ColumnSum();
            table.RowSum();

            return new DynamicColumnResultDto(table);
        }

        public async Task<DataTable> StatTicketCheckInByGateGroupAsync(StatTicketCheckInInput input)
        {
            var table = await _ticketCheckRepository.StatTicketCheckInByGateGroupAsync(input);

            table.Columns.Add("ParkName");
            table.Columns.Add("GateGroupName");
            table.Columns.Add("GateName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["ParkID"].ToString(), out int parkId))
                {
                    row["ParkName"] = _nameCacheService.GetParkName(parkId);
                }
                if (int.TryParse(row["GateGroupID"].ToString(), out int gateGroupId))
                {
                    row["GateGroupName"] = _nameCacheService.GetGateGroupName(gateGroupId);
                }
                if (int.TryParse(row["GateID"].ToString(), out int gateId))
                {
                    row["GateName"] = _nameCacheService.GetGateName(gateId);
                }
            }
            table.Columns.Remove("ParkID");
            table.Columns.Remove("GateGroupID");
            table.Columns.Remove("GateID");

            return table;
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckInByGroundAndGateGroupAsync(StatTicketCheckInInput input)
        {
            var table = await _ticketCheckRepository.StatTicketCheckInByGroundAndGateGroupAsync(input);

            table.Columns.Add("GroundName");
            table.Columns.Add("GateGroupName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["GroundID"].ToString(), out int groundId))
                {
                    row["GroundName"] = _nameCacheService.GetGroundName(groundId);
                }
                if (int.TryParse(row["GateGroupID"].ToString(), out int gateGroupId))
                {
                    row["GateGroupName"] = _nameCacheService.GetGateGroupName(gateGroupId);
                }
            }
            table.Columns.Remove("GroundID");
            table.Columns.Remove("GateGroupID");
            table.Columns["GroundName"].SetOrdinal(0);
            table.Columns["GateGroupName"].SetOrdinal(1);

            if (!table.IsNullOrEmpty())
            {
                table.RowSum(0, "合计", "GateGroupName");
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckByGroundAndTimeAsync(StatTicketCheckInInput input)
        {
            var table = await _ticketCheckRepository.StatTicketCheckByGroundAndTimeAsync(input);

            table.Columns["检票区域"].ReadOnly = false;
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["检票区域"].ToString(), out int groundId))
                {
                    row["检票区域"] = _nameCacheService.GetGroundName(groundId);
                }
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<DynamicColumnResultDto> StatStadiumTicketCheckInAsync(StatTicketCheckInInput input)
        {
            var data = await _ticketCheckDayStatRepository.StatStadiumTicketCheckInAsync(input);

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckByTradeSourceAsync(StatTicketCheckInInput input)
        {
            var data = await _ticketCheckRepository.StatTicketCheckByTradeSourceAsync(input);
            data.RemoveEmptyColumn();
            data.ColumnSum();

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckInByTicketTypeAsync(StatTicketCheckInInput input)
        {
            var table = await _ticketCheckRepository.StatTicketCheckInByTicketTypeAsync(input);
            table.Columns.Add("TicketTypeName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
            }
            table.Columns.Remove("TicketTypeID");

            return new DynamicColumnResultDto(table);
        }

        /// <summary>
        /// 检票年度对比统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DynamicColumnResultDto> StatTicketCheckInYearOverYearComparisonAsync(StatTicketCheckInInput input)
        {
            var today = DateTime.Now.Date;
            input.StartCTime = new DateTime(today.Year - input.StatType, 1, 1);
            input.EndCTime = new DateTime(today.Year, 12, 31);

            var data = await _ticketCheckDayStatRepository.StatTicketCheckInYearOverYearComparisonAsync(input);
            data.Columns["月份"].ReadOnly = false;
            foreach (DataRow row in data.Rows)
            {
                int.TryParse(row["月份"].ToString(), out int month);
                row["月份"] = $"{month}月";
            }

            return new DynamicColumnResultDto(data);
        }

        public async Task<TicketCheckOverviewResult> GetTicketCheckOverviewAsync(GetTicketCheckOverviewInput input)
        {
            var overview = new TicketCheckOverviewResult();
            overview.StadiumOverview = await GetStadiumTicketCheckOverviewAsync(input);
            overview.ScenicCheckInQuantity = await _ticketCheckDayStatRepository.GetScenicCheckInQuantityAsync(input.StartDate, input.EndDate);
            overview.ScenicCheckOutQuantity = await _ticketCheckDayStatRepository.GetScenicCheckOutQuantityAsync(input.StartDate, input.EndDate);

            overview.ScenicRealTimeQuantity = overview.ScenicCheckInQuantity - overview.ScenicCheckOutQuantity;
            int stadiumRealTimeQuantity = overview.StadiumOverview.Rows.Cast<DataRow>().Sum(r => Convert.ToInt32(r["RealTime"]));
            if (overview.ScenicRealTimeQuantity < stadiumRealTimeQuantity)
            {
                overview.ScenicRealTimeQuantity = stadiumRealTimeQuantity;
            }
            if (overview.ScenicRealTimeQuantity < 0)
            {
                overview.ScenicRealTimeQuantity = 0;
            }
            if (overview.ScenicCheckInQuantity < overview.ScenicRealTimeQuantity)
            {
                overview.ScenicCheckInQuantity = overview.ScenicRealTimeQuantity;
            }

            var parkCloseTime = $"{DateTime.Now.Date.ToDateString()} {_scenicOptions.ParkCloseTime}:00".To<DateTime>();
            if (DateTime.Now > parkCloseTime)
            {
                overview.ScenicRealTimeQuantity = 0;
            }

            return overview;
        }

        public async Task<DataTable> GetStadiumTicketCheckOverviewAsync(GetTicketCheckOverviewInput input)
        {
            var stadiumOverview = await _ticketCheckDayStatRepository.GetStadiumTicketCheckOverviewAsync(input.StartDate, input.EndDate);

            var parkCloseTime = $"{DateTime.Now.Date.ToDateString()} {_scenicOptions.ParkCloseTime}:00".To<DateTime>();

            stadiumOverview.Columns.Add("RealTime", typeof(int));
            foreach (DataRow row in stadiumOverview.Rows)
            {
                int.TryParse(row["CheckIn"].ToString(), out int checkIn);
                int.TryParse(row["CheckOut"].ToString(), out int checkOut);
                int realTime = checkIn - checkOut;

                row["RealTime"] = realTime > 0 ? realTime : 0;
                if (DateTime.Now > parkCloseTime)
                {
                    row["RealTime"] = 0;
                }
            }

            return stadiumOverview;
        }

        public async Task<int> GetScenicCheckInQuantityAsync(GetTicketCheckOverviewInput input)
        {
            return await _ticketCheckDayStatRepository.GetScenicCheckInQuantityAsync(input.StartDate, input.EndDate);
        }

        public async Task<DynamicColumnResultDto> StatTicketCheckInAverageAsync(StatTicketCheckInInput input)
        {
            DataTable data = null;
            if (input.StatType == 1)
            {
                data = await _ticketCheckDayStatRepository.StatTicketCheckInAverageByTimeslotAsync(input);
            }
            else if (input.StatType == 2)
            {
                data = await _ticketCheckDayStatRepository.StatTicketCheckInAverageByDateAsync(input);
            }
            else
            {
                data = await _ticketCheckDayStatRepository.StatTicketCheckInAverageByMonthAsync(input);
            }

            return new DynamicColumnResultDto(data);
        }

        public async Task<DynamicColumnResultDto> StatTicketSaleAsync(StatTicketSaleInput input)
        {
            var table = await _ticketSaleRepository.StatAsync(input);

            var columns = new Dictionary<TicketSaleStatType, ValueTuple<string, string, Func<int, string>>>();
            columns.Add(TicketSaleStatType.票类, ("TicketTypeID", "TicketTypeName", id => _nameCacheService.GetTicketTypeName(id)));
            columns.Add(TicketSaleStatType.销售渠道, ("TradeSource", "TradeSourceName", id => ((TradeSource)id).ToString()));

            if (columns.ContainsKey(input.StatType))
            {
                var originalColumn = columns[input.StatType].Item1;
                var newColumn = columns[input.StatType].Item2;

                table.Columns.Add(newColumn);

                foreach (DataRow row in table.Rows)
                {
                    if (int.TryParse(row[originalColumn].ToString(), out int id))
                    {
                        var func = columns[input.StatType].Item3;
                        row[newColumn] = func(id);
                    }
                }

                table.Columns.Remove(originalColumn);
                table.Columns[newColumn].SetOrdinal(0);
            }

            if (!table.IsNullOrEmpty())
            {
                table.RowSum();
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<DataTable> StatCashierSaleAsync(StatCashierSaleInput input)
        {
            var table = await _ticketSaleRepository.StatCashierSaleAsync(input);

            table.Columns.Add("CashierName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["CashierID"].ToString(), out int cashierId))
                {
                    row["CashierName"] = _nameCacheService.GetStaffName(cashierId);
                }
            }
            table.Columns.Remove("CashierID");

            return table;
        }

        public async Task<DataTable> StatTicketSaleByTradeSourceAsync(StatTicketSaleByTradeSourceInput input)
        {
            input.TicketTypeSearchGroupId = _session.SearchGroupId;

            var table = await _ticketSaleRepository.StatByTradeSourceAsync(input);

            table.Columns.Add("TradeSourceName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TradeSource"].ToString(), out int tradeSource))
                {
                    row["TradeSourceName"] = ((TradeSource)tradeSource).ToString();
                }
            }
            table.Columns.Remove("TradeSource");

            return table;
        }

        public async Task<DataTable> StatTicketSaleByTicketTypeClassAsync(StatTicketSaleByTicketTypeClassInput input)
        {
            var table = await _ticketSaleRepository.StatByTicketTypeClassAsync(input);

            table.Columns.Add("TicketTypeClassName");
            table.Columns.Add("TicketTypeName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TicketTypeClassID"].ToString(), out int ticketTypeClassId))
                {
                    row["TicketTypeClassName"] = _nameCacheService.GetTicketTypeClassName(ticketTypeClassId);
                }
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
            }
            table.Columns.Remove("TicketTypeClassID");
            table.Columns.Remove("TicketTypeID");

            return table;
        }

        public async Task<byte[]> StatTicketSaleByPayTypeToExcelAsync(StatTicketSaleByPayTypeInput input)
        {
            var result = await StatTicketSaleByPayTypeAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Data, "门票收款汇总", string.Empty);
        }

        public async Task<DynamicColumnResultDto> StatTicketSaleByPayTypeAsync(StatTicketSaleByPayTypeInput input)
        {
            var payTypes = await _payTypeAppService.GetPayTypeComboboxItemsAsync();

            var table = await _ticketSaleRepository.StatByPayTypeAsync(input, payTypes);
            foreach (DataColumn column in table.Columns)
            {
                if (int.TryParse(column.ColumnName, out int payTypeId))
                {
                    column.ColumnName = payTypes.FirstOrDefault(p => p.Value == payTypeId).DisplayText;
                }
            }

            DataColumn dataColumn = new DataColumn("票类");
            table.Columns.Add(dataColumn);
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row[dataColumn] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
            }
            table.Columns.Remove("TicketTypeID");
            dataColumn.SetOrdinal(0);

            if (!table.IsNullOrEmpty())
            {
                table.ColumnSum();
                table.RowSum();
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<DataTable> StatTicketSaleBySalePointAsync(StatTicketSaleBySalePointInput input)
        {
            var table = await _ticketSaleRepository.StatBySalePointAsync(input);

            table.Columns.Add("ParkName");
            table.Columns.Add("SalePointName");
            table.Columns.Add("TicketTypeName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["ParkID"].ToString(), out int parkId))
                {
                    row["ParkName"] = _nameCacheService.GetParkName(parkId);
                }
                if (int.TryParse(row["SalePointID"].ToString(), out int salePointId))
                {
                    row["SalePointName"] = _nameCacheService.GetSalePointName(salePointId);
                }
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
            }
            table.Columns.Remove("ParkID");
            table.Columns.Remove("SalePointID");
            table.Columns.Remove("TicketTypeID");

            return table;
        }

        public async Task<DataTable> StatTicketSaleGroundSharingAsync(StatGroundSharingInput input)
        {
            var table = await _ticketSaleRepository.StatGroundSharingAsync(input);

            table.Columns.Add("SalePointName");
            table.Columns.Add("TicketTypeName");
            table.Columns.Add("GroundName");
            table.Columns.Add("SharingPrice", typeof(decimal));
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["SalePointID"].ToString(), out int salePointId))
                {
                    row["SalePointName"] = _nameCacheService.GetSalePointName(salePointId);
                }
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
                if (int.TryParse(row["GroundID"].ToString(), out int groundId))
                {
                    row["GroundName"] = _nameCacheService.GetGroundName(groundId);
                }

                if (!decimal.TryParse(row["SharingRate"].ToString(), out decimal sharingRate))
                {
                    sharingRate = 100;
                }
                sharingRate = sharingRate / 100;
                row["SharingRate"] = sharingRate;

                decimal.TryParse(row["ReaPrice"].ToString(), out decimal realPrice);
                row["SharingPrice"] = Math.Round(realPrice * sharingRate, 2);
            }
            table.Columns.Remove("SalePointID");
            table.Columns.Remove("TicketTypeID");
            table.Columns.Remove("GroundID");

            return table;
        }

        public async Task<DataTable> StatTicketSaleJbAsync(StatJbInput input)
        {
            var table = await _ticketSaleRepository.StatJbAsync(input);

            if (input.StatTicketByPayType)
            {
                table.Columns.Add("PayTypeName");
            }
            table.Columns.Add("TradeTypeName");
            table.Columns.Add("TicketTypeName");
            foreach (DataRow row in table.Rows)
            {
                if (input.StatTicketByPayType && int.TryParse(row["PayTypeID"].ToString(), out int payTypeId))
                {
                    row["PayTypeName"] = _nameCacheService.GetPayTypeName(payTypeId);
                }
                if (int.TryParse(row["TradeTypeID"].ToString(), out int tradeTypeId))
                {
                    row["TradeTypeName"] = _nameCacheService.GetTradeTypeName(tradeTypeId);
                }
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
            }
            if (input.StatTicketByPayType)
            {
                table.Columns.Remove("PayTypeID");
            }
            table.Columns.Remove("TradeTypeID");
            table.Columns.Remove("TicketTypeID");

            return table;
        }

        public async Task<PagedResultDto<TicketReprintLogListDto>> QueryReprintLogsAsync(QueryReprintLogInput input)
        {
            var query = _ticketReprintLogRepository.GetAll()
                .Where(r => r.Ctime >= input.StartCTime)
                .Where(r => r.Ctime <= input.EndCTime)
                .WhereIf(input.TicketTypeId.HasValue, r => r.TicketTypeId == input.TicketTypeId)
                .WhereIf(!input.TicketCode.IsNullOrEmpty(), r => r.TicketCode == input.TicketCode)
                .WhereIf(!input.CardNo.IsNullOrEmpty(), r => r.CardNo == input.CardNo)
                .WhereIf(input.CashierId.HasValue, r => r.CashierId == input.CashierId)
                .WhereIf(input.CashpcId.HasValue, r => r.CashPcid == input.CashpcId)
                .WhereIf(input.SalePointId.HasValue, r => r.SalePointId == input.SalePointId);

            var count = await _ticketReprintLogRepository.CountAsync(query);

            var resultQuery = query.OrderByDescending(r => r.Id).PageBy(input).Select(log => new TicketReprintLogListDto
            {
                Id = log.Id,
                TicketId = log.TicketId,
                TicketTypeId = log.TicketTypeId,
                TicketCode = log.TicketCode,
                CardNo = log.CardNo,
                CashierId = log.CashierId,
                CashPcid = log.CashPcid,
                SalePointId = log.SalePointId,
                ParkId = log.ParkId,
                Ctime = log.Ctime
            });

            var items = await _ticketReprintLogRepository.ToListAsync(resultQuery);
            int rowNum = input.StartRowNum;
            foreach (var item in items)
            {
                item.TicketTypeName = _nameCacheService.GetTicketTypeName(item.TicketTypeId);
                item.CashierName = _nameCacheService.GetStaffName(item.CashierId);
                item.CashPcname = _nameCacheService.GetPcName(item.CashPcid);
                item.SalePointName = _nameCacheService.GetSalePointName(item.SalePointId);
                item.ParkName = _nameCacheService.GetParkName(item.ParkId);
                item.RowNum = rowNum;
                rowNum++;
            }

            return new PagedResultDto<TicketReprintLogListDto>(count, items);
        }

        public async Task<PagedResultDto<TicketExchangeHistoryListDto>> QueryExchangeHistorysAsync(QueryExchangeHistoryInput input)
        {
            var query = _ticketExchangeHistoryRepository.GetAll()
                .Where(e => e.Ctime >= input.StartCTime)
                .Where(e => e.Ctime <= input.EndCTime)
                .WhereIf(!input.OrderListNo.IsNullOrEmpty(), e => e.OrderListNo == input.OrderListNo)
                .WhereIf(!input.OldTicketCode.IsNullOrEmpty(), e => e.OldTicketCode == input.OldTicketCode)
                .WhereIf(input.TicketTypeId.HasValue, e => e.TicketTypeId == input.TicketTypeId)
                .WhereIf(input.CashierId.HasValue, e => e.CashierId == input.CashierId)
                .WhereIf(input.SalePointId.HasValue, e => e.SalePointId == input.SalePointId);

            var count = await _ticketExchangeHistoryRepository.CountAsync(query);

            var resultQuery = query.OrderByDescending(e => e.Id).PageBy(input).Select(e => new TicketExchangeHistoryListDto
            {
                Id = e.Id,
                OrderListNo = e.OrderListNo,
                TicketTypeId = e.TicketTypeId,
                OldTicketCode = e.OldTicketCode,
                OldCardNo = e.OldCardNo,
                NewTicketCode = e.NewTicketCode,
                NewCardNo = e.NewCardNo,
                Tkid = e.Tkid,
                SalePointId = e.SalePointId,
                CashierId = e.CashierId,
                Ctime = e.Ctime
            });

            var items = await _ticketExchangeHistoryRepository.ToListAsync(resultQuery);
            int rowNum = input.StartRowNum;
            foreach (var item in items)
            {
                item.TicketTypeName = _nameCacheService.GetTicketTypeName(item.TicketTypeId);
                item.CashierName = _nameCacheService.GetStaffName(item.CashierId);
                item.Tkname = item.Tkid?.ToString();
                item.SalePointName = _nameCacheService.GetSalePointName(item.SalePointId);
                item.RowNum = rowNum;
                rowNum++;
            }

            return new PagedResultDto<TicketExchangeHistoryListDto>(count, items);
        }

        public async Task<DataTable> StatExchangeHistoryJbAsync(StatJbInput input)
        {
            var table = await _ticketExchangeHistoryRepository.StatJbAsync(input);

            table.Columns.Add("TicketTypeName");
            table.Columns.Add("TKName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TicketTypeID"].ToString(), out int ticketTypeId))
                {
                    row["TicketTypeName"] = _nameCacheService.GetTicketTypeName(ticketTypeId);
                }
                if (int.TryParse(row["TKID"].ToString(), out int tkid))
                {
                    row["TKName"] = ((TicketKind)tkid).ToString();
                }
            }
            table.Columns.Remove("TicketTypeID");
            table.Columns.Remove("TKID");

            return table;
        }

        public async Task<byte[]> StatGroundChangCiSaleToExcelAsync(StatGroundChangCiSaleInput input)
        {
            var result = await StatGroundChangCiSaleAsync(input);

            result.Data.Columns["GroundName"].ColumnName = "项目";
            result.Data.Columns["ChangCiName"].ColumnName = "场次";
            result.Data.Columns["STime"].ColumnName = "起始时间";
            result.Data.Columns["ETime"].ColumnName = "截止时间";
            result.Data.Columns["TotalNum"].ColumnName = "总数量";
            result.Data.Columns["SaleNum"].ColumnName = "已售数量";
            result.Data.Columns["SurplusNum"].ColumnName = "剩余数量";

            return await ExcelHelper.ExportToExcelAsync(result.Data, "场次座位销售统计", string.Empty);
        }

        public async Task<DynamicColumnResultDto> StatGroundChangCiSaleAsync(StatGroundChangCiSaleInput input)
        {
            input.Week = (int)input.TravelDate.To<DateTime>().DayOfWeek;
            if (input.Week == 0)
            {
                input.Week = 7;
            }

            var table = await _ticketSaleSeatRepository.StatGroundChangCiSaleAsync(input);

            table.Columns.Add("GroundName");
            table.Columns.Add("ChangCiName");
            table.Columns["SurplusNum"].ReadOnly = false;
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["GroundID"].ToString(), out int groundId))
                {
                    row["GroundName"] = _nameCacheService.GetGroundName(groundId);
                }
                if (int.TryParse(row["ChangCiID"].ToString(), out int changCiId))
                {
                    row["ChangCiName"] = _nameCacheService.GetChangCiName(changCiId);
                }
                int.TryParse(row["TotalNum"].ToString(), out int totalNum);
                int.TryParse(row["SaleNum"].ToString(), out int saleNum);
                row["SurplusNum"] = totalNum - saleNum;
            }
            table.Columns.Remove("GroundID");
            table.Columns.Remove("ChangCiID");
            table.Columns["GroundName"].SetOrdinal(0);
            table.Columns["ChangCiName"].SetOrdinal(1);

            return new DynamicColumnResultDto(table);
        }

        public async Task<byte[]> QueryTicketConsumesToExcelAsync(QueryTicketConsumeInput input)
        {
            input.ShouldPage = false;

            var result = await QueryTicketConsumesAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Items, "核销查询", string.Empty);
        }

        public async Task<PagedResultDto<TicketConsumeListDto>> QueryTicketConsumesAsync(QueryTicketConsumeInput input)
        {
            var result = await _ticketConsumeRepository.QueryTicketConsumesAsync(input);
            foreach (var item in result.Items)
            {
                item.TicketTypeName = _nameCacheService.GetTicketTypeName(item.TicketTypeId);
                item.ConsumeMoney = item.Price * item.ConsumeNum;
                item.ConsumeTypeName = item.ConsumeType.ToString();
            }

            if (result.TotalCount > 0)
            {
                var total = new TicketConsumeListDto();
                total.RowNum = "合计";
                total.ConsumeNum = result.Items.Sum(i => i.ConsumeNum);
                total.ConsumeMoney = result.Items.Sum(i => i.ConsumeMoney);

                result.Items.Add(total);
            }

            return result;
        }

        public async Task<byte[]> StatTicketConsumeToExcelAsync(StatTicketConsumeInput input)
        {
            var items = await StatTicketConsumeAsync(input);

            return await ExcelHelper.ExportToExcelAsync(items, "核销统计", string.Empty);
        }

        public async Task<List<StatTicketConsumeListDto>> StatTicketConsumeAsync(StatTicketConsumeInput input)
        {
            var items = await _ticketConsumeRepository.StatTicketConsumeAsync(input);
            foreach (var item in items)
            {
                item.CustomerName = item.CustomerId.HasValue ? _nameCacheService.GetCustomerName(item.CustomerId) : "散客";
                item.TicketTypeName = _nameCacheService.GetTicketTypeName(item.TicketTypeId);
                item.CheckMoney = item.CheckNum * item.Price;
                item.ConsumeMoney = item.ConsumeNum * item.Price;
            }

            if (!items.IsNullOrEmpty())
            {
                var total = new StatTicketConsumeListDto();
                total.CustomerName = "合计";
                total.CheckNum = items.Sum(i => i.CheckNum);
                total.CheckMoney = items.Sum(i => i.CheckMoney);
                total.ConsumeNum = items.Sum(i => i.ConsumeNum);
                total.ConsumeMoney = items.Sum(i => i.ConsumeMoney);

                items.Add(total);
            }

            return items;
        }
    }
}
