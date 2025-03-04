﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iprox.Domain.Enums;

namespace Iprox.Domain.Entities;

public class Genre : Base
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}
