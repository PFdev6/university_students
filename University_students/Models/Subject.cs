﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_students.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Hour { get; set; }
        public virtual ICollection<User> Teachers { get; set; }
        public virtual ICollection<TaughtGroups> TaughtGroups { get; set; }
        public Subject()
        {
            TaughtGroups = new List<TaughtGroups>();
            Teachers = new List<User>();
        }

        public override string ToString()
        {
            return Name + " --- Hours: " + Hour;
        }
    }
}