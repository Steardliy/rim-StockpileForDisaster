using System.Linq;
using UnityEngine;
using Verse;

namespace StockpileForDisaster
{
    class Listing_EntityListUI : Listing_Tree
    {
        private IEntityFilter filter;
        
        public Listing_EntityListUI(IEntityFilter filter)
        {
            this.filter = filter;
        }

        public void CreateCheckBoxUI(EntityCategory root, int openMask, int indentLevel)
        {
            DoCurrent(root, openMask, indentLevel);
        }

        private void DoCurrent(EntityCategory current, int openMask, int indentLevel)
        {
            foreach (var curSpFilter in current.catDef.childSpecialEntityFilters)
            {
                this.DoSpecialFilter(curSpFilter, indentLevel);
            }
            foreach (var curChildCategory in current.childCategories)
            {
                this.DoCategory(curChildCategory, openMask, indentLevel);
            }
            foreach (var curThing in current.childThings)
            {
                this.DoThing(curThing, indentLevel);
            }
        }
        private void DoSpecialFilter(SpecialEntityFilterDef spFilter,int indentLevel)
        {
            base.LabelLeft("*" + spFilter.LabelCap, spFilter.description, indentLevel);
            bool flag = this.filter.IsAllowed(spFilter);
            bool flag2 = flag;
            Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false);
            if(flag != flag2)
            {
                this.filter.SetAllow(spFilter, flag);
            }
            base.EndLine();
        }
        private void DoCategory(EntityCategory node,int openMask, int indentLevel)
        {
            if (!node.DescendantThings.Any())
            {
                return;
            }
            base.OpenCloseWidget(node, indentLevel, openMask);
            base.LabelLeft(node.LabelCap, node.Description, indentLevel);
            MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
            if (Widgets.CheckboxMulti(new Vector2(this.LabelWidth, this.curY), multiCheckboxState, this.lineHeight))
            {
                bool allow = multiCheckboxState == MultiCheckboxState.Off;
                foreach(var thing in node.DescendantThings)
                {
                    this.filter.SetAllow(thing, allow);
                }
                foreach(var spFilter in node.catDef.DescendantSpecialEntityFilterDefs)
                {
                    this.filter.SetAllow(spFilter, allow);
                }
            }
            base.EndLine();
            if (node.IsOpen(openMask))
            {
                this.DoCurrent(node, openMask, indentLevel + 1);
            }
        }
        private void DoThing(Thing thing, int indentLevel)
        {
            base.LabelLeft(thing.LabelCap, thing.GetDescription(), indentLevel);
            
            bool flag = this.filter.IsAllowed(thing);
            bool flag2 = flag;
            Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false);
            if (flag != flag2)
            {
                this.filter.SetAllow(thing, flag);
            }
            base.EndLine();
        }
        private MultiCheckboxState AllowanceStateOf(EntityCategory cat)
        {
            int num = 0;
            int num2 = 0;
            foreach (Thing current in cat.DescendantThings)
            {
                num++;
                if (this.filter.IsAllowed(current))
                {
                    num2++;
                }
            }
            foreach (SpecialEntityFilterDef current in cat.catDef.DescendantSpecialEntityFilterDefs)
            {
                num++;
                if (this.filter.IsAllowed(current))
                {
                    num2++;
                }
            }
            if (num2 == 0)
            {
                return MultiCheckboxState.Off;
            }
            if (num == num2)
            {
                return MultiCheckboxState.On;
            }
            return MultiCheckboxState.Partial;
        }
    }
}
