using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LY.FileHelper
{
    public class ExcelHelper
    {

        public ExcelHelper() { }

        /// <summary>
        /// 文件流初始化对象
        /// </summary>
        /// <param name="stream"></param>
        public ExcelHelper(Stream stream)
        {
            _IWorkbook = CreateWorkbook(stream);
        }

        /// <summary>
        /// 传入文件名
        /// </summary>
        /// <param name="fileName"></param>
        public ExcelHelper(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
            {
                _IWorkbook = CreateWorkbook(fileStream);
            }
        }

        /// <summary>
        /// 工作薄
        /// </summary>
        private IWorkbook _IWorkbook;

        /// <summary>
        /// 创建工作簿对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IWorkbook CreateWorkbook(Stream stream)
        {
            try
            {
                return new XSSFWorkbook(stream); //07
            }
            catch
            {
                return new HSSFWorkbook(stream); //03
            }

        }

        /// <summary>
        /// 把Sheet中的数据转换为DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private DataTable ExportToDataTable(ISheet sheet)
        {
            DataTable dt = new DataTable();

            //默认，第一行是字段
            IRow headRow = sheet.GetRow(0);

            //设置datatable字段
            for (int i = headRow.FirstCellNum, len = headRow.LastCellNum; i < len; i++)
            {
                dt.Columns.Add(headRow.Cells[i].StringCellValue);
            }
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
            {
                IRow tempRow = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                //遍历一行的每一个单元格
                for (int r = 0, j = tempRow.FirstCellNum, len2 = tempRow.LastCellNum; j < len2; j++, r++)
                {

                    ICell cell = tempRow.GetCell(j);

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[r] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[r] = cell.NumericCellValue;
                                break;
                            case CellType.Boolean:
                                dataRow[r] = cell.BooleanCellValue;
                                break;
                            default: dataRow[r] = "ERROR";
                                break;
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// Sheet中的数据转换为List集合
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private IList<T> ExportToList<T>(ISheet sheet, string[] fields) where T : class,new()
        {
            IList<T> list = new List<T>();

            //遍历每一行数据
            for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);

                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    object cellValue = null;

                    if (cell != null)
                    {

                        switch (cell.CellType)
                        {
                            case CellType.String: //文本
                                cellValue = cell.StringCellValue;
                                break;
                            case CellType.Numeric: //数值
                                cellValue = cell.NumericCellValue.ToString();//Double转换为int
                                break;
                            case CellType.Boolean: //bool
                                cellValue = cell.BooleanCellValue;
                                break;
                            case CellType.Blank: //空白
                                cellValue = "";
                                break;
                            default: cellValue = "ERROR";
                                break;
                        }
                    }

                    typeof(T).GetProperty(fields[j]).SetValue(t, cellValue, null);
                }
                list.Add(t);
            }

            return list;
        }

        /// <summary>
        /// 获取第一个Sheet的第X行，第Y列的值。起始点为1
        /// </summary>
        /// <param name="X">行</param>
        /// <param name="Y">列</param>
        /// <returns></returns>
        public string GetCellValue(int X, int Y)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(0);

            IRow row = sheet.GetRow(X - 1);

            return row.GetCell(Y - 1).ToString();
        }

        /// <summary>
        /// 获取一行的所有数据
        /// </summary>
        /// <param name="X">第x行</param>
        /// <returns></returns>
        public string[] GetCells(int X)
        {
            List<string> list = new List<string>();

            ISheet sheet = _IWorkbook.GetSheetAt(0);

            IRow row = sheet.GetRow(X - 1);

            for (int i = 0, len = row.LastCellNum; i < len; i++)
            {
                list.Add(row.GetCell(i).StringCellValue);//这里没有考虑数据格式转换，会出现bug
            }
            return list.ToArray();
        }

        /// <summary>
        /// 第一个Sheet数据，转换为DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ExportExcelToDataTable()
        {
            return ExportToDataTable(_IWorkbook.GetSheetAt(0));
        }

        /// <summary>
        /// 第sheetIndex表数据，转换为DataTable
        /// </summary>
        /// <param name="sheetIndex">第几个Sheet，从1开始</param>
        /// <returns></returns>
        public DataTable ExportExcelToDataTable(int sheetIndex)
        {
            return ExportToDataTable(_IWorkbook.GetSheetAt(sheetIndex - 1));
        }


        /// <summary>
        /// Excel中默认第一张Sheet导出到集合
        /// </summary>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <returns></returns>
        public IList<T> ExcelToList<T>(string[] fields) where T : class,new()
        {
            return ExportToList<T>(_IWorkbook.GetSheetAt(0), fields);
        }

        /// <summary>
        /// Excel中指定的Sheet导出到集合
        /// </summary>
        /// <param name="sheetIndex">第几张Sheet,从1开始</param>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <returns></returns>
        public IList<T> ExcelToList<T>(int sheetIndex, string[] fields) where T : class,new()
        {
            return ExportToList<T>(_IWorkbook.GetSheetAt(sheetIndex - 1), fields);
        }

        public static string GetDisplayName(Type modelType, string propertyDisplayName)
        {
            return (System.ComponentModel.TypeDescriptor.GetProperties(modelType)[propertyDisplayName].Attributes[typeof(System.ComponentModel.DisplayNameAttribute)] as System.ComponentModel.DisplayNameAttribute).DisplayName;
        }


        public static void ExportListToExcel<T>(List<T> datas, string filename, string[] columns)
        {
            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new HSSFWorkbook();


            ISheet sheet = workbook.CreateSheet("data");
            IRow headerRow = sheet.CreateRow(0);

            int rowIndex = 1, piIndex = 0;
            Type type = typeof(T);
            PropertyInfo[] pis = type.GetProperties();
            int pisLen = pis.Length;//减2是多了2个外键引用  
            PropertyInfo pi = null;
            string displayName = string.Empty;
            while (piIndex < pisLen)
            {
                pi = pis[piIndex];

                //  if (!displayName.Equals(string.Empty))
                //  {//如果该属性指定了DisplayName，则输出  
                try
                {
                    //headerRow.CreateCell(piIndex).SetCellValue(displayName);
                    ICell cell = headerRow.CreateCell(piIndex);
                    cell.SetCellValue(columns[piIndex]);
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                    // SetCellRangeAddress(sheet, headerRow.RowNum, headerRow.RowNum, piIndex,piIndex+1);
                }
                catch (Exception)
                {
                    headerRow.CreateCell(piIndex).SetCellValue("");
                }
                //}
                piIndex++;
            }
            foreach (T data in datas)
            {
                piIndex = 0;
                IRow dataRow = sheet.CreateRow(rowIndex);
                while (piIndex < pisLen - 1)
                {
                    pi = pis[piIndex + 1];
                    try
                    {
                        dataRow.CreateCell(piIndex).SetCellValue(pi.GetValue(data, null).ToString());
                    }
                    catch (Exception)
                    {
                        dataRow.CreateCell(piIndex).SetCellValue("");
                    }
                    piIndex++;
                }
                rowIndex++;
            }

            for (int k = 0; k < pisLen; k++)
            {
                sheet.SetColumnWidth(k, 4000);
            }

            workbook.Write(ms);
            FileStream dumpFile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
            ms.WriteTo(dumpFile);
            ms.Flush();
            ms.Position = 0;
            dumpFile.Close();

        }
  
    }
}
