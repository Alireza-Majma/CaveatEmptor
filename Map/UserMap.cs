using System;
using CaveatEmptor.Model;
using FluentNHibernate.Mapping;

namespace CaveatEmptor.Map
{
    public class UserMap : ClassMap<User>
    {
        public const String TableName = "User";
        private const String HomeAddresscolumnPrefix = "_HOME";
        private const String BillingAddresscolumnPrefix = "_BILLING";
        public UserMap()
        {
            Table(TableName);

            Id(x => x.Id, "ID").GeneratedBy.Native();

            Map(x => x.Version, "VERSION");

            Map(x => x.Firstname, "FIRST_NAME").Not.Nullable();

            Map(x => x.Lastname, "LAST_NAME").Not.Nullable();

            Map(x => x.Username, "USERN_AME").Unique().Not.Nullable();

            Map(x => x.Password, "PASSWORD").Not.Nullable();

            Map(x => x.Email, "EMAIL");

            Map(x => x.Ranking, "RANKING");

            Component(x => x.HomeAddress, a =>
            {
                a.Map(x => x.Zipcode, HomeAddresscolumnPrefix + "ZIPCODE");
                a.Map(x => x.Country, HomeAddresscolumnPrefix + "COUNTRY");
                a.Map(x => x.City, HomeAddresscolumnPrefix + "CITY");
                a.Map(x => x.Street, HomeAddresscolumnPrefix + "STREET");
                a.Map(x => x.AddressLine, HomeAddresscolumnPrefix + "ADDRESS_LINE");
            });

            Component(x => x.BillingAddress, a =>
            {
                a.Map(x => x.Zipcode, BillingAddresscolumnPrefix + "ZIPCODE");
                a.Map(x => x.Country, BillingAddresscolumnPrefix + "COUNTRY");
                a.Map(x => x.City, BillingAddresscolumnPrefix + "CITY");
                a.Map(x => x.Street, BillingAddresscolumnPrefix + "STREET");
                a.Map(x => x.AddressLine, BillingAddresscolumnPrefix + "ADDRESS_LINE");
            });

            Map(x => x.Created, "CREATED");

            Map(x => x.IsAdmin, "ISADMIN");


        }
    }
}
