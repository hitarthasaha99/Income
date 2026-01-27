namespace Income
{
    public partial class MainPage : ContentPage
    {
        public static Func<bool>? OnBack;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // true = handled (we decide what to do)
            if (OnBack?.Invoke() == true)
                return true;

            return base.OnBackButtonPressed();
        }
    }
}
