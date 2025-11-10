using Yakihouse.Domain.Entities;

namespace Yakihouse.Application.Kitchen.Services;

public interface IKitchenTicketService
{
    Task<List<KitchenTicket>> CreateTicketsForOrderAsync(Order order, CancellationToken cancellationToken = default);
}

