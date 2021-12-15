using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApplDemo2018.Helper;
using WpfApplDemo2018.Model;
using WpfApplDemo2018.View;

namespace WpfApplDemo2018.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        readonly string path = @"D:\1. Универcитет\5-ый семестр\Разработка и сопровождение программных систем\Лаба1\WpfApplDemo2018\DataModels\PersonData.json";
        private PersonDPO _selectedPersonDPO;
        /// <summary>
        /// выделенные в списке данные по сотруднику
        /// </summary>
        public PersonDPO SelectedPersonDPO
        {
            get { return _selectedPersonDPO; }
            set
            {
                _selectedPersonDPO = value;
                OnPropertyChanged("SelectedPersonDPO");
            }
        }
        /// <summary>
        /// коллекция данных по сотрудникам
        /// </summary>
        public ObservableCollection<Person> ListPerson { get; set; }
        public ObservableCollection<PersonDPO> ListPersonDPO
        {
            get;
            set;
        }
        string _jsonPersons = String.Empty;
        public string Error { get; set; }
        public string Message { get; set; }
        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDPO = new ObservableCollection<PersonDPO>();
            ListPerson = LoadPerson();
            ListPersonDPO = GetListPersonDPO();
        }
        #region AddPerson
        /// <summary>
        /// добавление сотрудника
        /// </summary>
        private RelayCommand _addPerson;
        /// <summary>
        /// добавление сотрудника
        /// </summary>
        public RelayCommand AddPerson
        {
            get
            {
                return _addPerson ??
                (_addPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title = "Новый сотрудник"
                    };
                    // формирование кода нового собрудника
                    int maxIdPerson = MaxId() + 1;
                    PersonDPO per = new PersonDPO
                    {
                        Id = maxIdPerson,
                        Birthday = DateTime.Now.ToString(),
                    };

                    wnPerson.DataContext = per;
                    if (wnPerson.ShowDialog() == true)
                    {
                        var r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            per.RoleName = r.NameRole;
                            per.Birthday = PersonDPO.GetStringBirthday(per.Birthday);
                            ListPersonDPO.Add(per);
                            // добавление нового сотрудника в коллекциюListRoles<Person>
                            Person p = new Person();
                            p = p.CopyFromPersonDPO(per);
                            ListPerson.Add(p);
                            try
                            {
                            // сохранение изменений в файле json
                            SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка добавления данных в json файл\n" + e.Message;
                            }
                        }
                    }
               
                },
                (obj) => true));
            }
        }
        #endregion
        #region EditPerson
        /// команда редактирования данных по сотруднику
        private RelayCommand _editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return _editPerson ??
                (_editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee()
                    {
                        Title = "Редактирование данных сотрудника",
                    };
                    PersonDPO personDPO = SelectedPersonDPO;
                    var tempPerson = personDPO.ShallowCopy();
                    wnPerson.DataContext = tempPerson;

                    if (wnPerson.ShowDialog() == true)
                    {
                    // сохранение данных в оперативной памяти
                    // перенос данных из временного класса в класс отображения данных
                    var r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            personDPO.RoleName = r.NameRole;
                            personDPO.FirstName = tempPerson.FirstName;
                            personDPO.LastName = tempPerson.LastName;
                            personDPO.Birthday = PersonDPO.GetStringBirthday(tempPerson.Birthday);
                            // перенос данных из класса отображения данных в класс Person
        
                            var per = ListPerson.FirstOrDefault(p => p.Id == personDPO.Id);
                            if (per != null)
                            {
                                per = per.CopyFromPersonDPO(personDPO);
                            }
                            try
                            {
                            // сохраненее данных в файле json
                            SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
            
                                Error = "Ошибка редактирования данных в json файл\n"
                                + e.Message;
                            }
                        }
                        else
                        {
                            Message = "Необходимо выбрать должность сотрудника.";
                        }
                    }
                }, (obj) => SelectedPersonDPO != null && ListPersonDPO.Count > 0));
            }
        }
        #endregion
        #region DeletePerson
        /// команда удаления данных по сотруднику
        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return _deletePerson ??
                (_deletePerson = new RelayCommand(obj =>
                {
                    PersonDPO person = SelectedPersonDPO;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" +

                    person.LastName + " " + person.FirstName,
                    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                        // удаление данных в списке отображения данных
                        ListPersonDPO.Remove(person);
                        // поиск удаляемого класса в коллекции ListRoles
                        var per = ListPerson.FirstOrDefault(p => p.Id == person.Id);
                            if (per != null)
                            {
                                ListPerson.Remove(per);
                        // сохраненее данных в файле json
                        SaveChanges(ListPerson);
                            }
                        }
                        catch (Exception e)
                        {
                            Error = "Ошибка удаления данных\n" + e.Message;
                        }
                    }
               
                }, (obj) => SelectedPersonDPO != null && ListPersonDPO.Count > 0));
            }
        }
        #endregion
        #region Method
        /// <summary>
        /// Загрузка данных по сотрудникам из json файла
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Person> LoadPerson()
        {
            _jsonPersons = File.ReadAllText(path);
            if (_jsonPersons != null)
            {
                ListPerson = JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                return ListPerson;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Формирование коллекции классов PersonDpo из коллекции Person
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PersonDPO> GetListPersonDPO()
        {
            foreach (var person in ListPerson)
            {
                PersonDPO p = new PersonDPO();
                p = p.CopyFromPerson(person);
                ListPersonDPO.Add(p);
            }
            return ListPersonDPO;
        }
        /// <summary>
        /// Нахождение максимального Id в коллекции данных
        /// </summary>
        /// <returns></returns>
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        /// <summary>
        /// Сохранение json-строки с данными по сотрудникам в json файл
        /// </summary>
        /// <param name="listPersons"></param>
        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName]
        string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}