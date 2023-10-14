using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Desapegando.Data.Repository
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        public CompraRepository(DesapegandoDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Compra>> ReadExpression(Expression<Func<Compra, bool>> predicateExpression)
        {
            return await DbSet.AsNoTracking()
                                .Where(predicateExpression)
                                .Include(x => x.Produto).Include(y => y.Produto.ProdutoImagens).Include(z => z.Produto.ProdutoCurtidas)
                                .ToListAsync();
        }
    }
}
