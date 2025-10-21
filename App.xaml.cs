using Income.Database;
using Income.Services;
namespace Income
{
    public partial class App : Application
    {
        static Database.Database database = null;
        public App(IGlobalExceptionHandler exceptionHandler)
        {
            InitializeComponent();
            EnsureDatabaseInitializedAsync().GetAwaiter().GetResult();
            // Capture UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Capture unobserved task exceptions
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            exceptionHandler.Initialize();

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

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogException(ex);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception);
            e.SetObserved(); // Prevent crashing
        }

        private void LogException(Exception ex)
        {
            // Custom logging logic, e.g. send to server or local file
            Console.WriteLine($"Unhandled Exception: {ex.Message}\n{ex.StackTrace}");
        }
    }
}
