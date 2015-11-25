using PropertyChanged;
using SQLite.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SQLite
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	[ImplementPropertyChanged]
	public sealed partial class MainPage : Page, INotifyPropertyChanged
	{
		private string path;

		public event PropertyChangedEventHandler PropertyChanged;

		public MainPage()
		{
			this.InitializeComponent();
			this.path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
		}

		public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			using (var connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
			{
				if (TableExists<User>(connection))
				{
					this.Users = new ObservableCollection<User>(connection.Table<User>().Select(i => i));
				}
			}
		}

		private void RemoveTableButton_Click(object sender, RoutedEventArgs e)
		{
			using (var connection = new SQLiteConnection(new Net.Platform.WinRT.SQLitePlatformWinRT(), path))
			{
				connection.DropTable<User>();
				this.Users.Clear();
			}
		}

		private void CreateUserButton_Click(object sender, RoutedEventArgs e)
		{
			using (var connection = new SQLiteConnection(new Net.Platform.WinRT.SQLitePlatformWinRT(), path))
			{
				if (!TableExists<User>(connection))
				{
					connection.CreateTable<User>(Net.Interop.CreateFlags.AutoIncPK);
				}

				connection.InsertOrReplace(new User() { Name = this.NameTextBox.Text }, typeof(User));
				this.Users = new ObservableCollection<User>(connection.Table<User>().Select(i => i));
			}
		}

        private static bool TableExists<T>(SQLiteConnection connection)
        {
            var cmd = connection.CreateCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{typeof(T).Name}'");
            return cmd.ExecuteScalar<string>() != null;
        }
    }
}
