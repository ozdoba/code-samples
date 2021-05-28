namespace Payroll.Application.Employees.Queries.ListEmployees
{
    public enum EmployeeStatusType
    {
        /// <summary>
        /// The employee is registered and awaiting approval of the
        /// identity documents
        /// </summary>
        AwaitingApproval,

        /// <summary>
        /// The employee has been approved
        /// </summary>
        Approved,

        /// <summary>
        /// The employee is marked as active
        /// </summary>
        Active,

        /// <summary>
        /// The employee has been deactivated
        /// </summary>
        Inactive,

        /// <summary>
        /// The employee has been terminated
        /// </summary>
        Terminated,
    }
}