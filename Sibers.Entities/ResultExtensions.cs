using System;
using System.Threading.Tasks;

namespace Sibers.Entities
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Убедитесь, что указанный предикат истинен, в противном случае вернет провальный результат с указанной ошибкой.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="predicate">Предикат.</param>
        /// <param name="error">Ошибка.</param>
        /// <returns>
        /// Успешный результат, если предикат истинен и текущий результат успешен, в противном случае провальный результат.
        /// </returns>
        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
        {
            if (result.IsFailure)
            {
                return result;
            }

            return result.IsSuccess && predicate(result.Value) ? result : Result.Failure<T>(error);
        }

        /// <summary>
        /// Отобразить значение результата на новое значение на основе указанной функции отображения.
        /// </summary>
        /// <typeparam name="TIn">Тип результата.</typeparam>
        /// <typeparam name="TOut">Тип результата после отображения.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="func">Функция отображения.</param>
        /// <returns>
        /// Успешный результат с отображенным значением, если текущий результат успешен, в противном случае провальный результат.
        /// </returns>
        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func) =>
            result.IsSuccess ? func(result.Value) : Result.Failure<TOut>(result.Error);
        public static async ValueTask<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, ValueTask<Result>> func) =>
            result.IsSuccess ? await func(result.Value) : Result.Failure(result.Error);

        public static async ValueTask<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, ValueTask<Result<TOut>>> func) =>
            result.IsSuccess ? await func(result.Value) : Result.Failure<TOut>(result.Error);

        /// <summary>
        /// Сопоставление успешности результата с соответствующими функциями.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="resultTask">Результат задачи.</param>
        /// <param name="onSuccess">Функция в случае успеха.</param>
        /// <param name="onFailure">Функция в случае провала.</param>
        /// <returns>
        /// Результат функции в случае успеха, в противном случае результат провального результата.
        /// </returns>
        public static async ValueTask<T> Match<T>(this ValueTask<Result> resultTask, Func<T> onSuccess, Func<Error, T> onFailure)
        {
            Result result = await resultTask;

            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }

    
        public static async ValueTask<TOut> Match<TIn, TOut>(
            this ValueTask<Result<TIn>> resultTask,
            Func<TIn, TOut> onSuccess,
            Func<Error, TOut> onFailure)
        {
            Result<TIn> result = await resultTask;

            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
        }
    }
}
