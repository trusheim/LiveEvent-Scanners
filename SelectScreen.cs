using System;

using System.Collections.Generic;
using System.Text;

using Symbol.MT2000.UserInterface;

namespace SU_MT2000_SUIDScanner
{
    class SelectScreen : ListScreen
    {
        private ScrollableListItems items = null;

        public SelectScreen(ListForm listForm)
            : base(listForm)
        {
        }


        public override void Show()
        {
            listForm.TitleText = "Select Event";

            items = new ScrollableListItems();
            items.Add(new ScrollableListItem("asd",null,null,"asd"));
            items.Add(new ScrollableListItem("asd2", null, null, "asd2"));

            listForm.List.Items = items;
        }

        public override ListScreen ExecuteListItem(ScrollableListItem item)
        {
            if (item != null)
            {
                MsgBox.Show(listForm, "yep!", "selected " + (String)item.Tag);
            }

            // stay on this screen
            listForm.Close();
            return this;
        }

    }
}
