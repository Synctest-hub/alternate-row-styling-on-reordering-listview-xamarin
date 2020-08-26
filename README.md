# How to update alternate row styling on reordering in Xamarin.Forms ListView (SfListView)

You can update the alternate row style for [ListViewItem](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.ListViewItem.html)safter reordering in Xamarin.Forms [SfListView](https://help.syncfusion.com/xamarin/listview/overview).

You can also refer the following article.

https://www.syncfusion.com/kb/11906/how-to-update-alternate-row-styling-on-reordering-in-xamarin-forms-listview-sflistview

**C#**

You can get the index of the Items from the [DataSource.DisplayItems](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.DataSource.Portable~Syncfusion.DataSource.DisplayItems.html) collection and return the **BackgroundColor** based on the item index.

``` c#
namespace ListViewXamarin 
{
    public class IndexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var listview = parameter as SfListView;
            var index = listview.DataSource.DisplayItems.IndexOf(value);

            return index % 2 == 0 ? Color.FromHex("#ffdbf6") : Color.FromHex("#e2fcff");
        }
    }
}
```

**XAML**

Converter bound to the [BackgroundColor](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.visualelement.backgroundcolor) property of the elements loaded inside the [SfListView.ItemTemplate](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.SfListView~ItemTemplate.html).

``` xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:ListViewXamarin"
             xmlns:syncfusion="clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms"
             x:Class="ListViewXamarin.MainPage">
    <ContentPage.BindingContext>
        <local:ContactsViewModel/>
    </ContentPage.BindingContext>
    <ContentPage.Behaviors>
        <local:Behavior/>
    </ContentPage.Behaviors>
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:IndexToColorConverter x:Key="IndexToColorConverter"/>
        </ResourceDictionary>
    </ContentPage.Resources>
    
    <ContentPage.Content>
        <StackLayout>
            <syncfusion:SfListView x:Name="listView" ItemsSource="{Binding ContactsInfo}" ItemSize="50" DragStartMode="OnHold">
                <syncfusion:SfListView.ItemTemplate>
                    <DataTemplate>
                        <Grid Padding="10,0,0,0" BackgroundColor="{Binding ., Converter={StaticResource IndexToColorConverter}, ConverterParameter={x:Reference Name=listView}}">
                            <Label LineBreakMode="NoWrap" VerticalTextAlignment="End" Text="{Binding ContactName}"/>
                            <Label Grid.Row="1" VerticalTextAlignment="Start" Text="{Binding ContactNumber}"/>
                        </Grid>
                    </DataTemplate>
                </syncfusion:SfListView.ItemTemplate>
            </syncfusion:SfListView>
        </StackLayout>
    </ContentPage.Content>
</ContentPage>
```

**C#**

The [SfListView.ItemDragging](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.SfListView~ItemDragging_EV.html) event is triggered to update the alternate row styling after reordering the item using [RefreshListViewItem](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.SfListView~RefreshListViewItem.html) method in the [DropAction.Drop](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.ItemDraggingEventArgs~Action.html) action. You need to set the [UpdateSource](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.DragDropController~UpdateSource.html) property as **true** , to update the collection after reordering.

``` c#
namespace ListViewXamarin
{
    public class Behavior : Behavior<ContentPage>
    {
        SfListView ListView;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            ListView = bindable.FindByName<SfListView>("listView");
            ListView.DragDropController.UpdateSource = true;
            ListView.ItemDragging += ListView_ItemDragging;
            base.OnAttachedTo(bindable);
        }

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
    }
}
```

**Output**

![AlternateRowStyleReOrdering](https://github.com/SyncfusionExamples/alternate-row-styling-on-reordering-listview-xamarin/blob/master/ScreenShot/AlternateRowStyleReOrdering.gif)
