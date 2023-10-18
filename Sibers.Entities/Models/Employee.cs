using Sibers.Entities.Base;
using Sibers.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibers.Entities.Models
{
    public record Employee : Entity<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Patronymic { get; set; }

        [EmailAddress(ErrorMessage = "ERROR Email Address")]
        public required string Email { get; set; }

        public required Role Role { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
