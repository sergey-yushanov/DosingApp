using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public enum MenuItemType
    {
        Mixtures,
        Reports,
        Assignments,
        Recipes,
        Components,
        Applicators,
        Facilities,
        Transports,
        Crops,
        ProcessingTypes,
        AgrYears,
        Fields
    }

    class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
    }
}
