namespace Sibers.Entities.Responses
{
    public record EntityCreatedResponse(Guid Id) : BaseResponse<Guid>(Id);
}