using System;
using System.Diagnostics.Contracts;
using JHashimoto.Infrastructure.Diagnostics;

namespace JHashimoto.Repositories.Excel {
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ExcelCellAttributeBase : Attribute {
        public int ColumnIndex { get; }

        public ExcelCellAttributeBase(int columnIndex) {
            Guard.ArgumentIsPositive(columnIndex, "columnIndex");

            this.ColumnIndex = columnIndex;
        }
    }

}
