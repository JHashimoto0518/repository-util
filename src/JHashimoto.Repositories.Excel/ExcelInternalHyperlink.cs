using JHashimoto.Infrastructure.Diagnostics;
using System.Diagnostics.Contracts;

namespace JHashimoto.Repositories.Excel {
    public class ExcelInternalHyperlink {
        public string Value { get; }

        public ExcelInternalAddress Address { get; }

        public ExcelInternalHyperlink(string value, string address) {
            Guard.ArgumentNotNull<string>(value, "value");
            Guard.ArgumentNotNullOrWhiteSpace(address, "address");

            this.Value = value;
            this.Address = new ExcelInternalAddress(address);
        }
    }
}
