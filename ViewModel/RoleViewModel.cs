using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using WpfApplDemo2018.View;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplDemo2018.Helper;
using WpfApplDemo2018.Model;

namespace WpfApplDemo2018.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        readonly string path = @"D:\1. Универcитет\5-ый семестр\Разработка и сопровождение программных систем\Лаба1\WpfApplDemo2018\DataModels\RoleData.json";
        /// <summary>
        /// коллекция должностей сотрудников
        /// </summary>
        public ObservableCollection<Role> ListRole { get; set; } = new
       ObservableCollection<Role>();
        /// <summary>
        /// выбранная в списке должность
        /// </summary>
        private Role _selectedRole;
        /// <summary>
        /// выбранная в списке должность
        /// </summary>
        public Role SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }

        public string Error { get; set; }
        string _jsonRoles = String.Empty;
        public RoleViewModel()
        {
            ListRole = LoadRole();
        }
        #region command AddRole
        /// команда добавления новой должности
        private RelayCommand _addRole;
        public RelayCommand AddRole
        {
            get
            {
                return _addRole ??
                (_addRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Новая должность",
                    };
                    // формирование кода новой должности
                    int maxIdRole = MaxId() + 1;
                    Role role = new Role { Id = maxIdRole };
                    wnRole.DataContext = role;
                    if (wnRole.ShowDialog() == true)
                    {
                        ListRole.Add(role);
                        SaveChanges(ListRole);
                    }
                    SelectedRole = role;
                },
                (obj) => true));
            }
        }
        #endregion
        #region Command EditRole
        /// команда редактирования должности
        private RelayCommand _editRole;
        public RelayCommand EditRole
        {
            get
            {
                return _editRole ??
                (_editRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности",
                    };
                    Role role = SelectedRole;
                    var tempRole = role.ShallowCopy();
                    wnRole.DataContext = tempRole;
                    if (wnRole.ShowDialog() == true)
               
                {
                    // сохранение данных в оперативной памяти
                    role.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole);
                    }
                }, (obj) => SelectedRole != null && ListRole.Count >
               0));
            }
        }
        #endregion
        #region DeleteRole
        /// команда удаления должности
        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return _deleteRole ??
                (_deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по должности: " + role.NameRole,
    
                    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(role);
                        SaveChanges(ListRole);
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// загрузка json файла и дессериализация данных для коллекции должностей ListRole
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Role> LoadRole()
        {
            _jsonRoles = File.ReadAllText(path);
            if (_jsonRoles != null)
            {
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>>(_jsonRoles);
                return ListRole;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Нахождение максимального Id в коллекции
        /// </summary>
        /// <returns></returns>
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        /// <summary>
        /// Сохранение json-строки с данными по должностям в файл
        /// </summary>
        /// <param name="listRole"></param>
        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            var jsonRole = JsonConvert.SerializeObject(listRole);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]
string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}