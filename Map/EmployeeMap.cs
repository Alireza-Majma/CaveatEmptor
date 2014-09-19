using CaveatEmptor.Model;
using FluentNHibernate.Mapping;

namespace CaveatEmptor.Map
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("NEMPLOYEE");
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
        }
    }
}
