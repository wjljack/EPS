using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using ExcelLibrary.SpreadSheet;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace HRSeat.CommonClass
{
    public class ExcelOperater
    {
        private Dictionary<UInt32, UInt32> dicColumnsWidth = new Dictionary<UInt32, UInt32>();
        private Workbook wb = null;
        public Worksheet currentWorkSheet = null;

        /// <summary>
        /// 将DataTable中的数据导出到Excel中的当前的Sheet(不包含列头)
        /// </summary>
        /// <param name="leftIndex">表格左边第一个单元格索引</param>
        /// <param name="topIndex">表格上方第一个单元格索引</param>
        /// <param name="workSheetName">Excel中的Sheet名称</param>
        /// <param name="tab">保存数据的datatable</param>
        /// <returns></returns>
        public void DataTableToExcelSheet(int leftIndex, int topIndex, string workSheetName, DataTable tab, string title, Enums.ExpType exptype)
        {
            if (workSheetName.Equals(string.Empty) || tab == null)
            {
                return;
            }
            if (topIndex < 0)
            {
                topIndex = 0;
            }
            if (leftIndex < 0)
            {
                leftIndex = 0;
            }
            Worksheet worksheet = GetWorkSheet(workSheetName);
            if (worksheet == null)
            {
                return;
            }


            //构建标题
            //MergeCells((ushort)(topIndex - 1), (ushort)(topIndex - 1), (ushort)(leftIndex), (ushort)(leftIndex + tab.Columns.Count));
            //worksheet.Cells[leftIndex, topIndex - 1] = new Cell(title);
            //构建列头
            for (int col = leftIndex; col < tab.Columns.Count; col++)
            {
                worksheet.Cells[topIndex, col + 1] = new Cell(tab.Columns[col - leftIndex].ColumnName);
            }


            int roomColIndex;

            worksheet.Cells[topIndex, 0] = new Cell("序号");

            topIndex++;
            //构建内容
            for (int rowIndex = topIndex; rowIndex < tab.Rows.Count + topIndex; rowIndex++)
            {
                for (int colIndex = leftIndex; colIndex < tab.Columns.Count + leftIndex; colIndex++)
                {
                    if (tab.Rows[rowIndex - topIndex][colIndex - leftIndex] != DBNull.Value)
                    {
                        worksheet.Cells[rowIndex, 0] = new Cell(rowIndex - 1);
                        Cell c = new Cell(tab.Rows[rowIndex - topIndex][colIndex - leftIndex]);
                        CellStyle cs = new CellStyle();
                        //if (tab.Rows[rowIndex - topIndex]["部门"].ToString() != "" && colIndex - leftIndex > 0)
                        if (exptype == Enums.ExpType.Dept)
                        {
                            if (tab.Rows[rowIndex - topIndex]["部门"].ToString() != "")
                            {
                                cs.BackColor = Color.FromArgb(51, 102, 255);
                                c.Style = cs;
                            }
                        }
                        worksheet.Cells[rowIndex, colIndex + 1] = c;
                    }
                    else
                    {
                        Cell c = new Cell("");
                        CellStyle cs = new CellStyle();
                        if (exptype == Enums.ExpType.Dept)
                        {
                            if (tab.Rows[rowIndex - topIndex]["部门"].ToString() != "" && colIndex - leftIndex > 0)
                            {
                                cs.BackColor = Color.FromArgb(51, 102, 255);
                                c.Style = cs;
                            }
                        }
                        worksheet.Cells[rowIndex, colIndex + 1] = c;
                    }
                }
                //设置默认列宽度
                if (worksheet.Cells.LastRowIndex > 1)
                {
                    for (int n = leftIndex; n < tab.Columns.Count + leftIndex; n++)
                    {
                        if (worksheet.Cells[topIndex, n].Value.ToString().Length >= 50)
                            worksheet.Cells.ColumnWidth[(ushort)n] = 10000;
                        else if (worksheet.Cells[topIndex, n].Value.ToString().Length >= 10)
                            worksheet.Cells.ColumnWidth[(ushort)n] = (ushort)(worksheet.Cells[topIndex, n].Value.ToString().Length * 300);
                        else
                            worksheet.Cells.ColumnWidth[(ushort)n] = 3000;
                    }
                }
                else
                {
                    worksheet.Cells.ColumnWidth[0, (ushort)(tab.Columns.Count - 1)] = 3000;
                }
            }
            //合并房间单元格
            MergeCells(0, 4, 0, 4);
            //if (worksheet.Cells[1, 2].ToString() == "房间")
            //{
            //    int i = 2, j = 2;
            //    while (i < worksheet.Cells.Rows.Count && j < worksheet.Cells.Rows.Count)
            //    {
            //        j = i + 1;
            //        while (worksheet.Cells[i, 2].ToString() == worksheet.Cells[j, 2].ToString())
            //        {
            //            j++;
            //        }
            //        if (j - 1 - i > 1)
            //        {
            //            MergeCells((ushort)i, (ushort)(j - 1), (ushort)2, (ushort)2);
            //        }
            //        i = j;
            //    }
            //}
        }

        private Worksheet GetWorkSheet(string strSheetName)
        {
            foreach (Worksheet ws in wb.Worksheets)
            {
                if ((strSheetName == null) || (ws.Name == strSheetName))
                    return ws;
            }
            return null;
        }

        public bool CreateExcelSheet(string sheetName)
        {
            try
            {
                if (wb == null)
                {
                    wb = new ExcelLibrary.SpreadSheet.Workbook();
                }
                if (wb == null)
                    return false;
                currentWorkSheet = GetWorkSheet(sheetName);
                if (currentWorkSheet == null)
                {
                    currentWorkSheet = new ExcelLibrary.SpreadSheet.Worksheet(sheetName);
                    wb.Worksheets.Add(currentWorkSheet);
                }
                if (currentWorkSheet == null)
                    return false;
                return true;
            }
            catch /*(System.Exception ex)*/
            {
                return false;
            }
        }

        public void Save(string fullFilePath)
        {
            if (wb != null)
                wb.Save(fullFilePath);
        }

        internal void SetColumnCustomWidth(int colIndex, ushort Width)
        {
            currentWorkSheet.Cells.ColumnWidth[(UInt16)colIndex, (UInt16)colIndex] = (ushort)(Width * 256);
        }

        internal bool AddContent(uint rowIndex, int colIndex, object oContent
                                    , string formulaText = "", string horizontalAlignment = "")
        {
            UInt16 row = (UInt16)rowIndex, col = (UInt16)colIndex;
            if (formulaText.Length > 0)
            {
                currentWorkSheet.Cells[row, col] = new ExcelLibrary.SpreadSheet.Cell("=" + formulaText);
            }
            else
                if (oContent.GetType() == typeof(string))
                {
                    currentWorkSheet.Cells[row, col] = new ExcelLibrary.SpreadSheet.Cell(Convert.ToString(oContent));
                }
                else if (oContent.GetType() == typeof(bool))
                {
                    bool bValue = Convert.ToBoolean(oContent);
                    currentWorkSheet.Cells[row, col] = new ExcelLibrary.SpreadSheet.Cell(bValue);
                }
                else if (oContent.GetType() == typeof(DateTime))
                {
                    currentWorkSheet.Cells[row, col] = new ExcelLibrary.SpreadSheet.Cell(oContent, @"YYYY\-MM\-DD");
                }
                else
                {
                    currentWorkSheet.Cells[row, col] = new ExcelLibrary.SpreadSheet.Cell(oContent);
                }
            /* if (horizontalAlignment.Length == 0)
             {
                 currentWorkSheet.Cells[row, col].
             }*/
            return false;
        }

        internal bool MergeCells(ushort firstRowIndex, ushort lastRowIndex,
                                 ushort firstColumnIndex, ushort lastColumnIndex)
        {
            if (currentWorkSheet == null)
                return false;
            currentWorkSheet.MergeCell((ushort)firstRowIndex, lastRowIndex, firstColumnIndex, lastColumnIndex);
            return true;
        }
    }
}