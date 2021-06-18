namespace EmployeeDepartment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Departments : INotifyPropertyChanged
    {
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departments()
        {
            this.Employees1 = new HashSet<Employees>();
        }

        public int Id { get; set; }
        private string _Name;
        public string Name {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                    NotifyPropertyChanged("DisplayDepartmentChief");
                }
            }
        }
        public Nullable<int> СhiefId { get; set; }
        private Employees _Employees;
        public virtual Employees Employees {
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
                    NotifyPropertyChanged("DisplayDepartmentChief");
                }
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees> Employees1 { get; set; }

        //отображение значения подразделения
        public string DisplayDepartmentChief
        {
            get {return string.Format("{0} Директо:({1} {2})", Name, Employees?.Name, Employees?.Surname); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayDepartmentChief"));
            }
        }

    }
}
