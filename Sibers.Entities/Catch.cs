using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities
{
    public sealed class Catch<T> : IEquatable<Catch<T>>
    {
        private readonly T _value;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Catch{T}"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        private Catch(T value) => _value = value;

        /// <summary>
        /// Получает экземпляр по умолчанию.
        /// </summary>
        public static Catch<T> None => new(default);

        /// <summary>
        /// Получает значение, указывающее на существование значения.
        /// </summary>
        public bool HasValue => !HasNoValue;

        /// <summary>
        /// Получает значение, указывающее на отсутствие значения.
        /// </summary>
        public bool HasNoValue => _value is null;

        /// <summary>
        /// Получает значение.
        /// </summary>
        public T Value => HasValue
            ? _value
            : throw new InvalidOperationException("The value is not available as it is missing.");

        public static implicit operator Catch<T>(T value) => From(value);

        /// <summary>
        /// Создает новый экземпляр <see cref="Catch{T}"/> на основе указанного значения.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Новый экземпляр <see cref="Catch{T}"/>.</returns>
        public static Catch<T> From(T value) => new(value);

        /// <inheritdoc />
        public bool Equals(Catch<T> other)
        {
            if (other is null)
            {
                return false;
            }

            if (HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (HasNoValue || other.HasNoValue)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj switch
            {
                null => false,
                T value => Equals(new Catch<T>(value)),
                Catch<T> maybe => Equals(maybe),
                _ => false
            };

        /// <inheritdoc />
        public override int GetHashCode() => HasValue ? Value.GetHashCode() : 0;
    }
}
