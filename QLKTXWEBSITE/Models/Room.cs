using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? Mowroom { get; set; }

    public string? Building { get; set; }

    public int? Floor { get; set; }

    public int? NumberRoom { get; set; }

    public int? BedNumber { get; set; }

    public int? NumberOfStudents { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BedOfRoom> BedOfRooms { get; set; } = new List<BedOfRoom>();

    public virtual ICollection<Occupancy> Occupancies { get; set; } = new List<Occupancy>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
