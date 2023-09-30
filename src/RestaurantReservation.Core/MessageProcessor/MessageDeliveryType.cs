namespace RestaurantReservation.Core.MessageProcessor;

[Flags]
public enum MessageDeliveryType
{
    Outbox = 1,
    Inbox = 2,
    Internal = 4
}

public enum MessageStatus
{
    InProgress = 1,
    Processed = 2
}
