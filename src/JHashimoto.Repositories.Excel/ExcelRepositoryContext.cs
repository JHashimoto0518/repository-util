using ClosedXML.Excel;
using JHashimoto.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using JHashimoto.Infrastructure.Diagnostics;

namespace JHashimoto.Repositories.Excel {
    public class ExcelRepositoryContext : IDisposable {
        public string FileFullName { get; }

        private readonly XLWorkbook workBook;

        private IXLWorksheet worksheet;

        public string WorksheetName {
            set {
                Guard.ArgumentNotNullOrWhiteSpace(value, "value");

                this.worksheet = this.workBook.Worksheet(value);
            }
        }

        public ExcelRepositoryContext(string fileFullName, string sheetName) {
            Guard.ArgumentNotNullOrWhiteSpace(fileFullName, "fileFullName");
            Guard.ArgumentNotNullOrWhiteSpace(sheetName, "sheetName");
            Guard.Assert(File.Exists(fileFullName), $"{fileFullName}は存在しません。");

            this.FileFullName = fileFullName;

            // TODO:ファイルを開いている時のエラー
            this.workBook = new XLWorkbook(fileFullName, XLEventTracking.Disabled);
            this.WorksheetName = sheetName;
        }

        public T GetCellValue<T>(string cellAddress) {
            Guard.ArgumentNotNullOrWhiteSpace(cellAddress, "cellAddress");

            return this.worksheet.Cell(cellAddress).GetValue<T>();
        }

        public IEnumerable<T> GetCellValues<T>(int startRowIndex) where T : new() {
            Guard.ArgumentIsPositive(startRowIndex, "startRowIndex");

            ExcelKeyColumnAttribute keyColumnIndexAttr = typeof(T).GetCustomAttributes(typeof(ExcelKeyColumnAttribute), false).Cast<ExcelKeyColumnAttribute>().FirstOrDefault();

            // TODO:ExcelKeyColumnIndexAttributeない時例外をスロー

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            int lastRowIndex = this.worksheet.LastRowUsed().RowNumber();
            for (int i = startRowIndex; i <= lastRowIndex; i++) {
                IXLRow row = this.worksheet.Row(i);

                if (row.Cell(keyColumnIndexAttr.ColumnIndex).IsEmpty())
                    break;

                T value = new T();
                foreach (PropertyInfo property in properties) {
                    ExcelCellValueAttribute attr = property.GetCustomAttributes(typeof(ExcelCellValueAttribute), false).Cast<ExcelCellValueAttribute>().FirstOrDefault();
                    if (attr == null) {
                        attr = property.GetAttributeFromMetaData<ExcelCellValueAttribute>();
                    }

                    if (attr != null) {
                        string s = row.Cell(attr.ColumnIndex).GetValue<string>();
                        property.SetValue(value, s, null);
                        continue;
                    }

                    ExcelCellInternallHyperlinkAttribute linkAttr = null;
                    linkAttr = property.GetCustomAttributes(typeof(ExcelCellInternallHyperlinkAttribute), false).Cast<ExcelCellInternallHyperlinkAttribute>().FirstOrDefault();
                    if (linkAttr == null) {
                        linkAttr = property.GetAttributeFromMetaData<ExcelCellInternallHyperlinkAttribute>();
                    }

                    if (linkAttr == null)
                        continue;

                    IXLCell linkCell = row.Cell(linkAttr.ColumnIndex);
                    if (linkCell.HasHyperlink == false)
                        continue;

                    XLHyperlink link = linkCell.Hyperlink;

                    if (link.IsExternal)
                        continue;

                    property.SetValue(value, new ExcelInternalHyperlink(link.Cell.GetValue<string>(), link.InternalAddress), null);
                }

                yield return value;
            }
        }

        #region 終了処理

        /// <summary>
        /// <see cref="ExcelRepositoryContext"/>オブジェクトがガベージコレクションにより収集される前に、<see cref="ExcelRepositoryContext"/>がリソースを解放し、
        /// その他のクリーンアップ操作を実行できるようにします。
        /// </summary>
        ~ExcelRepositoryContext() {
            Dispose();
        }

        /// <summary>
        /// 既にDisoseが呼ばれた場合は<c>true</c>。まだ呼ばれていない場合は<c>false</c>。
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        /// <param name="disposing">
        /// マネージ リソースとアンマネージ リソースの両方を解放する場合は<c>true</c>。アンマネージ リソースだけを解放する場合は<c>false</c>。
        /// </param>
        private void Dispose(bool disposing) {
            if (disposed) {
                return;
            }

            // アンマネージリソースを解放する。
            this.workBook?.Dispose();

            disposed = true;
        }

        #endregion 終了処理

    }
}
