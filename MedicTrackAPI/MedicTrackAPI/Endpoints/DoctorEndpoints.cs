using MedicTrack.Application.Doctors.Responses;
using MedicTrack.Application.Interfaces;

namespace MedicTrackAPI.Endpoints;

public static class DoctorEndpoints
{
    public static RouteGroupBuilder MapDoctorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/doctors")
            .WithTags("Doctors");

        group.MapGet("/", async (IDoctorService service) =>
        {
            var doctors = await service.GetDoctorsAsync();
            return Results.Ok(doctors);
        });

        group.MapGet("/{doctorId:guid}/unavailability",
            async (Guid doctorId, IDoctorService service) =>
            {
                var result = await service.GetUnavailabilityByDoctorIdAsync(doctorId);
                return Results.Ok(result);
            });


        group.MapPost("", async (
            CreateDoctorRequest request,
            IDoctorService doctorService) =>
        {
            var id = await doctorService.CreateAsync(request);
            return Results.Created($"/api/doctors/{id}", new { id });
        });

        return group;
    }
}