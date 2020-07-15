using System.Data;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class StatTicketCheckListDto
    {
        public string ground_name { get; set; }
        public string gate_type { get; set; }
        public string before_09 { get; set; }
        public string time_09_10 { get; set; }
        public string time_10_11 { get; set; }
        public string time_11_12 { get; set; }
        public string time_12_13 { get; set; }
        public string time_13_14 { get; set; }
        public string time_14_15 { get; set; }
        public string time_15_16 { get; set; }
        public string time_16_17 { get; set; }
        public string time_17_18 { get; set; }
        public string time_18_19 { get; set; }
        public string time_19_20 { get; set; }
        public string time_20_21 { get; set; }
        public string after_21 { get; set; }

        public static StatTicketCheckListDto FromDataRow(DataRow row)
        {
            var dto = new StatTicketCheckListDto();
            dto.ground_name = row["检票区域"].ToString();
            dto.gate_type = row["出入类型"].ToString();
            dto.before_09 = row["9点前"].ToString();
            dto.time_09_10 = row["09"].ToString();
            dto.time_10_11 = row["10"].ToString();
            dto.time_11_12 = row["11"].ToString();
            dto.time_12_13 = row["12"].ToString();
            dto.time_13_14 = row["13"].ToString();
            dto.time_14_15 = row["14"].ToString();
            dto.time_15_16 = row["15"].ToString();
            dto.time_16_17 = row["16"].ToString();
            dto.time_17_18 = row["17"].ToString();
            dto.time_18_19 = row["18"].ToString();
            dto.time_19_20 = row["19"].ToString();
            dto.time_20_21 = row["20"].ToString();
            dto.after_21 = row["21点后"].ToString();

            return dto;
        }
    }
}
