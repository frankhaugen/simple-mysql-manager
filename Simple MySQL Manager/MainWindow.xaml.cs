using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
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

		private StringBuilder welcomMessage = new StringBuilder();
		private string serverFile = "servers.json";

		// Debugging
		private string debugServerFile = "server.secret"; // For debugging purposes 
		private bool debugmode = false; // For debugging purposes 

		public MainWindow()
		{
			InitializeComponent();


			if (System.Diagnostics.Debugger.IsAttached)
			{
				debugmode = true;
			}

			if (debugmode && File.Exists(debugServerFile)) { serverFile = debugServerFile; } // For debugging purposes 

			if (File.Exists(serverFile))
			{
				servers = JsonConvert.DeserializeObject<Servers>(File.ReadAllText(serverFile));
				SelectSaved.ItemsSource = servers.GetServers;
				SelectSaved.SelectedItem = SelectSaved.Items[0];
				SelectionBox.Visibility = Visibility.Visible;
			}

			CreatWelcomeText();
			MessageBox.Show(welcomMessage.ToString(), "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void CreatWelcomeText()
		{
			welcomMessage.AppendLine("Welcome to Simple MySQL Manager");
			welcomMessage.AppendLine("");
			welcomMessage.AppendLine("This simple tool will ease your database management.");
			welcomMessage.AppendLine("Unfortunately, for now, this tool is read-only,");
			welcomMessage.AppendLine("but we are working to improve this tool");
			welcomMessage.AppendLine("");
			welcomMessage.AppendLine("How-To:");
			welcomMessage.AppendLine("Either enter your database login-info manually,");
			welcomMessage.AppendLine("or edit/add the json-file named servers.json");
			welcomMessage.AppendLine("in the application's directory.");
			welcomMessage.AppendLine("");
			welcomMessage.AppendLine("This is released under the MIT License.");
			welcomMessage.AppendLine("Created by Frank R. Haugen");

		}

		private void BuildString()
		{
			connectionStringBuilder.Server = ServerField.Text;
			connectionStringBuilder.Database = DatabaseField.Text;
			connectionStringBuilder.UserID = UsernameField.Text;
			connectionStringBuilder.Password = PasswordField.Text;
		}


		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if (ServerField.Text != "" || DatabaseField.Text != "" || UsernameField.Text != "" || PasswordField.Text != "")
			{
				try
				{
					BuildString();

					Tabs.ItemsSource = null;
					tabItems.Clear();
					CollectTables();
					CreateGrids();

					Tabs.ItemsSource = tabItems;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				MessageBox.Show("All fields must be filled");
			}


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

			ServerField.Text = server.Address;
			DatabaseField.Text = server.Database;
			PasswordField.Text = server.Password;
			UsernameField.Text = server.Username;
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
