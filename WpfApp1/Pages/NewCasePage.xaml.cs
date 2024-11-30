using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class NewCasePage : Page
    {
        private readonly CaseService _caseService;

        public NewCasePage()
        {
            InitializeComponent();
            _caseService = new CaseService();

            // Set default values
            StartDatePicker.SelectedDate = DateTime.Now;
            CaseTypeComboBox.SelectedIndex = 0;
        }

        private void CreateCase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Please enter a case title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TitleTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
                {
                    MessageBox.Show("Please enter a client name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    ClientNameTextBox.Focus();
                    return;
                }

                var newCase = new Case
                {
                    Title = TitleTextBox.Text.Trim(),
                    ClientName = ClientNameTextBox.Text.Trim(),
                    CaseType = (CaseTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other",
                    StartDate = StartDatePicker.SelectedDate ?? DateTime.Now,
                    EndDate = EndDatePicker.SelectedDate,
                    Description = DescriptionTextBox.Text?.Trim() ?? string.Empty,
                    IsCompleted = false
                };

                _caseService.SaveCase(newCase);
                MessageBox.Show("Case created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating case: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to cancel? Any unsaved changes will be lost.",
                "Confirm Cancel",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NavigationService?.GoBack();
            }
        }
    }
}
