namespace MedicTrack.Application.Doctors.Responses;

public class DoctorUnavailabilityResponse
{
    public Guid Id { get; init; }

    public Guid DoctorId { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public string Reason { get; init; } = string.Empty;
}