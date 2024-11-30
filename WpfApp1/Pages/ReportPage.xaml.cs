using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class ReportPage : Page
    {
        private readonly CaseService _caseService;

        public ReportPage()
        {
            InitializeComponent();
            _caseService = new CaseService();
            StartDatePicker.SelectedDate = DateTime.Now.AddMonths(-1);
            EndDatePicker.SelectedDate = DateTime.Now;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cases = _caseService.GetAllCases();

                // Apply filters
                if (ActiveCasesRadio.IsChecked == true)
                    cases = cases.Where(c => !c.IsCompleted);
                else if (CompletedCasesRadio.IsChecked == true)
                    cases = cases.Where(c => c.IsCompleted);
                else if (DateRangeRadio.IsChecked == true)
                {
                    if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
                    {
                        MessageBox.Show("Please select both start and end dates.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    cases = cases.Where(c => 
                        c.StartDate >= StartDatePicker.SelectedDate.Value &&
                        c.EndDate <= EndDatePicker.SelectedDate.Value);
                }

                if (!cases.Any())
                {
                    MessageBox.Show("No cases found matching the selected criteria.", "No Data", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Show save file dialog
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt",
                    DefaultExt = "txt",
                    FileName = $"CaseKeeper_Report_{DateTime.Now:yyyyMMdd}"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    _caseService.GenerateReport(saveDialog.FileName, cases);
                    MessageBox.Show("Report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Ask if user wants to open the report
                    var result = MessageBox.Show(
                        "Would you like to open the report now?",
                        "Open Report",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("notepad.exe", saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
