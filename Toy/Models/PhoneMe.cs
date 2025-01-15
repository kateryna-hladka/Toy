using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class PhoneMe
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public TimeOnly? Time { get; set; }

    public DateOnly? Date { get; set; }
}
