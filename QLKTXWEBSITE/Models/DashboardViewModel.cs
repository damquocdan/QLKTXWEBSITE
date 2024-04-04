namespace QLKTXWEBSITE.Models
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalRooms { get; set; }
        public int TotalBeds { get; set; }
        public double StudentWithoutBedPercentage { get; set; }
        public double BedWithoutStudentPercentage {  get; set; }
        public double TotalFalseServices { get; set; }
        public double TotalServices { get; set; }
        public double TotalFalseServicesPercentage { get; set; }
        public int StudentsWithoutBed { get; set; } // Số sinh viên chưa có giường
        public int BedsWithoutStudent { get; set; } // Số giường chưa có sinh viên
        public int NotPaymoney { get; set; }
        public int TotalPaymoney { get; set;}
        public double NotPaymoneyPercentage { get; set; }
    }
}
