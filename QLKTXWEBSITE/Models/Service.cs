using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public int? Month { get; set; }

    public decimal? Price { get; set; }

    public int? RoomId { get; set; }

    public int? StudentId { get; set; }

    public bool? Status { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Student? Student { get; set; }
}
