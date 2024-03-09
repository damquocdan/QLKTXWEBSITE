using System;
using System.Collections.Generic;

namespace QLKTXWEBSITE.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? StudentId { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }

    public string? TransactionCode { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Student? Student { get; set; }
}
