using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ListViewXamarin
{
    public class Behavior : Behavior<ContentPage>
    {
        #region Field

        SfListView ListView;
        #endregion

        #region Overrides

        protected override void OnAttachedTo(ContentPage bindable)
        {
            ListView = bindable.FindByName<SfListView>("listView");
            ListView.DragDropController.UpdateSource = true;
            ListView.ItemDragging += ListView_ItemDragging;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            ListView.ItemDragging -= ListView_ItemDragging;
            ListView = null;
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region Events

        private void ListView_ItemDragging(object sender, ItemDraggingEventArgs e)
        {
            if (e.Action == DragAction.Drop)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (e.NewIndex < e.OldIndex)
                    {
                        ListView.RefreshListViewItem(-1, -1, true);
                    }
                });
            }
        }
        #endregion
    }
}
