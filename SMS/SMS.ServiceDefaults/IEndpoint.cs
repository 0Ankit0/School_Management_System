using Microsoft.AspNetCore.Routing;

namespace SMS.ServiceDefaults;

public interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
