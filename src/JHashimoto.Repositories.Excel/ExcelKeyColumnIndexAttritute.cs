using JHashimoto.Infrastructure.Diagnostics;
using System;
using System.Diagnostics.Contracts;

namespace JHashimoto.Repositories.Excel {
    [AttributeUsage(AttributeTargets.Class)]
    public class ExcelKeyColumnAttribute : Attribute {
        public int ColumnIndex { get; }

        public ExcelKeyColumnAttribute(int columnIndex) {
            Guard.ArgumentIsPositive(columnIndex, "columnIndex");

            this.ColumnIndex = columnIndex;
        }
    }
}
