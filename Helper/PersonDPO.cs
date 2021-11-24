using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using WpfApplDemo2018.Annotations;
using System.Threading.Tasks;
using WpfApplDemo2018.Model;
using WpfApplDemo2018.ViewModel;
using Hangfire.Annotations;

namespace WpfApplDemo2018.Helper
{
    public class PersonDPO : INotifyPropertyChanged
    {
        public PersonDPO CopyFromPerson(Person person) 
        {
            PersonDPO perDPO = new PersonDPO();
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;
            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }
            if (role != string.Empty)
            {
                perDPO.Id = person.Id;
                perDPO.RoleName = role;
                perDPO.FirstName = person.FirstName;
                perDPO.LastName = person.LastName;
                perDPO.Birthday = person.Birthday;
            }
            return perDPO;
        }        public PersonDPO ShallowCopy() 
        {
            return (PersonDPO)this.MemberwiseClone();
        }
        public int Id { get; set; } 
        /// <summary>
        /// должность сотрудника
        /// </summary>
        private string _roleName;
        /// <summary>
        /// должность сотрудника
        /// </summary>
        public string RoleName
        {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged("RoleName");
            }
        }
        /// <summary>
        /// имя сотрудника
        /// </summary>
        private string firstName;
        /// <summary>
        /// имя сотрудника
        /// </summary>
        public string FirstName
        {
        get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        private string lastName;
        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        /// <summary>
        /// дата рождения сотрудника
        /// </summary>
        private DateTime birthday;
        /// <summary>
        /// дата рождения сотрудника
        /// </summary>
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        public PersonDPO() { } 
        public PersonDPO(int id, string role, string firstName, string lastName, DateTime birthday) 
        {
            this.Id = id;
            this.RoleName = role;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
