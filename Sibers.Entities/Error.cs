using System;
using System.Runtime.CompilerServices;

namespace Sibers.Entities
{
    public sealed class Error : IEquatable<Error>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Error"/>.
        /// </summary>
        /// <param name="code">Код ошибки.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static Error CreateFromResourceKey(string resourceKey,
            [CallerArgumentExpression("resourceKey")]
            string? paramName = null) => new Error(paramName, resourceKey);

        /// <summary>
        /// Получает пустой экземпляр ошибки.
        /// </summary>
        public static Error None => new(string.Empty, string.Empty);

        /// <summary>
        /// Получает код ошибки.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Получает сообщение об ошибке.
        /// </summary>
        public string Message { get; }

        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

        public static bool operator ==(Error a, Error b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error a, Error b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(Error other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code && Message == other.Message;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is not Error error)
            {
                return false;
            }

            return Equals(error);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Code, Message);
    }
}
