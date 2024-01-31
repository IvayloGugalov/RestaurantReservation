using FluentValidation;

namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class UpdateReviewHandler : ICommandHandler<UpdateReservation, UpdateReservationResult>
{
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;
    private readonly IMongoRepository<Table, TableId> tableRepository;

    public UpdateReviewHandler(
        IMongoRepository<Reservation, ReservationId> reservationRepository,
        IMongoRepository<Table, TableId> tableRepository)
    {
        this.reservationRepository = reservationRepository;
        this.tableRepository = tableRepository;
    }

    public async Task<UpdateReservationResult> Handle(UpdateReservation request, CancellationToken ct)
    {
        var reservation = await this.reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (reservation == null) throw new ReservationNotFoundException();

        var table = await this.tableRepository.GetByIdAsync(request.TableId, ct);
        if (table == null) throw new TableNotFoundException();

        reservation.Update(
            request.ReservationId,
            request.RestaurantId,
            table,
            request.CustomerId,
            request.ReservationStatus,
            request.ReservationDate,
            request.Occupants,
            request.ReviewId);

        await this.reservationRepository.UpdateAsync(reservation, ct);

        return new UpdateReservationResult(
            reservation.Id,
            reservation.RestaurantId,
            reservation.TableId,
            reservation.CustomerId,
            reservation.Status,
            reservation.ReservationDate,
            reservation.Occupants,
            reservation.ReviewId);
    }

    public class UpdateReservationValidator : AbstractValidator<UpdateReservation>
    {
        public UpdateReservationValidator()
        {
            RuleFor(x => x.ReservationId).NotEmpty().WithMessage("ReservationId must be not empty");
            RuleFor(x => x.RestaurantId).NotEmpty().WithMessage("RestaurantId must be not empty");
            RuleFor(x => x.TableId).NotEmpty().WithMessage("TableId must be not empty");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId must be not empty");

            RuleFor(x => x.ReservationStatus)
                .Must(p => (p.GetType().IsEnum &&
                    p == ReservationStatus.Created ||
                    p == ReservationStatus.Confirmed ||
                    p == ReservationStatus.Unpaid ||
                    p == ReservationStatus.Paid ||
                    p == ReservationStatus.Delayed ||
                    p == ReservationStatus.Canceled ||
                    p == ReservationStatus.Completed))
                .WithMessage($"Status must be {string.Join(" ", Enum.GetValues(typeof(ReservationStatus)))}");

            RuleFor(x => x.Occupants).GreaterThan((ushort)0).WithMessage("Occupants must be greater than 0");
            RuleFor(x => x.ReservationDate).NotEmpty().WithMessage("ReservationDate must be not empty");
        }
    }
}
