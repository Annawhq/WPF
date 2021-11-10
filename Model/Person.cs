﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplDemo2018.Helper;
using WpfApplDemo2018.ViewModel;
//using WpfApplDemo2018.Helper.PersonDPO;

namespace WpfApplDemo2018.Model
{
    public class Person
    {
        public Person CopyFromPersonDPO(PersonDPO p)
        {
            RoleViewModel vmRole = new RoleViewModel();
            int roleId = 0;
            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == p.Role)
                {
                    roleId = r.Id;
                    break;
                }
            }
            if (roleId != 0)
            {
                this.Id = p.Id;
                this.RoleId = roleId;
                this.FirstName = p.FirstName;
                this.LastName = p.LastName;
                this.Birthday = p.Birthday;
            }
            return this;
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public Person() { }
        public Person(int id, int roleId, string firstName,
        string lastName, DateTime birthday)
        {
            this.Id = id;
            this.RoleId = roleId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }
    }
}
