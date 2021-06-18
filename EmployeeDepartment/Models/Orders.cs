namespace EmployeeDepartment.Models
{
    using System;
    using System.ComponentModel;

    public partial class Orders : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ContractorName { get; set; }

        private Nullable<int> _Number;
        public Nullable<int> Number
        {
            get
            {
                return _Number;
            }
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    NotifyPropertyChanged("Name");
                    NotifyPropertyChanged("DisplayOrderAuthor");
                }
            }
        }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> AuthorId { get; set; }

        private Employees _Employees;
        public virtual Employees Employees
        {
            get
            {
                return _Employees;
            }
            set
            {
                if (_Employees != value)
                {
                    _Employees = value;
                    NotifyPropertyChanged("Employees");
                    NotifyPropertyChanged("DisplayOrderAuthor");
                }
            }
        }
        //отображение значения заказов
        public string DisplayOrderAuthor
        {
            get { return string.Format("{0} Автор:({1} {2})", _Number, Employees?.Name, Employees?.Surname); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayOrderAuthor"));
            }
        }
    }
}
