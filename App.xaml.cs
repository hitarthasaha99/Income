using Income.Database;
namespace Income
{
    public partial class App : Application
    {
        static Database.Database database = null;
        public App()
        {
            InitializeComponent();
            EnsureDatabaseInitializedAsync().GetAwaiter().GetResult();
        }

        public static Database.Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database.Database();
                }

                return database;
            }
        }
        private static bool _dbInitialized = false;
        private static async Task EnsureDatabaseInitializedAsync()
        {
            if (!_dbInitialized)
            {
                await Database.InitializeAsync().ConfigureAwait(false);
                _dbInitialized = true;
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Income" };
        }
    }
}
