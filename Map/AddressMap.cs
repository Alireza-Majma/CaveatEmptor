using System;
using CaveatEmptor.Model;
using FluentNHibernate.Mapping;

namespace CaveatEmptor.Map
{
    public class AddressMap
    {
        public const string TableName = "ADDRESS";

        public static Action<ComponentPart<Address>> WithColumnPrefix(string columnPrefix)
        {
            return a =>
            {
                a.Map(x => x.Zipcode, columnPrefix + "ZIPCODE");
                a.Map(x => x.Country, columnPrefix + "COUNTRY");
                a.Map(x => x.City, columnPrefix + "CITY");
                a.Map(x => x.Street, columnPrefix + "STREET");
                a.Map(x => x.AddressLine, columnPrefix + "ADDRESS_LINE");
            };

        }
    }
}
