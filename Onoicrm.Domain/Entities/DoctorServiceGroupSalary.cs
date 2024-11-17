namespace Onoicrm.Domain.Entities;

public class DoctorServiceGroupSalary : Entity
{
    public long ClinicId { get; set; }
    public double Salary { get; set; }
    public SalaryType SalaryType { get; set; } = SalaryType.Percent;
    public long DoctorId { get; set; }
    public UserProfile? Doctor { get; set; }
    public long ServiceGroupId { get; set; }
    public ServiceGroup? ServiceGroup { get; set; }
    protected override int GetClassId() =>  ClassNames.DoctorServiceSalary;
}