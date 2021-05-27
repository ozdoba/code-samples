namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public enum PaycycleStatus
    {
        Initiated,
        Submitted,
        AwaitingApproval,
        AwaitingFunds,
        Funded,
        PendingProcessing,
        Processed,
    }
}