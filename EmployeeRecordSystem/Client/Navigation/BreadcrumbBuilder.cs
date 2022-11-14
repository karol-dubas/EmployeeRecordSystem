using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Navigation;

public class BreadcrumbBuilder
{
    private readonly List<BreadcrumbItem> _breadcrumbs = new();

    public BreadcrumbBuilder AddHome()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.Home, href: "#", icon: MyIcons.Home));
        return this;
    }
        
    public BreadcrumbBuilder AddEmployeeDetails()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.EmployeeDetails, href: "#", icon: MyIcons.EmployeeDetails));
        return this;
    }
        
    public BreadcrumbBuilder AddEmployeeBalanceLogs()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.BalanceLogs, href: "#", icon: MyIcons.BalanceLogs));
        return this;
    }

    public BreadcrumbBuilder AddEmployeeWithdrawalRequests()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.WithdrawalRequests, href: "#", icon: MyIcons.WithdrawalRequests));
        return this;
    }
        
    public BreadcrumbBuilder AddPendingWithdrawalRequests()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.PendingWithdrawalRequests, href: "#", icon: MyIcons.PendingWithdrawalRequests));
        return this;
    }
        
    public BreadcrumbBuilder AddGroups()
    {
        _breadcrumbs.Add(new BreadcrumbItem(PageNames.Groups, href: "#", icon: MyIcons.Groups));
        return this;
    }

    public List<BreadcrumbItem> Build() => _breadcrumbs;
}