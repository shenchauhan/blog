using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDrop
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		public ObservableCollection<MyItem> MyItems { get; private set; } = new ObservableCollection<MyItem>();

		public ObservableCollection<MyItem> Likes { get; private set; } = new ObservableCollection<MyItem>();

		public ObservableCollection<MyItem> Dislikes { get; private set; } = new ObservableCollection<MyItem>();

		private void UnorganizedListView_OnDragOver(object sender, DragEventArgs e)
		{
			e.AcceptedOperation = DataPackageOperation.Copy;
		}

		private async void UnorganizedListView_OnDrop(object sender, DragEventArgs e)
		{
			if (e.DataView.Contains(StandardDataFormats.StorageItems))
			{
				var storageItems = await e.DataView.GetStorageItemsAsync();

				foreach (StorageFile storageItem in storageItems)
				{
					var bitmapImage = new BitmapImage();
					await bitmapImage.SetSourceAsync(await storageItem.OpenReadAsync());

					var myItem = new MyItem
					{
						Id = Guid.NewGuid(),
						Image = bitmapImage
					};

					this.MyItems.Add(myItem);
				}
			}
		}

		private void UnorganizedListView_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
		{
			var items = string.Join(",", e.Items.Cast<MyItem>().Select(i => i.Id));
			e.Data.SetText(items);
			e.Data.RequestedOperation = DataPackageOperation.Move;
		}

		private void ListView_DragOver(object sender, DragEventArgs e)
		{
			if (e.DataView.Contains(StandardDataFormats.Text))
			{
				e.AcceptedOperation = DataPackageOperation.Move;
			}
		}

		private async void ListView_Drop(object sender, DragEventArgs e)
		{
			if (e.DataView.Contains(StandardDataFormats.Text))
			{
				var id = await e.DataView.GetTextAsync();
				var itemIdsToMove = id.Split(',');

				var destinationListView = sender as ListView;
				var listViewItemsSource = destinationListView?.ItemsSource as ObservableCollection<MyItem>;

				if (listViewItemsSource != null)
				{
					foreach (var itemId in itemIdsToMove)
					{
						var itemToMove = this.MyItems.First(i => i.Id.ToString() == itemId);

						listViewItemsSource.Add(itemToMove);
						this.MyItems.Remove(itemToMove);
					}
				}
			}
		}
	}
}