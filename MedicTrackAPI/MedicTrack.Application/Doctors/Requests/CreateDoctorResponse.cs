namespace MedicTrack.Application.Doctors.Responses;

public class CreateDoctorRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
}