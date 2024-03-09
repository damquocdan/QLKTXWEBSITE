using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class Occupancy
{
    public int OccupancyId { get; set; }

    public int? StudentId { get; set; }

    public int? RoomId { get; set; }

    public DateTime? RenewalDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int? CycleMonths { get; set; }

    public bool? Status { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Student? Student { get; set; }
}
