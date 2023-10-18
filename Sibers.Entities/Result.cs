using System;

namespace Sibers.Entities
{
    public class Result
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Result"/> с указанными параметрами.
        /// </summary>
        /// <param name="isSuccess">Флаг, указывающий, успешен ли результат.</param>
        /// <param name="error">Ошибка.</param>
        protected Result(bool isSuccess, Error error)
        {
            switch (isSuccess)
            {
                case true when error != Error.None:
                    throw new InvalidOperationException();
                case false when error == Error.None:
                    throw new InvalidOperationException();
                default:
                    IsSuccess = isSuccess;
                    Error = error;
                    break;
            }
        }

        /// <summary>
        /// Получает значение, указывающее, успешен ли результат.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Получает значение, указывающее, провален ли результат.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Получает ошибку.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Возвращает успешный <see cref="Result"/>.
        /// </summary>
        /// <returns>Новый экземпляр <see cref="Result"/> с установленным флагом успешности.</returns>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Возвращает успешный <see cref="Result{TValue}"/> с указанным значением.
        /// </summary>
        /// <typeparam name="TValue">Тип результата.</typeparam>
        /// <param name="value">Значение результата.</param>
        /// <returns>Новый экземпляр <see cref="Result{TValue}"/> с установленным флагом успешности.</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Создает новый экземпляр <see cref="Result{TValue}"/> с указанным значением и указанной ошибкой.
        /// </summary>
        /// <typeparam name="TValue">Тип результата.</typeparam>
        /// <param name="value">Значение результата.</param>
        /// <param name="error">Ошибка, если значение равно null.</param>
        /// <returns>Новый экземпляр <see cref="Result{TValue}"/> с указанным значением или ошибкой.</returns>
        public static Result<TValue> Create<TValue>(TValue value, Error error)
            where TValue : class
            => value is null ? Failure<TValue>(error) : Success(value);

        public static Result<TValue> Create<TValue>(TValue value, string resourceKey)
            where TValue : class
            => value is null ? Failure<TValue>(Entities.Error.CreateFromResourceKey(resourceKey)) : Success(value);

        /// <summary>
        /// Возвращает провальный <see cref="Result"/> с указанной ошибкой.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Новый экземпляр <see cref="Result"/> с указанной ошибкой и флагом провала.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Возвращает провальный <see cref="Result{TValue}"/> с указанной ошибкой.
        /// </summary>
        /// <typeparam name="TValue">Тип результата.</typeparam>
        /// <param name="error">Ошибка.</param>
        /// <returns>Новый экземпляр <see cref="Result{TValue}"/> с указанной ошибкой и флагом провала.</returns>
        /// <remarks>
        /// Мы специально игнорируем здесь присваивание значения с пустым значением, потому что API никогда не позволит его получить.
        /// Значение получается через метод, который вызовет исключение, если результат провален.
        /// </remarks>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        /// <summary>
        /// Возвращает первый провальный результат из указанных <paramref name="results"/>.
        /// Если нет провалов, возвращается успешный результат.
        /// </summary>
        /// <param name="results">Массив результатов.</param>
        /// <returns>
        /// Первый провальный результат из указанного массива <paramref name="results" или успешный результат, если его нет.
        /// </returns>
        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Success();
        }
    }

    /// <summary>
    /// Представляет результат некоторой операции, с информацией о статусе и, возможно, значением и ошибкой.
    /// </summary>
    /// <typeparam name="TValue">Тип значения результата.</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Result{TValueType}"/> с указанными параметрами.
        /// </summary>
        /// <param name="value">Значение результата.</param>
        /// <param name="isSuccess">Флаг, указывающий, успешен ли результат.</param>
        /// <param name="error">Ошибка.</param>
        protected internal Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error)
            => _value = value;

        /// <summary>
        /// Получает значение результата, если результат успешен, в противном случае вызывает исключение.
        /// </summary>
        /// <returns>Значение результата, если результат успешен.</returns>
        /// <exception cref="InvalidOperationException">, когда <see cref="Result.IsFailure"/> равен true.</exception>
        public TValue Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Значение провального результата нельзя получить.");

        public static implicit operator Result<TValue>(TValue value) => Success(value);
    }
}
