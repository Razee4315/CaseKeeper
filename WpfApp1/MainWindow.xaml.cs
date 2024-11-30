using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Button _currentActiveButton;
        private bool _isInitialized;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindowState();
        }

        private void InitializeWindowState()
        {
            try
            {
                // Set minimum window size
                MinWidth = 1024;
                MinHeight = 768;

                // Center the window on startup
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                // Initialize the frame and navigation
                MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
                NavigateToDefaultPage();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing window state: {ex.Message}", 
                    "Initialization Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private void NavigateToDefaultPage()
        {
            try
            {
                MainFrame.Navigate(new NewCasePage());
                HighlightActiveButton(FindName("NewCaseButton") as Button);
                AnimateContentTransition();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to default page: {ex.Message}",
                    "Navigation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AnimateContentTransition()
        {
            if (MainFrame.Content is FrameworkElement content)
            {
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                content.Opacity = 0;
                content.BeginAnimation(OpacityProperty, fadeIn);
            }
        }

        private void HighlightActiveButton(Button button)
        {
            if (!_isInitialized) return;

            if (_currentActiveButton != null)
            {
                _currentActiveButton.Background = Brushes.Transparent;
            }

            if (button != null)
            {
                _currentActiveButton = button;
                _currentActiveButton.Background = FindResource("AccentBrush") as SolidColorBrush;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            try
            {
                // Load any saved window position/size
                LoadWindowSettings();
                
                // Register for window closing to save settings
                Closing += MainWindow_Closing;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during window initialization: {ex.Message}", 
                    "Initialization Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private void LoadWindowSettings()
        {
            // TODO: Load window position and size from settings
            // This can be implemented later when we add settings persistence
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // TODO: Save window position and size to settings
                // This can be implemented later when we add settings persistence
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving window settings: {ex.Message}",
                    "Settings Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void NewCase_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NewCasePage());
            HighlightActiveButton(sender as Button);
            AnimateContentTransition();
        }

        private void ViewCases_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ViewCasesPage());
            HighlightActiveButton(sender as Button);
            AnimateContentTransition();
        }

        private void Reminders_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RemindersPage());
            HighlightActiveButton(sender as Button);
            AnimateContentTransition();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportPage());
            HighlightActiveButton(sender as Button);
            AnimateContentTransition();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());
            HighlightActiveButton(sender as Button);
            AnimateContentTransition();
        }
    }
}