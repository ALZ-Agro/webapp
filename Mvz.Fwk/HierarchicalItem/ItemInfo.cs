using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.HierarchicalItem {
    public class ItemInfo {

        public Int64 Id { get; set; }

        public String Description { get; set; }

        public Int64? ParentId { get; set; }

        public String Info { get; set; }
    }
}