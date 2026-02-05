using MedicTrack.Application.Doctors.Responses;

namespace MedicTrack.Application.Interfaces;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorListItemResponse>> GetDoctorsAsync();
}