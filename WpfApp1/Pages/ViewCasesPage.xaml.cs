using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class ViewCasesPage : Page
    {
        private readonly CaseService _caseService;

        public ViewCasesPage()
        {
            InitializeComponent();
            _caseService = new CaseService();
            FilterComboBox.SelectedIndex = 0;
            LoadCases();
        }

        private void LoadCases()
        {
            try
            {
                var cases = _caseService.GetAllCases();
                ApplyFilters(cases);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cases: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters(IEnumerable<Case> cases)
        {
            if (cases == null) return;

            // Apply status filter
            if (FilterComboBox?.SelectedIndex == 1) // Active Cases
                cases = cases.Where(c => !c.IsCompleted);
            else if (FilterComboBox?.SelectedIndex == 2) // Completed Cases
                cases = cases.Where(c => c.IsCompleted);

            // Apply search filter
            string searchTerm = SearchTextBox?.Text?.ToLower() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                cases = cases.Where(c => 
                    (c.Title?.ToLower()?.Contains(searchTerm) ?? false) || 
                    (c.ClientName?.ToLower()?.Contains(searchTerm) ?? false));
            }

            CasesDataGrid.ItemsSource = cases.OrderByDescending(c => c.StartDate);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCases();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCases();
        }

        private void CasesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable/disable context menu items based on selection
        }

        private void EditCase_Click(object sender, RoutedEventArgs e)
        {
            if (CasesDataGrid?.SelectedItem is Case selectedCase)
            {
                NavigationService?.Navigate(new EditCasePage(selectedCase));
            }
        }

        private void DeleteCase_Click(object sender, RoutedEventArgs e)
        {
            if (CasesDataGrid?.SelectedItem is Case selectedCase)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this case?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _caseService.DeleteCase(selectedCase.CaseId);
                        LoadCases();
                        MessageBox.Show("Case deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting case: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (CasesDataGrid?.SelectedItem is Case selectedCase)
            {
                selectedCase.IsCompleted = true;
                try
                {
                    _caseService.UpdateCase(selectedCase);
                    LoadCases();
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
