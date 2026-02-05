namespace MedicTrack.Domain.Entities;

public class Doctor
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<DoctorUnavailability> Unavailabilities { get; set; } = new List<DoctorUnavailability>();
}