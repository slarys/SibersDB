using Microsoft.AspNetCore.Authorization;

namespace SibersApi
{
    // Обработчик для проверки наличия указанной области (scope) в токенах доступа пользователя
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasScopeRequirement requirement
        )
        {
            // Если у пользователя нет утверждения (claim) о наличии области (scope), завершаем проверку
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Разделяем строку областей на массив
            var scopes = context.User
                .FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)
                ?.Value.Split(' ');

            if (scopes is null) return Task.CompletedTask;

            // Успех, если массив областей содержит необходимую область
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    // Требование для проверки наличия указанной области (scope) в токенах доступа пользователя
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
