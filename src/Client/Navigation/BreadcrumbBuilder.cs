using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeRecordSystem.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace EmployeeRecordSystem.Client.Navigation;

public class BreadcrumbBuilder
{
    private readonly Guid _employeeId;
    public BreadcrumbBuilder(AuthenticationStateProvider authenticationStateProvider)
    {
        _employeeId = authenticationStateProvider.GetUserIdAsync().Result;
    }

    private readonly List<BreadcrumbItem> _breadcrumbs = new();

    public BreadcrumbBuilder AddHome()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.Home, href: "/", icon: MyIcons.Home));
        return this;
    }
        
    public BreadcrumbBuilder AddEmployeeDetails()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.EmployeeDetails, href: $"/employee/details/{_employeeId}", icon: MyIcons.EmployeeDetails));
        return this;
    }
        
    public BreadcrumbBuilder AddEmployeeBalanceLogs()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.BalanceLogs, href: $"/employee/balance-logs/{_employeeId}", icon: MyIcons.BalanceLogs));
        return this;
    }

    public BreadcrumbBuilder AddEmployeeWithdrawalRequests()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.WithdrawalRequests, href: $"/employee/withdrawal-requests/{_employeeId}", icon: MyIcons.WithdrawalRequests));
        return this;
    }
        
    public BreadcrumbBuilder AddPendingWithdrawalRequests()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.PendingWithdrawalRequests, href: "/withdrawal-requests/pending", icon: MyIcons.PendingWithdrawalRequests));
        return this;
    }
        
    public BreadcrumbBuilder AddGroups()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.Groups, href: "/groups", icon: MyIcons.Groups));
        return this;
    }

    public List<BreadcrumbItem> Build() => _breadcrumbs;
}