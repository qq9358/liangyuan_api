using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Egoal.Extensions
{
    public static class DataTableExtensions
    {
        public static void RowSum(this DataTable table, int sumTextIndex = 0, string sumText = "合计", params string[] excludeColumns)
        {
            DataRow totalRow = table.NewRow();

            foreach (DataColumn column in table.Columns)
            {
                if (excludeColumns.Any(c => c.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                decimal total = 0;
                foreach (DataRow row in table.Rows)
                {
                    decimal.TryParse(row[column].ToString(), out decimal value);
                    total += value;
                }
                totalRow[column] = Convert.ChangeType(total, column.DataType);
            }

            totalRow[sumTextIndex] = sumText;
            table.Rows.Add(totalRow);
        }

        public static void ColumnSum(this DataTable table, string sumText = "合计", params string[] excludeColumns)
        {
            DataColumn totalColumn = new DataColumn();
            totalColumn.ColumnName = sumText;

            List<string> sumColumns = new List<string>();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (i == 0 && excludeColumns.Length == 0)
                {
                    continue;
                }

                DataColumn column = table.Columns[i];
                if (excludeColumns.Any(c => c.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                totalColumn.DataType = column.DataType;
                sumColumns.Add(column.ColumnName);
            }
            table.Columns.Add(totalColumn);

            foreach (DataRow row in table.Rows)
            {
                decimal total = 0;
                foreach (var column in sumColumns)
                {
                    decimal.TryParse(row[column].ToString(), out decimal value);
                    total += value;
                }
                row[totalColumn.ColumnName] = Convert.ChangeType(total, totalColumn.DataType);
            }
        }

        public static void RemoveEmptyColumn(this DataTable table)
        {
            var rows = table.Rows.Cast<DataRow>();
            var emptyColumns = table.Columns.Cast<DataColumn>().Where(column => rows.All(r => r[column.ColumnName].ToString().IsNullOrEmpty())).ToList();
            emptyColumns.ForEach(column => table.Columns.Remove(column));
        }

        public static bool IsNullOrEmpty(this DataTable table)
        {
            return table == null || table.Rows.Count <= 0;
        }
    }
}
