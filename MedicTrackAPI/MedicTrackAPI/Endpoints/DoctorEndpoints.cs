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

        return group;
    }
}