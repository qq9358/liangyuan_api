using Egoal.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Excel
{
    public static class ExcelHelper
    {
        public const int MaxRowCount = 1048576;

        public static Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string title, string memo)
        {
            return Task.Run(() =>
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data", "data不能为Null");
                }

                if (data.Count() > MaxRowCount - 3)
                {
                    throw new TmsException("数据过多");
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(title);

                    int columnIndex = 1;

                    var properties = typeof(T).GetProperties();
                    foreach (var property in properties)
                    {
                        var displayAttributes = property.GetCustomAttributes(typeof(DisplayAttribute), false);
                        if (displayAttributes.IsNullOrEmpty())
                        {
                            continue;
                        }

                        var displayAttribute = displayAttributes[0].As<DisplayAttribute>();
                        worksheet.Cells[2, columnIndex].Value = displayAttribute.Name;

                        int rowIndex = 3;
                        foreach (var row in data)
                        {
                            var value = property.GetValue(row);

                            if (value != null)
                            {
                                if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                                {
                                    value = value.To<bool>() ? "是" : "否";
                                }

                                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                {
                                    value = value.To<DateTime>().ToDateTimeString();
                                }
                            }

                            worksheet.Cells[rowIndex, columnIndex].Value = value;

                            rowIndex++;
                        }

                        columnIndex++;
                    }

                    SetHeaderAndFooter(worksheet, columnIndex - 1, data.Count(), title, memo);

                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);

                        return stream.ToArray();
                    }
                }
            });
        }

        public static Task<byte[]> ExportToExcelAsync(DataTable data, string title, string memo)
        {
            return Task.Run(() =>
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data", "data不能为Null");
                }

                if (data.Rows.Count > MaxRowCount - 3)
                {
                    throw new TmsException("数据过多");
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(title);

                    int columnIndex = 1;

                    foreach (DataColumn column in data.Columns)
                    {
                        worksheet.Cells[2, columnIndex].Value = column.ColumnName;

                        int rowIndex = 3;
                        foreach (DataRow row in data.Rows)
                        {
                            var value = row[column];
                            if (value.GetType() == typeof(DBNull))
                            {
                                if (column.DataType.IsValueType)
                                {
                                    value = Activator.CreateInstance(column.DataType);
                                }
                                else
                                {
                                    value = "0";
                                }
                            }
                            worksheet.Cells[rowIndex, columnIndex].Value = value;

                            rowIndex++;
                        }

                        columnIndex++;
                    }

                    SetHeaderAndFooter(worksheet, columnIndex - 1, data.Rows.Count, title, memo);

                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);

                        return stream.ToArray();
                    }
                }
            });
        }

        private static void SetHeaderAndFooter(ExcelWorksheet worksheet, int totalColumn, int totalRow, string title, string memo)
        {
            worksheet.Cells[1, 1].Value = title;
            using (var titleRange = worksheet.Cells[1, 1, 1, totalColumn])
            {
                titleRange.Merge = true;
                titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleRange.Style.Font.Size = 20;
                titleRange.Style.Font.Bold = true;
            }

            int memoRowIndex = totalRow + 3;
            worksheet.Cells[memoRowIndex, 1].Value = memo;
            worksheet.Cells[memoRowIndex, 1, memoRowIndex, totalColumn].Merge = true;

            worksheet.Cells.AutoFitColumns(0);
        }
    }
}
