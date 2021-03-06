﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using University_students.Enums;

namespace University_students.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Login { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public int? PulpitId { get; set; }
        public virtual Pulpit Pulpit { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group{ get; set; }
        public Role TypeUser { get; set; }
        public virtual Teaching Teaching { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<SubjectProgress> SubjectsProgress { get; set; }
        public User()
        {
            SubjectsProgress = new List<SubjectProgress>();
            Subjects = new List<Subject>();
        }

        public override string ToString() => FirstName + " " + LastName;
    }

}
