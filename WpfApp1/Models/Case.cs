using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WpfApp1.Models
{
    public class Case : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _title = string.Empty;
        private string _clientName = string.Empty;
        private string _caseType = string.Empty;
        private DateTime _startDate = DateTime.Now;
        private DateTime? _endDate;
        private string _description = string.Empty;
        private bool _isCompleted;

        public Guid CaseId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "Client name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 100 characters")]
        public string ClientName
        {
            get => _clientName;
            set
            {
                if (_clientName != value)
                {
                    _clientName = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "Case type is required")]
        public string CaseType
        {
            get => _caseType;
            set
            {
                if (_caseType != value)
                {
                    _caseType = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                    ValidateEndDate();
                }
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                    ValidateEndDate();
                }
            }
        }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(Title):
                        if (string.IsNullOrWhiteSpace(Title))
                            error = "Title is required";
                        else if (Title.Length < 3 || Title.Length > 100)
                            error = "Title must be between 3 and 100 characters";
                        break;

                    case nameof(ClientName):
                        if (string.IsNullOrWhiteSpace(ClientName))
                            error = "Client name is required";
                        else if (ClientName.Length < 2 || ClientName.Length > 100)
                            error = "Client name must be between 2 and 100 characters";
                        break;

                    case nameof(CaseType):
                        if (string.IsNullOrWhiteSpace(CaseType))
                            error = "Case type is required";
                        break;

                    case nameof(EndDate):
                        if (EndDate.HasValue && EndDate.Value < StartDate)
                            error = "End date cannot be earlier than start date";
                        break;

                    case nameof(Description):
                        if (Description?.Length > 2000)
                            error = "Description cannot exceed 2000 characters";
                        break;
                }

                return error;
            }
        }

        private void ValidateEndDate()
        {
            if (EndDate.HasValue && EndDate.Value < StartDate)
            {
                EndDate = StartDate;
            }
        }
    }
}
