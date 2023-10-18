using Sibers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.BL
{
   public static class Program
    {
        public static async Task SeedData(this IServiceProvider provider)
        {
            await provider.Seed();
        }
    }
}
