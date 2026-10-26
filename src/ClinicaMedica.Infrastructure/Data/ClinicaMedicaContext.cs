
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Data;

public class ClinicaMedicaContext : DbContext
{
    public ClinicaMedicaContext(
        DbContextOptions<ClinicaMedicaContext> options)
        : base(options)
    {
    }
}
