namespace EmployeeRecordSystem.Server.Installers.Helpers;

public class ServiceAttributes
{
    public class ScopedRegistrationAttribute : Attribute { }
    public class SingletonRegistrationAttribute : Attribute { }
    public class TransientRegistrationAttribute : Attribute { }
}