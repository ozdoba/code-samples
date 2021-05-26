namespace Payroll.Application.Common.Interfaces
{
    public interface ICountryLookup
    {
        public bool IsKnownIsoCode(string isoCode);
        
        
        public class Country
        {
            public string Name { get; private set; }
            public string IsoCode2Cc { get; private set; }

            private Country(string name, string isoCode2Cc)
                => (Name, IsoCode2Cc) = (name, isoCode2Cc);

            public static Country FromString(string name, string iso2Cc) => new Country(name, iso2Cc);
        }
    }
}