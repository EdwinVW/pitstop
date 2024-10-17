namespace Pitstop.RepairManagemenAPI.Commands
{
    public class ReceiveCustomerNotification : Command
    {
        public readonly Guid notificationId;
        public readonly string CustomerId;

        public readonly bool ApprovalStatus;

        public readonly DateTime ResponseTime;

        public ReceiveCustomerNotification(
            Guid notificationId,
            string customerId,
            bool approvalStatus,
            DateTime responseTime,
        ) : base(messageId)
        {
            RequestId = requestId;
            CustomerId = customerId;
            ApprovalStatus = approvalStatus;
            ResponseTime = responseTime;
        }
    }
}
