using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataAdapterExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // вспомогательная процедура, создающая подключение к БД
        private SqlConnection GetDbConnection()
        {
            // обработка исключений будет выполняться выше по стеку
            string connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // connection.Open(); - в отсоединенном режиме подключение открывает и закрывает DbDataAdapter
            return connection;
        }

        // вспомогательный метод обновления данных
        private void fillData()
        {
            // вытягивание данных из БД согласно запросу
            dataSet = new DataSet();    // в этот DataSet будет выполняться запись результата вытягивания
            dataAdapter.Fill(dataSet, "computer_game");  // выгружаем данные из БД и сохраняем в DataSet
            dataGrid.ItemsSource = dataSet.Tables["computer_game"].DefaultView;
        }
        private void fillDataUser()
        {
            dataSet1 = new DataSet();
            dataAdapter1.Fill(dataSet1, "computer_game");
            dataGrid1.ItemsSource = dataSet1.Tables["computer_game"].DefaultView;
        }
        public MainWindow()
        {
            InitializeComponent();
            // подготовка объектов
            dataAdapter = new SqlDataAdapter(fillCmd, GetDbConnection());
            dataAdapter1 = new SqlDataAdapter(UserCmd, GetDbConnection());
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);  // строка заполняет INSERT/UPDATE/DELETE-команды
            // реализацией по умолчанию
            SqlCommandBuilder commandBuilder1 = new SqlCommandBuilder(dataAdapter1);
        }

        // вспомогательные поля
        private SqlDataAdapter dataAdapter;
        private SqlDataAdapter dataAdapter1;
        private DataSet dataSet;
        private DataSet dataSet1;
        private string fillCmd = "SELECT * FROM computer_game;";
        private string UserCmd= "SELECT * FROM computer_game;";

        private void fillBtn_Click(object sender, RoutedEventArgs e)
        {
            fillData();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            dataAdapter.Update(dataSet, "computer_game");
            fillData();

        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        { 
            UserCmd=QweryText.Text;
            dataAdapter1.SelectCommand.CommandText = UserCmd;
           
            fillDataUser();
        }

        private void update1Btn_Click(object sender, RoutedEventArgs e)
        {
            dataAdapter1.Update(dataSet1, "computer_game");
            fillData();
        }
    }
}
