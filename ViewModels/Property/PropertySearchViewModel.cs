using RealEstate_WebAPI.ViewModels.Common;

namespace RealEstate_WebAPI.ViewModels.Property
{
    public class PropertySearchViewModel
    {
        public List<PropertyViewModel>? Properties { get; set; }

        public PropertySearchFilterViewModel Filters { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}