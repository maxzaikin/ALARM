namespace Alarm.Models.Da
{
    public class AddressData
    {
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryIsoCode { get; set; }
        public string FederalDistrict { get; set; }
        public string RegionFiasId { get; set; }
        public string RegionKladrId { get; set; }
        public string RegionIsoCode { get; set; }
        public string RegionWithType { get; set; }
        public string RegionType { get; set; }
        public string RegionTypeFull { get; set; }
        public string Region { get; set; }
        public string CityFiasId { get; set; }
        public string CityKladrId { get; set; }
        public string CityWithType { get; set; }
        public string CityType { get; set; }
        public string CityTypeFull { get; set; }
        public string City { get; set; }
        public string StreetFiasId { get; set; }
        public string StreetKladrId { get; set; }
        public string StreetWithType { get; set; }
        public string StreetType { get; set; }
        public string StreetTypeFull { get; set; }
        public string Street { get; set; }
        public string HouseFiasId { get; set; }
        public string HouseKladrId { get; set; }
        public string House { get; set; }
        public string Block { get; set; }
        public string Flat { get; set; }
        public string FiasId { get; set; }
        public string FiasCode { get; set; }
        public string FiasLevel { get; set; }
        public string KladrId { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public string BeltwayHit { get; set; }
        public string Timezone { get; set; }
    }

    public class Address
    {
        public string Value { get; set; }
        public string UnrestrictedValue { get; set; }
        public AddressData Data { get; set; }
    }

    public class Management
    {
        public string Name { get; set; }
        public string Post { get; set; }
        public long? StartDate { get; set; }
        public bool? Disqualified { get; set; }
    }

    public class State
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public long ActualityDate { get; set; }
        public long RegistrationDate { get; set; }
        public long? LiquidationDate { get; set; }
    }

    public class Opf
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Full { get; set; }
        public string Short { get; set; }
    }

    public class Name
    {
        public string FullWithOpf { get; set; }
        public string ShortWithOpf { get; set; }
        public string Latin { get; set; }
        public string Full { get; set; }
        public string Short { get; set; }
    }

    public class DaData
    {
        public string Inn { get; set; }
        public string Ogrn { get; set; }
        public string Okpo { get; set; }
        public string Okato { get; set; }
        public string Oktmo { get; set; }
        public string Okogu { get; set; }
        public string Okfs { get; set; }
        public string Okved { get; set; }
        public State State { get; set; }
        public Opf Opf { get; set; }
        public Name Name { get; set; }
        public Address Address { get; set; }
        public Management Management { get; set; }
    }

    public class Suggestion
    {
        public string Value { get; set; }
        public string UnrestrictedValue { get; set; }
        public DaData Data { get; set; }
    }

    public class DaDataResponse
    {
        public List<Suggestion> Suggestions { get; set; }
    }

}
