﻿namespace AffiliatePMS.Application.Common
{
    public interface IQueryHandler<TQuery, TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}
