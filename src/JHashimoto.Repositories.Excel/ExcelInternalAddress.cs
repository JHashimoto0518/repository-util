using JHashimoto.Infrastructure.Diagnostics;
using JHashimoto.Infrastructure.Extensions;
using JHashimoto.Infrastructure.Text;
using System.Diagnostics.Contracts;

namespace JHashimoto.Repositories.Excel {
    public class ExcelInternalAddress {

        public string SheetName { get; }

        public string Cell { get; }

        public ExcelInternalAddress(string address) {
            Guard.ArgumentNotNullOrWhiteSpace(address, "address");

            StringDivider addressDivider = new StringDivider(address, divider: "!");

            this.SheetName = addressDivider.Left.TrimStartOnce("'").TrimEndOnce("'");
            this.Cell = addressDivider.Right;
        }
    }
}
