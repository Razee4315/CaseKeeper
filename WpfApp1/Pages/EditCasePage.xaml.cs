using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class EditCasePage : Page
    {
        private readonly CaseService _caseService;
        private readonly Case _case;

        public EditCasePage(Case caseToEdit)
        {
            InitializeComponent();
            _caseService = new CaseService();
            _case = caseToEdit ?? throw new ArgumentNullException(nameof(caseToEdit));
            LoadCaseData();
        }

        private void LoadCaseData()
        {
            TitleTextBox.Text = _case.Title;
            ClientNameTextBox.Text = _case.ClientName;
            CaseTypeComboBox.SelectedItem = CaseTypeComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _case.CaseType);
            StartDatePicker.SelectedDate = _case.StartDate;
            EndDatePicker.SelectedDate = _case.EndDate;
            DescriptionTextBox.Text = _case.Description;
            IsCompletedCheckBox.IsChecked = _case.IsCompleted;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _case.Title = TitleTextBox.Text;
                _case.ClientName = ClientNameTextBox.Text;
                _case.CaseType = (CaseTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other";
                _case.StartDate = StartDatePicker.SelectedDate ?? DateTime.Now;
                _case.EndDate = EndDatePicker.SelectedDate;
                _case.Description = DescriptionTextBox.Text;
                _case.IsCompleted = IsCompletedCheckBox.IsChecked ?? false;

                _caseService.UpdateCase(_case);
                MessageBox.Show("Case updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating case: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
