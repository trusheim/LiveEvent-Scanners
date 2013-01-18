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
            listForm.LeftSoftKeyText = "";
            listForm.RightSoftKeyText = "";

            AdmitListInfo[] infos = AdmitList.GetAllAdmitLists();

            items = new ScrollableListItems();
            foreach (AdmitListInfo info in infos)
            {
                items.Add(new ScrollableListItem(info.eventName, null, null, info.filePath));
            }

            listForm.List.Items = items;

        }

        public override ListScreen ExecuteListItem(ScrollableListItem item)
        {
            if (item != null)
            {
                MainForm.ChangeAdmitListFile(item.Tag.ToString());
            }

            // return back to the other form
            listForm.Close();
            return this;
        }

    }
}
