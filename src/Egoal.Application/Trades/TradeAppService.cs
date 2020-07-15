using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Caches;
using Egoal.Excel;
using Egoal.Extensions;
using Egoal.Payment;
using Egoal.Tickets;
using Egoal.Tickets.Dto;
using Egoal.Trades.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Trades
{
    public class TradeAppService : ApplicationService, ITradeAppService
    {
        private readonly INameCacheService _nameCacheService;
        private readonly ITradeRepository _tradeRepository;
        private readonly IPayTypeAppService _payTypeAppService;
        private readonly IPayDetailRepository _payDetailRepository;

        public TradeAppService(
            INameCacheService nameCacheService,
            ITradeRepository tradeRepository,
            IPayTypeAppService payTypeAppService,
            IPayDetailRepository payDetailAppRepository)
        {
            _nameCacheService = nameCacheService;
            _tradeRepository = tradeRepository;
            _payTypeAppService = payTypeAppService;
            _payDetailRepository = payDetailAppRepository;
        }

        public async Task SaleTicketAsync(SaleTicketInput input)
        {
            var trade = input.MapToTrade();
            trade.SetTotalMoney(input.TotalMoney);
            trade.StatFlag = input.StatFlag;

            var tradeDetail = trade.MapToTradeDetail();
            tradeDetail.SetTotalMoney(input.TotalMoney);
            trade.TradeDetails.Add(tradeDetail);

            if (input.PayFlag)
            {
                trade.Pay(input.PayTypeId.Value, input.PayTypeName);
            }

            await _tradeRepository.InsertAsync(trade);
        }

        public async Task RefundTicketAsync(RefundTicketInput input)
        {
            var trade = input.MapToTrade();

            var originalTrade = await _tradeRepository.GetAsync(input.OriginalTradeId);
            originalTrade.CopyTo(trade);
            trade.ReturnTradeId = input.OriginalTradeId;
            trade.SetTotalMoney(input.TotalMoney);

            var tradeDetail = trade.MapToTradeDetail();
            tradeDetail.SetTotalMoney(input.TotalMoney);
            trade.TradeDetails.Add(tradeDetail);

            var payDetail = trade.MapToPayDetail();
            trade.PayDetails.Add(payDetail);

            await _tradeRepository.InsertAsync(trade);
        }

        public async Task<List<ComboboxItemDto<int>>> GetTradeTypeTypeComboboxItemsAsync()
        {
            return await _tradeRepository.GetTradeTypeTypeComboboxItemsAsync();
        }

        public async Task<List<ComboboxItemDto<int>>> GetTradeTypeComboboxItemsAsync(int? tradeTypeTypeId)
        {
            return await _tradeRepository.GetTradeTypeComboboxItemsAsync(tradeTypeTypeId);
        }

        public async Task<PagedResultDto<TradeListDto>> QueryTradesAsync(QueryTradeInput input)
        {
            var result = await _tradeRepository.QueryTradesAsync(input);

            foreach (var trade in result.Items)
            {
                if (trade.CashierId.HasValue && trade.CashierName.IsNullOrEmpty())
                {
                    trade.CashierName = _nameCacheService.GetStaffName(trade.CashierId.Value);
                }
            }

            if (result.TotalCount > 0)
            {
                var total = new TradeListDto();
                total.RowNum = "合计";
                total.TotalMoney = result.Items.Sum(t => t.TotalMoney);
                result.Items.Add(total);
            }

            return result;
        }

        public async Task<byte[]> StatPayDetailToExcelAsync(StatPayDetailInput input)
        {
            var result = await StatPayDetailAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Data, "收款汇总", string.Empty);
        }

        public async Task<DynamicColumnResultDto> StatPayDetailAsync(StatPayDetailInput input)
        {
            var payTypes = await _payTypeAppService.GetPayTypeComboboxItemsAsync();

            var table = await _payDetailRepository.StatAsync(input, payTypes);
            foreach (DataColumn column in table.Columns)
            {
                if (int.TryParse(column.ColumnName, out int payTypeId))
                {
                    column.ColumnName = payTypes.FirstOrDefault(p => p.Value == payTypeId).DisplayText;
                }
            }

            if (input.StatType == PayDetailStatType.按日期)
            {
                table.Columns["CDate"].ColumnName = "日期";
            }
            else
            {
                var statTypeColumns = new string[] { "ParkID", "SalePointID", "CashierID" };
                var statTypeColumn = statTypeColumns[(int)input.StatType - 2];
                var statTypeNames = new string[] { "景点", "售票点", "收银员" };
                var statTypeName = statTypeNames[(int)input.StatType - 2];
                DataColumn dataColumn = new DataColumn(statTypeName);
                table.Columns.Add(dataColumn);
                foreach (DataRow row in table.Rows)
                {
                    if (int.TryParse(row[statTypeColumn].ToString(), out int id))
                    {
                        if (input.StatType == PayDetailStatType.按景点)
                        {
                            row[dataColumn] = _nameCacheService.GetParkName(id);
                        }
                        if (input.StatType == PayDetailStatType.按售票点)
                        {
                            row[dataColumn] = _nameCacheService.GetSalePointName(id);
                        }
                        if (input.StatType == PayDetailStatType.按收银员)
                        {
                            row[dataColumn] = _nameCacheService.GetStaffName(id);
                        }
                    }
                }
                table.Columns.Remove(statTypeColumn);
                dataColumn.SetOrdinal(0);
            }

            if (!table.IsNullOrEmpty())
            {
                table.ColumnSum();
                table.RowSum();
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<DataTable> StatPayDetailJbAsync(StatJbInput input)
        {
            var table = await _payDetailRepository.StatJbAsync(input);

            table.Columns.Add("Ratio", typeof(decimal));
            var totalMoney = table.Rows.Cast<DataRow>().Sum(r => r["PayMoney"].To<decimal>());
            if (totalMoney != 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    decimal.TryParse(row["PayMoney"].ToString(), out decimal payMoney);
                    row["Ratio"] = Math.Round(payMoney / totalMoney, 6);
                }
            }

            return table;
        }

        public async Task<DynamicColumnResultDto> StatTradeAsync(StatTradeInput input)
        {
            var table = await _tradeRepository.StatAsync(input);

            table.Columns.Add("TradeTypeName");
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TradeTypeID"].ToString(), out int id))
                {
                    row["TradeTypeName"] = _nameCacheService.GetTradeTypeName(id);
                }
            }
            table.Columns.Remove("TradeTypeID");
            table.Columns["TradeTypeName"].SetOrdinal(input.StatType == TradeStatType.交易类型 ? 0 : 1);

            if (!table.IsNullOrEmpty())
            {
                table.RowSum();
            }

            return new DynamicColumnResultDto(table);
        }

        public async Task<byte[]> StatTradeByPayTypeToExcelAsync(StatTradeByPayTypeInput input)
        {
            var result = await StatTradeByPayTypeAsync(input);

            return await ExcelHelper.ExportToExcelAsync(result.Data, "交易收款汇总", string.Empty);
        }

        public async Task<DynamicColumnResultDto> StatTradeByPayTypeAsync(StatTradeByPayTypeInput input)
        {
            var payTypes = await _payTypeAppService.GetPayTypeComboboxItemsAsync();

            var table = await _tradeRepository.StatByPayTypeAsync(input, payTypes);
            foreach (DataColumn column in table.Columns)
            {
                if (int.TryParse(column.ColumnName, out int payTypeId))
                {
                    column.ColumnName = payTypes.FirstOrDefault(p => p.Value == payTypeId).DisplayText;
                }
            }

            DataColumn dataColumn = new DataColumn("交易类型");
            table.Columns.Add(dataColumn);
            foreach (DataRow row in table.Rows)
            {
                if (int.TryParse(row["TradeTypeID"].ToString(), out int tradeTypeId))
                {
                    row[dataColumn] = _nameCacheService.GetTradeTypeName(tradeTypeId);
                }
            }
            table.Columns.Remove("TradeTypeID");
            dataColumn.SetOrdinal(0);

            if (!table.IsNullOrEmpty())
            {
                table.ColumnSum();
                table.RowSum();
            }

            return new DynamicColumnResultDto(table);
        }
    }
}
