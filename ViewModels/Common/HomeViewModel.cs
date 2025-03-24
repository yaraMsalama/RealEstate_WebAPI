using RealEstate_WebAPI.ViewModels.Property;

namespace RealEstate_WebAPI.ViewModels.Common
{
    public class HomeViewModel
    {
        public List<PropertyViewModel>? FeaturedProperties { get; set; }

        public List<PropertyViewModel>? RecentProperties { get; set; }

    }
}
