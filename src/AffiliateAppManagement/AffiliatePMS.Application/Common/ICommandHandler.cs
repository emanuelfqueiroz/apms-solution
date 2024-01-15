namespace AffiliatePMS.Application.Common
{
    public interface ICommandHandler<TCommand, TResponse>
    {
        Task<CommandResponse<TResponse?>> HandleAsync(TCommand command);
    }
}
