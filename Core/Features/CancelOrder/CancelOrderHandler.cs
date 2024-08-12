using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data;
using Core.Data.Entity;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Result<CancelOrderResponse>>
    {
        private readonly ApplicationDbContext _context;

        public CancelOrderHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CancelOrderResponse>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProcess) 
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == request.UserId, cancellationToken);

            if (order == null)
            {
                return Result.Failure<CancelOrderResponse>(new Error("OrderNotFound", "Order not found or you do not have permission to cancel this order."));
            }

            var orderProcess = order.OrderProcess;

            if (orderProcess == null || orderProcess.Status != OrderProcessStatus.Pending)
            {
                return Result.Failure<CancelOrderResponse>(new Error("OrderNotCancellable", "This order cannot be canceled because it is either completed or already canceled."));
            }

            orderProcess.Status = OrderProcessStatus.Canceled;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CancelOrderResponse
            {
                IsSuccess = true,
                Message = $"Order {order.Id} has been successfully canceled."
            });
        }
    }

}