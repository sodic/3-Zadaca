﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    public class AddTodoViewModel
    {
        [Required]
        [MinLength(3)]
        public string Text { get; set; }
    }
}
