namespace MauiAppTask2CRUD
{
    public partial class App : Application
    {
        static PersonDatabase _database;

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        public static PersonDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "People.db3");
                    _database = new PersonDatabase(dbPath);
                }
                return _database;
            }
        }
    }
}
