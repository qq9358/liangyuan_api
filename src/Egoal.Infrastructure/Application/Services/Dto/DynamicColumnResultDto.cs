using System.Collections.Generic;
using System.Data;

namespace Egoal.Application.Services.Dto
{
    public class DynamicColumnResultDto
    {
        public DynamicColumnResultDto(DataTable table)
        {
            Columns = new List<string>();
            if (table != null)
            {
                foreach (DataColumn column in table.Columns)
                {
                    Columns.Add(column.ColumnName);
                }

                Data = table;
            }
        }

        public List<string> Columns { get; }
        public DataTable Data { get; }
    }
}
