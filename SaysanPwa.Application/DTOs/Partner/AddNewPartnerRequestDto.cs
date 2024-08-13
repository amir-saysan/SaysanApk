namespace SaysanPwa.Application.DTOs.Partner;

public record AddNewPartnerRequestDto(string Name, string? NationalId, int GroupId, string? EconomicCode, int JobId, string PhoneNumber, int RegionId, int CityId, string PostalCode, string LandlinePhone, string Address);
