using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Domain
{
    public static class Program
    {
        public static void AddSqlDbCore(this IServiceCollection services, string con)
        {
            services.AddDbContext<SibersContext>(options =>
                options.UseSqlServer(con));
        }
    }
}
