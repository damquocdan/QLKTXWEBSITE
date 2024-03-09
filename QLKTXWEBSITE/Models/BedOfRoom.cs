using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class BedOfRoom
{
    public int BedId { get; set; }

    public int? RoomId { get; set; }

    public int? NumberBed { get; set; }

    public bool? Status { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
