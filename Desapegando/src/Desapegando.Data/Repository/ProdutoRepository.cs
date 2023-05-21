using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Desapegando.Data.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(DesapegandoDbContext context) : base(context)
    {

    }


    public override async Task<IEnumerable<Produto>> Read()
    {
        return await DbSet.AsNoTracking().Include(x => x.ProdutoImagens).ToListAsync();
    }

    public override async Task<Produto> ReadById(Guid id)
    {
        return await DbSet.AsNoTracking().Where(x => x.Id == id).Include(x => x.ProdutoImagens).FirstOrDefaultAsync();
    }
}