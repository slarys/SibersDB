using System;
using System.Threading.Tasks;

namespace Sibers.Entities
{
    public static class CatchExtensions
    {
        /// <summary>
        /// Привязывается к результату функции и возвращает его.
        /// </summary>
        /// <typeparam name="TIn">Тип результата.</typeparam>
        /// <typeparam name="TOut">Тип результата после преобразования.</typeparam>
        /// <param name="maybe">Возможное значение.</param>
        /// <param name="func">Функция привязки.</param>
        /// <returns>
        /// Привязанное значение, если в `maybe` есть значение, в противном случае пустой экземпляр `Catch<TOut>`.
        /// </returns>
        public static async ValueTask<Catch<TOut>> Bind<TIn, TOut>(this Catch<TIn> maybe, Func<TIn, ValueTask<Catch<TOut>>> func) =>
            maybe.HasValue ? await func(maybe.Value) : Catch<TOut>.None;

        /// <summary>
        /// Сопоставляет функции в зависимости от наличия значения.
        /// </summary>
        /// <typeparam name="TIn">Тип ввода.</typeparam>
        /// <typeparam name="TOut">Тип вывода.</typeparam>
        /// <param name="resultTask">Задача, представляющая возможное значение.</param>
        /// <param name="onSuccess">Функция при успешном сопоставлении.</param>
        /// <param name="onFailure">Функция при неудачном сопоставлении.</param>
        /// <returns>
        /// Результат функции `onSuccess`, если в `maybe` есть значение, в противном случае результат функции `onFailure`.
        /// </returns>
        public static async Task<TOut> Match<TIn, TOut>(
            this ValueTask<Catch<TIn>> resultTask,
            Func<TIn, TOut> onSuccess,
            Func<TOut> onFailure)
        {
            Catch<TIn> maybe = await resultTask;

            return maybe.HasValue ? onSuccess(maybe.Value) : onFailure();
        }
    }
}
