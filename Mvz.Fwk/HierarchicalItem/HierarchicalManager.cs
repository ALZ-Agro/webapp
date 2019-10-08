using ALZAGRO.AppRendicionGastos.Fwk.HierarchicalItem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ALZAGRO.AppRendicionGastos.Application {

    public class HierarchicalManager {

        public List<HierarchicalItem<Int64>> GetItems(List<ItemInfo> sourceList) {
            var targetList = new List<HierarchicalItem<Int64>>();

            var rootItems = sourceList.Where(x => x.ParentId == null).ToList();

            foreach (ItemInfo item in rootItems) {
                var rootItem = CreateItem(item, false);
                targetList.Add(rootItem);

                this.AddItemToParent(rootItem, sourceList);
            }

            return targetList;
        }
        private void AddItemToParent(HierarchicalItem<Int64> parentItem, List<ItemInfo> sourceList) {

            var childItems = sourceList.Where(x => x.ParentId == parentItem.Id).ToList();

            if (childItems != null && childItems.Count > 0) {
                foreach (ItemInfo item in childItems) {
                    var hierarchicalItem = CreateItem(item, false);
                    parentItem.Children.Add(hierarchicalItem);

                    this.AddItemToParent(hierarchicalItem, sourceList);
                }
            }
        }

        private static HierarchicalItem<Int64> CreateItem(ItemInfo child, Boolean Selected) {

            var newItem = new HierarchicalItem<Int64>();
            newItem.Id = child.Id;
            newItem.Name = child.Description;
            newItem.Info = child.Info;
            newItem.IsExpanded = true;
            newItem.IsSelected = Selected;
            newItem.Children = new List<HierarchicalItem<Int64>>();

            return newItem;
        }
    }
}