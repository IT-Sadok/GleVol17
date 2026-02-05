using MedicTrack.Application.Doctors.Responses;

namespace MedicTrack.Application.Interfaces;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorListItemResponse>> GetDoctorsAsync();
    Task<List<DoctorUnavailabilityResponse>> GetUnavailabilityByDoctorIdAsync(Guid doctorId);
    Task<Guid> CreateAsync(CreateDoctorRequest response);
}