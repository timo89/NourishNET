using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
using NourishNet.Repository.Repositories.Interfaces;


namespace NourishNet.Repository.Repositories.Implementations;
public class CourierRepository : BaseRepository<Courier>, ICourierRepository
{
    public CourierRepository(DbContext context) : base(context)
    {
    }
}