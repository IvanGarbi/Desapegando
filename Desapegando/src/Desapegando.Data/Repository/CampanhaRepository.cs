﻿using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Desapegando.Data.Repository
{
    public class CampanhaRepository : Repository<Campanha>, ICampanhaRepository
    {
        public CampanhaRepository(DesapegandoDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Campanha>> Read()
        {
            return await DbSet.AsNoTracking()
                              .Include(x => x.CampanhaImagens)
                              .ToListAsync();
        }

        public override async Task<Campanha> ReadById(Guid id)
        {
            return await DbSet.AsNoTracking()
                              .Where(x => x.Id == id)
                              .Include(x => x.CampanhaImagens)
                              .FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<Campanha>> ReadExpression(Expression<Func<Campanha, bool>> predicateExpression)
        {
            return await DbSet.AsNoTracking()
                                .Where(predicateExpression)
                                .Include(x => x.CampanhaImagens)
                                .ToListAsync();
        }
    }
}
