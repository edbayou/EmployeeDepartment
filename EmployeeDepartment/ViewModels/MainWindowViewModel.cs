using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Xaml;
using EmployeeDepartment.Models;
using System.Collections.ObjectModel;
using System.Windows;
using EmployeeDepartment.Commands;
using System.Linq;
using System.Data.Entity;

namespace EmployeeDepartment.ViewModels
{

    internal class MainWindowViewModel : INotifyPropertyChanged
    {


        EmployeeDepartmentEntities db;

        private Employees selectedEmployee;
        private Departments selectedDepartment;
        private Orders selectedOrder;


        public ObservableCollection<Departments> DepartmentsCollection { get; set; }
        public ObservableCollection<Employees> EmployeesCollection { get; set; }
        public ObservableCollection<Orders> OrdersCollection { get; set; }

        public ObservableCollection<Employees> ChangedEmployeeCollection { get; set; }
        public ObservableCollection<Departments> ChangedDepartmentCollection { get; set; }
        public ObservableCollection<Orders> ChangedOrderCollection { get; set; }

        public Departments SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                if (selectedDepartment != null)
                    sortEmployees(selectedDepartment.Id);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                }
                OnPropertyChanged("SelectedDepartment");
            }
        }
        public Employees SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                }

                OnPropertyChanged("SelectedEmployee");

            }
        }
        public Orders SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                }
                OnPropertyChanged("SelectedOrder");
            }
        }
        public MainWindowViewModel()
        {
            db = new EmployeeDepartmentEntities();

            ChangedEmployeeCollection = new ObservableCollection<Employees>();
            ChangedDepartmentCollection = new ObservableCollection<Departments>();
            ChangedOrderCollection = new ObservableCollection<Orders>();

            EmployeesCollection = new ObservableCollection<Employees>();
            foreach (Employees item in db.Employees)
            {
                EmployeesCollection.Add(item);
            }

            DepartmentsCollection = new ObservableCollection<Departments>();
            foreach (Departments item in db.Departments)
            {
                DepartmentsCollection.Add(item);
            }
            OrdersCollection = new ObservableCollection<Orders>();
            foreach (Orders item in db.Orders)
            {
                OrdersCollection.Add(item);
            }
        }

        //добавить подразделение
        private LambdaCommand addDepartmentCommand;
        public LambdaCommand AddDepartmentCommand
        {
            get
            {
                return addDepartmentCommand ??
                  (addDepartmentCommand = new LambdaCommand(obj =>
                  {
                      Departments department = new Departments();
                      ChangedDepartmentCollection.Clear();
                      ChangedDepartmentCollection.Add(department);
                      SelectedDepartment = department;
                  },
                  (obj) => ChangedDepartmentCollection.Count == 0));
            }
        }

        //добавить заказ 
        private LambdaCommand addOrderCommand;
        public LambdaCommand AddOrderCommand
        {
            get
            {
                return addOrderCommand ??
                (addOrderCommand = new LambdaCommand(obj =>
                {
                    Orders order = new Orders();
                    ChangedOrderCollection.Clear();
                    ChangedOrderCollection.Add(order);
                    SelectedOrder = order;
                },
                  (obj) => ChangedOrderCollection.Count == 0));
            }
        }

        //добавить сотрудника
        private LambdaCommand addEmployeeCommand;
        public LambdaCommand AddEmployeeCommand
        {
            get
            {
                return addEmployeeCommand ??
                  (addEmployeeCommand = new LambdaCommand(obj =>
                  {
                      Employees employee = new Employees();
                      if (selectedDepartment != null)
                      {
                          employee.DepartmentsId = SelectedDepartment.Id;
                          ChangedEmployeeCollection.Clear();
                          ChangedEmployeeCollection.Add(employee);
                          SelectedEmployee = employee;
                      }
                      else
                      {
                          MessageBoxResult result = MessageBox.Show("Выберите подразделение!");
                      }
                  },
                 (obj) => ChangedEmployeeCollection.Count == 0));
            }
        }

        // удалить подразделение
        private LambdaCommand removeDepartmentCommand;
        public LambdaCommand RemoveDepartmentCommand
        {
            get
            {
                return removeDepartmentCommand ??
                  (removeDepartmentCommand = new LambdaCommand(obj =>
                  {

                      Departments department = obj as Departments;
                      if (department != null)
                      {
                          foreach (Employees employees in db.Employees)
                          {
                              if (employees.DepartmentsId == department.Id)
                              {
                                  EmployeesCollection.Remove(employees);
                                  db.Employees.Remove(employees);
                              }
                          }
                          DepartmentsCollection.Remove(department);
                          ChangedDepartmentCollection.Remove(department);
                          if (DepartmentsCollection.Count > 0)
                              SelectedDepartment = DepartmentsCollection.ElementAt(0);
                          else
                              SelectedDepartment = null;
                          db.Departments.Remove(department);
                          db.SaveChanges();
                      }
                  },
                 (obj) => selectedDepartment != null));
            }
        }
        // удалить заказ
        private LambdaCommand removeOrderCommand;
        public LambdaCommand RemoveOrderCommand
        {
            get
            {
                return removeOrderCommand ??
                  (removeOrderCommand = new LambdaCommand(obj =>
                  {

                      Orders order = obj as Orders;
                      if (order != null)
                      {
                          OrdersCollection.Remove(order);
                          ChangedOrderCollection.Remove(order);
                          if (OrdersCollection.Count > 0)
                              SelectedOrder = OrdersCollection.ElementAt(0);
                          else
                              SelectedOrder = null;
                          db.Orders.Remove(order);
                          db.SaveChanges();
                      }
                  },
                 (obj) => selectedDepartment != null));
            }
        }
        // удалить сотрудника
        private LambdaCommand removeEmployeeCommand;
        public LambdaCommand RemoveEmployeeCommand
        {
            get
            {
                return removeEmployeeCommand ??
                  (removeEmployeeCommand = new LambdaCommand(obj =>
                  {
                      Employees employee = obj as Employees;
                      if (employee != null)
                      {
                          if (EmployeesCollection.Count > 0)
                              SelectedEmployee = EmployeesCollection.ElementAt(0);
                          else
                              SelectedEmployee = null;
                          ChangedEmployeeCollection.Remove(employee);
                          db.Employees.Remove(employee);
                          db.SaveChanges();
                          EmployeesCollection.Remove(employee);
                      }
                  },
                 (obj) => selectedEmployee != null));
            }
        }

        //установить директора
        private LambdaCommand setupChiefEmployeeCommand;
        public LambdaCommand SetupChiefEmployeeCommand
        {
            get
            {
                return setupChiefEmployeeCommand ??
                  (setupChiefEmployeeCommand = new LambdaCommand(obj =>
                  {

                      if (selectedDepartment != null)
                      {
                          Departments department = selectedDepartment;
                          if(SelectedEmployee != null)
                            department.СhiefId = SelectedEmployee.Id;
                          ChangedDepartmentCollection.Clear();
                          ChangedDepartmentCollection.Add(department);
                          var item = DepartmentsCollection.FirstOrDefault(i => i.Id == department.Id);
                          item.Employees = SelectedEmployee;


                          ChangedDepartmentCollection.Clear();

                          try
                          {
                              db.SaveChanges();
                          }
                          catch
                          {
                              MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                          }
                      }
                      else
                      {
                          MessageBoxResult result = MessageBox.Show("Выберите подразделение!");
                      }

                  },
                  (obj) => ChangedDepartmentCollection.Count == 0));
            }
        }
        //установить автора
        private LambdaCommand setupAuthorEmployeeCommand;
        public LambdaCommand SetupAuthorEmployeeCommand
        {
            get
            {
                return setupAuthorEmployeeCommand ??
                  (setupAuthorEmployeeCommand = new LambdaCommand(obj =>
                  {

                      if (selectedOrder != null)
                      {
                          Orders order = selectedOrder;
                          if (SelectedEmployee != null)
                              order.AuthorId = SelectedEmployee.Id;
                          ChangedOrderCollection.Clear();
                          ChangedOrderCollection.Add(order);
                          var item = OrdersCollection.FirstOrDefault(i => i.Id == order.Id);
                          item.Employees = SelectedEmployee;
                          ChangedOrderCollection.Clear();
                          try
                          {
                              db.SaveChanges();
                          }
                          catch
                          {
                              MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                          }
                      }
                      else
                      {
                          MessageBoxResult result = MessageBox.Show("Выберите заказ!");
                      }

                  },
                  (obj) => ChangedOrderCollection.Count == 0));
            }
        }
        //сохранить подразделение
        private LambdaCommand saveDepartmentCommand;
        public LambdaCommand SaveDepartmentCommand
        {
            get
            {
                return saveDepartmentCommand ??
                  (saveDepartmentCommand = new LambdaCommand(obj =>
                  {
                      foreach (Departments department in ChangedDepartmentCollection)
                      {
                          if (department != null)
                          {
                              DepartmentsCollection.Add(department);
                              db.Departments.Add(department);
                          }
                      }
                      ChangedDepartmentCollection.Clear();
                      try
                      {
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                      }

                  },
                 (obj) => ChangedDepartmentCollection.Count > 0));
            }
        }
        //сохранить заказ
        private LambdaCommand saveOrderCommand;
        public LambdaCommand SaveOrderCommand
        {
            get
            {
                return saveOrderCommand ??
                  (saveOrderCommand = new LambdaCommand(obj =>
                  {
                      foreach (Orders order in ChangedOrderCollection)
                      {
                          if (order != null)
                          {
                              OrdersCollection.Add(order);
                              db.Orders.Add(order);
                          }
                      }
                      ChangedOrderCollection.Clear();
                      try
                      {
                          db.SaveChanges();
                      }
                      catch (Exception ex)
                      {
                          string d = ex.ToString();
                          MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                      }

                  },
                 (obj) => ChangedOrderCollection.Count > 0));
            }
        }
        //сохранить сотрудника
        private LambdaCommand saveEmployeeCommand;
        public LambdaCommand SaveEmployeeCommand
        {
            get
            {
                return saveEmployeeCommand ??
                  (saveEmployeeCommand = new LambdaCommand(obj =>
                  {
                      foreach (Employees employee in ChangedEmployeeCollection)
                      {
                          if (employee != null)
                          {

                              if (employee.DepartmentsId > 0)
                              {
                                  db.Entry(employee).State = EntityState.Modified;
                              }
                              else
                              {
                                  db.Entry(employee).State = EntityState.Added;
                              }
                              EmployeesCollection.Add(employee);
                              db.Employees.Add(employee);
                          }
                      }
                      ChangedEmployeeCollection.Clear();
                      try
                      {
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBoxResult result = MessageBox.Show("Ошибка :-(");
                      }
                  },
                 (obj) => ChangedEmployeeCollection.Count > 0));
            }
        }

        //отменить создание подразделение
        private LambdaCommand cancelDepartmentCommand;
        public LambdaCommand CancelDepartmentCommand
        {
            get
            {
                return cancelDepartmentCommand ??
                  (cancelDepartmentCommand = new LambdaCommand(obj =>
                  {
                      Departments department = obj as Departments;
                      SelectedDepartment = DepartmentsCollection.Count() > 0 ? DepartmentsCollection.ElementAt(0) : null;
                      ChangedDepartmentCollection.Clear();
                  },
                 (obj) => ChangedDepartmentCollection.Count > 0));
            }
        }
        //отменить создание заказа
        private LambdaCommand canceOrderCommand;
        public LambdaCommand CanceOrderCommand
        {
            get
            {
                return canceOrderCommand ??
                  (canceOrderCommand = new LambdaCommand(obj =>
                  {
                      Orders order = obj as Orders;
                      SelectedOrder = OrdersCollection.Count()>0 ? OrdersCollection.ElementAt(0) : null;
                      ChangedOrderCollection.Clear();
                  },
                 (obj) => ChangedOrderCollection.Count > 0));
            }
        }
        //отменить создание сотрудника
        private LambdaCommand cancelEmployeeCommand;
        public LambdaCommand CancelEmployeeCommand
        {
            get
            {
                return cancelEmployeeCommand ??
                  (cancelEmployeeCommand = new LambdaCommand(obj =>
                  {
                      Employees user = obj as Employees;
                      SelectedEmployee = EmployeesCollection.Count() > 0 ? EmployeesCollection?.ElementAt(0) : null;
                      ChangedEmployeeCollection.Clear();
                  },
                 (obj) => ChangedEmployeeCollection.Count > 0));
            }
        }

        //сортировать список пользователей по выбранной компании
        private void sortEmployees(int parametr)
        {
            EmployeesCollection.Clear();
            foreach (Employees item in db.Employees)
            {
                if (item.DepartmentsId.Equals(parametr))
                {
                    EmployeesCollection.Add(item);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
