﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class UsersNotSercure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class NamePassword
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
