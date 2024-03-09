using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class Dh
{
    public int Dhid { get; set; }

    public string Dhcode { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
