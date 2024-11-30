using System;
using System.Collections.Generic;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public interface ICaseService
    {
        void SaveCase(Case @case);
        void UpdateCase(Case @case);
        void DeleteCase(Guid caseId);
        Case? GetCase(Guid caseId);
        IEnumerable<Case> GetAllCases();
        IEnumerable<Case> GetUpcomingDeadlines(int daysAhead = 7);
        void BackupCases(string backupPath);
        void RestoreCases(string backupPath);
    }
}
