namespace Payroll.Application.Paycycles.Queries.ListPaycycles
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