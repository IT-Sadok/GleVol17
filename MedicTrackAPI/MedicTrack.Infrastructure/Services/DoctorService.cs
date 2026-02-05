using MedicTrack.Application.Doctors.Responses;
using MedicTrack.Application.Interfaces;
using MedicTrack.Domain.Entities;
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

    public async Task<List<DoctorUnavailabilityResponse>> GetUnavailabilityByDoctorIdAsync(Guid doctorId)
    {
        return await db.DoctorUnavailabilities
            .Where(u => u.DoctorId == doctorId)
            .Select(u => new DoctorUnavailabilityResponse
            {
                Id = u.Id,
                DoctorId = u.DoctorId,
                Reason = u.Reason
            })
            .ToListAsync();
    }
    
    public async Task<Guid> CreateAsync(CreateDoctorResponse request)
    {
        var newDoctor = new Doctor
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Specialization = request.Specialization
        };

        db.Doctors.Add(newDoctor);
        await db.SaveChangesAsync();

        return newDoctor.Id;
    }
}