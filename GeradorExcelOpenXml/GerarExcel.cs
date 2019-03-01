using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using OpenXmlPackaging = DocumentFormat.OpenXml.Packaging;
using OpenXmlSpread = DocumentFormat.OpenXml.Spreadsheet;

namespace GeradorExcelOpenXml
{
    public static class GerarExcel
    {
        public static void GerarExcelOpenXml<T>(List<T> list, string pathExcelFile, string sheetName,
            string ColunaDefault, int rowDefault,
            int larguraColuna)
        {
            try
            {
                System.Data.DataTable dt = ListToDataTable(list);
                GerarExcelOpXml(dt, pathExcelFile, sheetName, ColunaDefault, rowDefault, larguraColuna);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Criação da Planilha

        private static void GerarExcelOpXml(System.Data.DataTable dt, string pathExcelFile, string sheetName,
            string ColunaDefault, int rowDefault, int larguraColuna)
        {
            var styleIndexDefault = 0;
            var styleIndexDefaultLine = 2;

            DocumentFormat.OpenXml.UInt32Value styleIndex;

            using (OpenXmlPackaging.SpreadsheetDocument myDoc =
                OpenXmlPackaging.SpreadsheetDocument.Open(pathExcelFile, true))
            {
                OpenXmlPackaging.WorksheetPart worksheetPart = GetWorksheetPartByName(myDoc, sheetName);

                OpenXmlSpread.Cell cellDefault =
                    GetCell(worksheetPart.Worksheet, ColunaDefault, Convert.ToUInt32(rowDefault));
                styleIndex = cellDefault.StyleIndex;
                styleIndexDefault = Convert.ToInt32(styleIndex.Value.ToString());

                OpenXmlSpread.Cell cellDefaultline =
                    GetCell(worksheetPart.Worksheet, ColunaDefault, Convert.ToUInt32(rowDefault + 1));
                styleIndex = cellDefaultline.StyleIndex;
                styleIndexDefaultLine = Convert.ToInt32(styleIndex.Value.ToString());
            }

            using (OpenXmlPackaging.SpreadsheetDocument myDoc =
                OpenXmlPackaging.SpreadsheetDocument.Open(pathExcelFile, true))
            {
                OpenXmlPackaging.WorksheetPart worksheetPart = GetWorksheetPartByName(myDoc, sheetName);
                OpenXmlSpread.Stylesheet stylesheet = myDoc.WorkbookPart.WorkbookStylesPart.Stylesheet;

                OpenXmlSpread.CellFormat deafultFormat = new OpenXmlSpread.CellFormat()
                {
                    Alignment = new OpenXmlSpread.Alignment()
                    {
                        Horizontal = OpenXmlSpread.HorizontalAlignmentValues.Left,
                        Vertical = OpenXmlSpread.VerticalAlignmentValues.Center
                    },
                    ApplyAlignment = true
                };

                stylesheet.CellFormats.AppendChild(deafultFormat);

                DocumentFormat.OpenXml.OpenXmlWriter
                    writer = DocumentFormat.OpenXml.OpenXmlWriter.Create(worksheetPart);

                var indexColumn = ColumnIndex(ColunaDefault);
                var startColunaIndex = indexColumn + 1;

                writer.WriteStartElement(new OpenXmlSpread.Worksheet());
                writer.WriteStartElement(new OpenXmlSpread.Columns());

                AjustaLarguraColunas(writer, startColunaIndex, larguraColuna, dt.Columns.Count);

                writer.WriteStartElement(new OpenXmlSpread.SheetData());
                writer.WriteStartElement(new OpenXmlSpread.Row { RowIndex = (UInt32)rowDefault });

                CriarColunas(writer, dt, indexColumn, rowDefault, styleIndexDefault);

                CriarLinha(writer, dt, indexColumn, rowDefault, styleIndexDefaultLine);

                writer.Dispose();
            }
        }

        private static void AjustaLarguraColunas(DocumentFormat.OpenXml.OpenXmlWriter writer, int startColunaIndex,
            int larguraColuna, int ColunsCount)
        {
            var xmlCols = new List<DocumentFormat.OpenXml.OpenXmlAttribute>();
            xmlCols.Add(new DocumentFormat.OpenXml.OpenXmlAttribute("min", null, startColunaIndex.ToString()));
            xmlCols.Add(new DocumentFormat.OpenXml.OpenXmlAttribute("max", null,
                (startColunaIndex + ColunsCount).ToString()));
            xmlCols.Add(new DocumentFormat.OpenXml.OpenXmlAttribute("width", null, larguraColuna.ToString()));
            writer.WriteStartElement(new OpenXmlSpread.Column(), xmlCols);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        private static void CriarColunas(DocumentFormat.OpenXml.OpenXmlWriter writer, System.Data.DataTable dt,
            int indexColumn, int rowDefault, int styleIndexDefault)
        {
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                var reference = (GetExcelColumnName(indexColumn + col) + rowDefault.ToString());
                DataColumn cols = dt.Columns[col];

                writer.WriteElement(new OpenXmlSpread.Cell
                {
                    CellReference = reference,
                    CellValue = new OpenXmlSpread.CellValue(cols.ColumnName.ToUpper()),
                    StyleIndex = (UInt32)styleIndexDefault,
                    DataType = OpenXmlSpread.CellValues.String
                });
            }

            writer.WriteEndElement();
        }

        private static void CriarLinha(DocumentFormat.OpenXml.OpenXmlWriter writer, System.Data.DataTable dt,
            int indexColumn, int rowDefault, int styleSheetIndex)
        {
            foreach (DataRow dr in dt.Rows)
            {
                rowDefault++;
                writer.WriteStartElement(new OpenXmlSpread.Row { RowIndex = (UInt32)(rowDefault) });
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    var reference = (GetExcelColumnName(indexColumn + col) + rowDefault.ToString());
                    writer.WriteElement(new OpenXmlSpread.Cell
                    {
                        CellReference = reference,
                        CellValue = new OpenXmlSpread.CellValue(dr.ItemArray[col].ToString()),
                        DataType = new EnumValue<OpenXmlSpread.CellValues>(OpenXmlSpread.CellValues.Date),
                        StyleIndex = (UInt32)styleSheetIndex
                    });
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private static System.Data.DataTable ListToDataTable<T>(List<T> list)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(GetDescription(info), GetNullableType(info.PropertyType)));
            }

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[GetDescription(info)] = info.GetValue(t, null);
                    else
                        row[GetDescription(info)] = (info.GetValue(t, null) ?? DBNull.Value);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        public static string GetDescription(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(true);
            if (attributes.Count() > 0)
            {
                DescriptionAttribute da = attributes[0] as DescriptionAttribute;
                return da.Description;
            }
            else
                return info.Name.ToString();
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }

            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        private static OpenXmlPackaging.WorksheetPart GetWorksheetPartByName(
            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<OpenXmlSpread.Sheet> sheets =
                document.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>()
                    .Elements<OpenXmlSpread.Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
                throw new Exception(String.Format("A planilha {0} não existe", sheetName));

            string relationshipId = sheets.First().Id.Value;
            DocumentFormat.OpenXml.Packaging.WorksheetPart worksheetPart =
                (DocumentFormat.OpenXml.Packaging.WorksheetPart)
                document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            char CharA;
            char CharB;
            char CharC;

            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            if (columnIndex < 702)
            {
                CharA = (char)('A' + (columnIndex / 26) - 1);
                CharB = (char)('A' + (columnIndex % 26));

                return string.Format("{0}{1}", CharA, CharB);
            }

            int firstInt = columnIndex / 676;
            int secondInt = (columnIndex % 676) / 26;
            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt = firstInt - 1;
            }

            int thirdInt = (columnIndex % 26);

            CharA = (char)('A' + firstInt - 1);
            CharB = (char)('A' + secondInt - 1);
            CharC = (char)('A' + thirdInt);

            return string.Format("{0}{1}{2}", CharA, CharB, CharC);
        }

        private static int ColumnIndex(string reference)
        {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 65);
            return ci;
        }

        private static OpenXmlSpread.Cell GetCell(DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet,
            string columnName, uint rowIndex)
        {
            OpenXmlSpread.Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<OpenXmlSpread.Cell>()
                .Where(c => string.Compare(c.CellReference.Value, columnName + rowIndex, true) == 0).First();
        }

        private static OpenXmlSpread.Row GetRow(DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<OpenXmlSpread.SheetData>().Elements<OpenXmlSpread.Row>()
                .Where(r => r.RowIndex == rowIndex).First();
        }
        
        #endregion
    }
}