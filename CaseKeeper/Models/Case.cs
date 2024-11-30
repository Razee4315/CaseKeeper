using System;

namespace CaseKeeper.Models
{
    public class Case
    {
        public string CaseId { get; set; }
        public string Title { get; set; }
        public string ClientName { get; set; }
        public string CaseType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Case()
        {
            CaseId = Guid.NewGuid().ToString();
            IsCompleted = false;
        }

        public override string ToString()
        {
            return $"{CaseId}|{Title}|{ClientName}|{CaseType}|{StartDate:yyyy-MM-dd}|{EndDate:yyyy-MM-dd}|{IsCompleted}|{Description}";
        }

        public static Case FromString(string data)
        {
            string[] parts = data.Split('|');
            return new Case
            {
                CaseId = parts[0],
                Title = parts[1],
                ClientName = parts[2],
                CaseType = parts[3],
                StartDate = DateTime.Parse(parts[4]),
                EndDate = DateTime.Parse(parts[5]),
                IsCompleted = bool.Parse(parts[6]),
                Description = parts[7]
            };
        }
    }
}
