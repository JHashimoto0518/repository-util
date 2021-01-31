using System.Diagnostics.Contracts;

namespace JHashimoto.Repositories.Excel {
    public class ExcelCellValueAttribute : ExcelCellAttributeBase {
        public ExcelCellValueAttribute(int columnIndex) : base(columnIndex) {
        }
    }
}
