using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class RemindersPage : Page
    {
        private readonly CaseService _caseService;

        public RemindersPage()
        {
            InitializeComponent();
            _caseService = new CaseService();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReminders();
        }

        private void LoadReminders()
        {
            var upcomingDeadlines = _caseService.GetUpcomingDeadlines();
            RemindersListView.ItemsSource = upcomingDeadlines;
        }

        private void Snooze_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Case selectedCase && selectedCase.EndDate.HasValue)
            {
                selectedCase.EndDate = selectedCase.EndDate.Value.AddDays(1);
                try
                {
                    _caseService.UpdateCase(selectedCase);
                    LoadReminders();
                    MessageBox.Show("Deadline snoozed for 1 day.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating case: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Case selectedCase)
            {
                selectedCase.IsCompleted = true;
                try
                {
                    _caseService.UpdateCase(selectedCase);
                    LoadReminders();
                    MessageBox.Show("Case marked as completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating case: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
