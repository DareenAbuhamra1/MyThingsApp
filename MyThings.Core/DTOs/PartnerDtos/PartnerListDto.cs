namespace MyThings.Core.Dto
{
    public class PartnerListDto
    {
        public int Id {get;set;}
        public string Name {get;set;} = null!;
        public string Area {get;set;} = null!;
        public string? DescriptionEn {get;set;}
        public string? DescriptionAr {get;set;}
        public decimal Rating {get;set;}
        public int RatingCount {get;set;}
        public decimal Latitude {get;set;}
        public decimal Longitude {get;set;}
        public double Distance {get;set;}
    }
}