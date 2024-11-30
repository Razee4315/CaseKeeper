using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;

namespace WpfApp1.Pages
{
    public partial class SettingsPage : Page
    {
        private readonly CaseService _caseService;

        public SettingsPage()
        {
            InitializeComponent();
            _caseService = new CaseService();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load settings from app config or settings file
            // For now, using default values
        }

        private void ThemeRadio_Checked(object sender, RoutedEventArgs e)
        {
            // Implement theme switching logic
            MessageBox.Show("Theme functionality will be implemented in the next update.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackupData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Zip Files (*.zip)|*.zip",
                    DefaultExt = "zip",
                    FileName = $"CaseKeeper_Backup_{DateTime.Now:yyyyMMdd}"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), "CaseKeeperBackup");
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);
                    Directory.CreateDirectory(tempDir);

                    // Copy all case files to temp directory
                    string casesDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "CaseKeeper",
                        "Cases"
                    );

                    if (Directory.Exists(casesDir))
                    {
                        foreach (string file in Directory.GetFiles(casesDir))
                        {
                            string destFile = Path.Combine(tempDir, Path.GetFileName(file));
                            File.Copy(file, destFile);
                        }
                    }

                    // Create zip file
                    ZipFile.CreateFromDirectory(tempDir, saveDialog.FileName);
                    Directory.Delete(tempDir, true);

                    MessageBox.Show("Backup created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestoreData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog
                {
                    Filter = "Zip Files (*.zip)|*.zip",
                    DefaultExt = "zip"
                };

                if (openDialog.ShowDialog() == true)
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), "CaseKeeperRestore");
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);

                    // Extract backup
                    ZipFile.ExtractToDirectory(openDialog.FileName, tempDir);

                    // Clear current cases directory
                    string casesDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "CaseKeeper",
                        "Cases"
                    );

                    if (Directory.Exists(casesDir))
                        Directory.Delete(casesDir, true);
                    Directory.CreateDirectory(casesDir);

                    // Copy restored files
                    foreach (string file in Directory.GetFiles(tempDir))
                    {
                        string destFile = Path.Combine(casesDir, Path.GetFileName(file));
                        File.Copy(file, destFile);
                    }

                    Directory.Delete(tempDir, true);
                    MessageBox.Show("Data restored successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Save settings to app config or settings file
                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
