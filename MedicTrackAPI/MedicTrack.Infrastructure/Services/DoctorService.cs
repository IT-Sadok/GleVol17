using MedicTrack.Application.Doctors.Responses;
using MedicTrack.Application.Interfaces;
using MedicTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicTrack.Infrastructure.Services;

public class DoctorService(AppDbContext db) : IDoctorService
{
    public async Task<IReadOnlyList<DoctorListItemResponse>> GetDoctorsAsync()
    {
        return await db.Doctors
            .AsNoTracking()
            .OrderBy(d => d.LastName)
            .ThenBy(d => d.FirstName)
            .Select(d => new DoctorListItemResponse
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization
            })
            .ToListAsync();
    }
}