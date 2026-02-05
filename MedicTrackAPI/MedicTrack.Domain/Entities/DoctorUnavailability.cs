namespace MedicTrack.Domain.Entities;

public class DoctorUnavailability
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }

    public string? Reason { get; set; }
} 