using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Fwk.HierarchicalItem {
    public class HierarchicalItem<T> {

        public Int64 Id { get; set; }

        public string Name { get; set; }

        public virtual List<HierarchicalItem<T>> Children { get; set; }

        public Boolean IsExpanded { get; set; }

        public Boolean IsSelected { get; set; }

        public String Info { get; set; }

        public HierarchicalItem() {
            this.Children = new List<HierarchicalItem<T>>();
        }
    }
}