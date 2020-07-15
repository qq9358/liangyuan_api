using Egoal.Application.Services.Dto;
using Egoal.Tickets.Dto;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleQueryAppService
    {
        List<ComboboxItemDto<int>> GetTicketStatusComboboxItems();
        Task<PagedResultDto<TicketSaleListDto>> QueryTicketSalesAsync(QueryTicketSaleInput input);
        Task<byte[]> QueryTicketSalesToExcelAsync(QueryTicketSaleInput input);
        Task<List<TicketSaleSimpleDto>> GetOrderTicketSalesAsync(string listNo);
        Task<List<TouristDto>> GetOrderTouristsAsync(string listNo);
        Task<List<TicketSaleSeatDto>> GetTicketSeatsAsync(GetTicketSeatsInput input);
        Task<TicketSaleFullDto> GetTicketFullInfoAsync(string ticketCode);
        Task<decimal> GetOrderRealMoneyAsync(string listNo);
        Task<string> DownloadInvoiceAsync(DownloadInvoiceInput input);
        Task<PagedResultDto<MemberTicketSaleDto>> GetMemberTicketsForMobileAsync(GetMemberTicketsInput input);
        Task<CheckTicketOutput> GetLastCheckTicketInfoAsync(GetLastCheckTicketInfoInput input);
        Task<DynamicColumnResultDto> StatTouristByAgeRangeAsync(StatTouristByAgeRangeInput input);
        Task<DynamicColumnResultDto> StatTouristByAreaAsync(StatTouristByAreaInput input);
        Task<DynamicColumnResultDto> StatTouristBySexAsync(StatTouristBySexInput input);
        Task<PagedResultDto<TicketCheckListDto>> QueryTicketChecksAsync(QueryTicketCheckInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInByDateAndParkAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInByGateGroupAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInByGroundAndGateGroupAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckByGroundAndTimeAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatStadiumTicketCheckInAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckByTradeSourceAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInByTicketTypeAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInYearOverYearComparisonAsync(StatTicketCheckInInput input);
        Task<TicketCheckOverviewResult> GetTicketCheckOverviewAsync(GetTicketCheckOverviewInput input);
        Task<DataTable> GetStadiumTicketCheckOverviewAsync(GetTicketCheckOverviewInput input);
        Task<int> GetScenicCheckInQuantityAsync(GetTicketCheckOverviewInput input);
        Task<DynamicColumnResultDto> StatTicketCheckInAverageAsync(StatTicketCheckInInput input);
        Task<DynamicColumnResultDto> StatTicketSaleAsync(StatTicketSaleInput input);
        Task<DataTable> StatCashierSaleAsync(StatCashierSaleInput input);
        Task<DataTable> StatTicketSaleByTradeSourceAsync(StatTicketSaleByTradeSourceInput input);
        Task<byte[]> StatTicketSaleByPayTypeToExcelAsync(StatTicketSaleByPayTypeInput input);
        Task<DynamicColumnResultDto> StatTicketSaleByPayTypeAsync(StatTicketSaleByPayTypeInput input);
        Task<DataTable> StatTicketSaleBySalePointAsync(StatTicketSaleBySalePointInput input);
        Task<DataTable> StatTicketSaleGroundSharingAsync(StatGroundSharingInput input);
        Task<PagedResultDto<TicketReprintLogListDto>> QueryReprintLogsAsync(QueryReprintLogInput input);
        Task<PagedResultDto<TicketExchangeHistoryListDto>> QueryExchangeHistorysAsync(QueryExchangeHistoryInput input);
        Task<DataTable> StatExchangeHistoryJbAsync(StatJbInput input);
        Task<DataTable> StatTicketSaleByTicketTypeClassAsync(StatTicketSaleByTicketTypeClassInput input);
        Task<DataTable> StatTicketSaleJbAsync(StatJbInput input);
        Task<byte[]> StatGroundChangCiSaleToExcelAsync(StatGroundChangCiSaleInput input);
        Task<DynamicColumnResultDto> StatGroundChangCiSaleAsync(StatGroundChangCiSaleInput input);
        Task<byte[]> QueryTicketConsumesToExcelAsync(QueryTicketConsumeInput input);
        Task<PagedResultDto<TicketConsumeListDto>> QueryTicketConsumesAsync(QueryTicketConsumeInput input);
        Task<byte[]> StatTicketConsumeToExcelAsync(StatTicketConsumeInput input);
        Task<List<StatTicketConsumeListDto>> StatTicketConsumeAsync(StatTicketConsumeInput input);
    }
}
