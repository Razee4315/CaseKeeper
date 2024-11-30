using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class CaseService : ICaseService
    {
        private readonly string _casesDirectory;
        private readonly JsonSerializerOptions _jsonOptions;

        public CaseService()
        {
            _casesDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CaseKeeper",
                "Cases"
            );
            Directory.CreateDirectory(_casesDirectory);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public void SaveCase(Case @case)
        {
            try
            {
                if (@case == null) throw new ArgumentNullException(nameof(@case));
                if (string.IsNullOrWhiteSpace(@case.Title)) throw new ArgumentException("Case title cannot be empty");

                string filePath = Path.Combine(_casesDirectory, $"{@case.CaseId}.json");
                string jsonString = JsonSerializer.Serialize(@case, _jsonOptions);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save case: {@case?.CaseId}", ex);
            }
        }

        public void UpdateCase(Case @case)
        {
            try
            {
                if (@case == null) throw new ArgumentNullException(nameof(@case));
                if (!File.Exists(Path.Combine(_casesDirectory, $"{@case.CaseId}.json")))
                {
                    throw new FileNotFoundException($"Case not found: {@case.CaseId}");
                }

                SaveCase(@case);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update case: {@case?.CaseId}", ex);
            }
        }

        public void DeleteCase(Guid caseId)
        {
            try
            {
                string filePath = Path.Combine(_casesDirectory, $"{caseId}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete case: {caseId}", ex);
            }
        }

        public Case? GetCase(Guid caseId)
        {
            try
            {
                string filePath = Path.Combine(_casesDirectory, $"{caseId}.json");
                if (!File.Exists(filePath)) return null;

                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Case>(jsonString, _jsonOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve case: {caseId}", ex);
            }
        }

        public IEnumerable<Case> GetAllCases()
        {
            try
            {
                return Directory.GetFiles(_casesDirectory, "*.json")
                    .Select(file =>
                    {
                        try
                        {
                            string jsonString = File.ReadAllText(file);
                            return JsonSerializer.Deserialize<Case>(jsonString, _jsonOptions);
                        }
                        catch
                        {
                            return null;
                        }
                    })
                    .Where(c => c != null)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve cases", ex);
            }
        }

        public IEnumerable<Case> GetUpcomingDeadlines(int daysAhead = 7)
        {
            try
            {
                var endDate = DateTime.Now.AddDays(daysAhead);
                return GetAllCases()
                    .Where(c => !c.IsCompleted && 
                               c.EndDate.HasValue && 
                               c.EndDate.Value.Date <= endDate.Date)
                    .OrderBy(c => c.EndDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve upcoming deadlines", ex);
            }
        }

        public void BackupCases(string backupPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(backupPath))
                    throw new ArgumentException("Backup path cannot be empty");

                string backupDir = Path.GetDirectoryName(backupPath) ?? 
                    throw new ArgumentException("Invalid backup path");
                
                Directory.CreateDirectory(backupDir);

                var cases = GetAllCases().ToList();
                string jsonString = JsonSerializer.Serialize(cases, _jsonOptions);
                File.WriteAllText(backupPath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to backup cases", ex);
            }
        }

        public void RestoreCases(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                    throw new FileNotFoundException("Backup file not found", backupPath);

                string jsonString = File.ReadAllText(backupPath);
                var cases = JsonSerializer.Deserialize<List<Case>>(jsonString, _jsonOptions);

                if (cases == null) throw new InvalidOperationException("Invalid backup file");

                // Clear existing cases
                foreach (var file in Directory.GetFiles(_casesDirectory, "*.json"))
                {
                    File.Delete(file);
                }

                // Restore cases from backup
                foreach (var @case in cases)
                {
                    SaveCase(@case);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to restore cases", ex);
            }
        }

        public void GenerateReport(string reportPath, IEnumerable<Case> cases)
        {
            using (var writer = new StreamWriter(reportPath))
            {
                writer.WriteLine("CaseKeeper - Case Report");
                writer.WriteLine($"Generated on: {DateTime.Now:f}\n");

                foreach (var caseData in cases)
                {
                    writer.WriteLine($"Case ID: {caseData.CaseId}");
                    writer.WriteLine($"Title: {caseData.Title}");
                    writer.WriteLine($"Client: {caseData.ClientName}");
                    writer.WriteLine($"Type: {caseData.CaseType}");
                    writer.WriteLine($"Start Date: {caseData.StartDate:d}");
                    writer.WriteLine($"End Date: {caseData.EndDate:d}");
                    writer.WriteLine($"Status: {(caseData.IsCompleted ? "Completed" : "Active")}");
                    writer.WriteLine($"Description: {caseData.Description}\n");
                    writer.WriteLine(new string('-', 50) + "\n");
                }
            }
        }
    }
}
