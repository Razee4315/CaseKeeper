using System;
using System.IO;
using System.Collections.Generic;
using CaseKeeper.Models;

namespace CaseKeeper.Utils
{
    public static class FileManager
    {
        private static readonly string BaseDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CaseKeeper"
        );

        public static readonly string CasesDirectory = Path.Combine(BaseDirectory, "Cases");
        public static readonly string ReportsDirectory = Path.Combine(BaseDirectory, "Reports");

        static FileManager()
        {
            // Create necessary directories if they don't exist
            Directory.CreateDirectory(CasesDirectory);
            Directory.CreateDirectory(ReportsDirectory);
        }

        public static void SaveCase(Case caseData)
        {
            string filePath = Path.Combine(CasesDirectory, $"{caseData.CaseId}.txt");
            File.WriteAllText(filePath, caseData.ToString());
        }

        public static Case LoadCase(string caseId)
        {
            string filePath = Path.Combine(CasesDirectory, $"{caseId}.txt");
            if (!File.Exists(filePath))
                return null;

            string data = File.ReadAllText(filePath);
            return Case.FromString(data);
        }

        public static List<Case> LoadAllCases()
        {
            List<Case> cases = new List<Case>();
            foreach (string file in Directory.GetFiles(CasesDirectory, "*.txt"))
            {
                string data = File.ReadAllText(file);
                cases.Add(Case.FromString(data));
            }
            return cases;
        }

        public static void DeleteCase(string caseId)
        {
            string filePath = Path.Combine(CasesDirectory, $"{caseId}.txt");
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void GenerateReport(List<Case> cases, string reportName)
        {
            string filePath = Path.Combine(ReportsDirectory, $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("CaseKeeper - Case Report");
                writer.WriteLine($"Generated on: {DateTime.Now}\n");

                foreach (var caseData in cases)
                {
                    writer.WriteLine($"Case ID: {caseData.CaseId}");
                    writer.WriteLine($"Title: {caseData.Title}");
                    writer.WriteLine($"Client: {caseData.ClientName}");
                    writer.WriteLine($"Type: {caseData.CaseType}");
                    writer.WriteLine($"Start Date: {caseData.StartDate:d}");
                    writer.WriteLine($"End Date: {caseData.EndDate:d}");
                    writer.WriteLine($"Status: {(caseData.IsCompleted ? "Completed" : "Ongoing")}");
                    writer.WriteLine($"Description: {caseData.Description}\n");
                    writer.WriteLine(new string('-', 50) + "\n");
                }
            }
        }
    }
}
