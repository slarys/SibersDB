using System;
using System.Threading.Tasks;

namespace Sibers.Entities
{
    public static class FunctionalExtensions
    {
        public static async Task<TOut> Map<TIn, TOut>(this ValueTask<TIn> task, Func<TIn, TOut> func) => func(await task);
    }
}
