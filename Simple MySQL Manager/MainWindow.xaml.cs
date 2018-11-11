using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Simple_MySQL_Manager
{

	internal class Servers
	{
		[JsonProperty("servers")]
		public List<Server> GetServers { get; set; }
	}

	internal class Server
	{
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("friendlyname")]
		public string Friendlyname { get; set; }

		[JsonProperty("database")]
		public string Database { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("password")]
		public string Password { get; set; }
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Servers servers = null;
		private List<TabItem> tabItems = new List<TabItem>();
		private Dictionary<string, Server> serverDict = new Dictionary<string, Server>();

		private MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
		private MySqlConnection mySqlConnection = null;
		private MySqlCommand mySqlCommand = null;
		private string query = "";


		public MainWindow()
		{
			InitializeComponent();

			if (File.Exists("server.secret"))
			{
				servers = JsonConvert.DeserializeObject<Servers>(File.ReadAllText("server.secret"));
				SelectSaved.ItemsSource = servers.GetServers;
				SelectSaved.SelectedItem = SelectSaved.Items[0];
				SelectionBox.Visibility = Visibility.Visible;
			}

		}

		private void BuildString()
		{
			connectionStringBuilder.Server = ServerField.Password;
			connectionStringBuilder.Database = DatabaseField.Password;
			connectionStringBuilder.UserID = UsernameField.Password;
			connectionStringBuilder.Password = PasswordField.Password;
		}


		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			BuildString();

			tabItems.Clear();
			CollectTables();
			CreateGrids();

			Tabs.ItemsSource = tabItems;
		}

		private void CollectTables()
		{
			query = "SHOW TABLES;";

			mySqlConnection = new MySqlConnection(connectionStringBuilder.ToString());
			mySqlConnection.Open();
			mySqlCommand = new MySqlCommand(query, mySqlConnection);
			MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();

			try
			{
				MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

				while (mySqlDataReader.Read())
				{

					tabItems.Add(new TabItem()
					{
						Header = mySqlDataReader[0].ToString()
					});
				}
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.ToString());

			}
			finally
			{

				if (mySqlConnection != null)
				{
					mySqlConnection.Close();
				}

			}

		}

		private void SelectSaved_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Server server = (sender as ComboBox).SelectedItem as Server;

			ServerField.Password = server.Address;
			DatabaseField.Password = server.Database;
			PasswordField.Password = server.Password;
			UsernameField.Password = server.Username;
		}

		private void CreateGrids()
		{
			foreach (TabItem tabItem in tabItems)
			{
				DataTable dataTable = new DataTable();
				MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
				string sql = "SELECT * FROM " + tabItem.Header.ToString();
				mySqlDataAdapter.SelectCommand = new MySqlCommand(sql, mySqlConnection);

				mySqlDataAdapter.Fill(dataTable);

				DataGrid dataGrid = new DataGrid
				{

					ItemsSource = dataTable.DefaultView,
					DataContext = dataTable,
				};

				tabItem.Content = dataGrid;
			}

			mySqlConnection.Close();
		}

	}
}
