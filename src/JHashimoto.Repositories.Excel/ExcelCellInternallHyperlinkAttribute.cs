using System.Diagnostics.Contracts;

namespace JHashimoto.Repositories.Excel {
    public class ExcelCellInternallHyperlinkAttribute : ExcelCellAttributeBase {
        public ExcelCellInternallHyperlinkAttribute(int columnIndex) : base(columnIndex) {
        }
    }
}
