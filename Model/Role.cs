using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
//using WpfApplDemo2018.Annotations;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Annotations;

namespace WpfApplDemo2018.Model
{
    /// <summary>
    /// класс должность сотрудника
    /// </summary>
    public class Role : INotifyPropertyChanged
    {
        /// <summary>
        /// код должности
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// наименование должности
        /// </summary>
        private string nameRole;
        /// <summary>
        /// наименование должности
        /// </summary>
        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
        }
        public Role() { }
        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
        }
        /// <summary>
        /// Метод поверхностного копирования
        /// </summary>
        /// <returns></returns>
        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]    
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
