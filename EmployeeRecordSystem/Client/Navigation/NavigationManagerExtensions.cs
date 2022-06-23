using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Navigation
{
    public static class NavigationManagerExtensions
    {
        public static void NavigateToEmployeeBalanceLogs(this NavigationManager navigationManager, Guid employeeId)
        {
            navigationManager.NavigateTo($"/employee/balance-logs/{employeeId}");
        }

        public static void NavigateToEmployeeWithdrawalRequests(this NavigationManager navigationManager, Guid employeeId)
        {
            navigationManager.NavigateTo($"/employee/withdrawal-requests/{employeeId}");
        }

        public static void NavigateToEmployeeDetails(this NavigationManager navigationManager, Guid employeeId)
        {
            navigationManager.NavigateTo($"employee/details/{employeeId}");
        }

        public static void NavigateToPendingWithdrawalRequests(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("withdrawal-requests/pending");
        }

        public static void NavigateToGroups(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("groups");
        }

        public static void NavigateToGroup(this NavigationManager navigationManager, Guid groupId)
        {
            navigationManager.NavigateTo($"groups/{groupId}");
        }
    }
}
