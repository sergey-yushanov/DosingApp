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
        Fields,
        Users
    }

    public enum MenuItemAccess
    {
        MainMenu,
        MainParams,
        AdditionalParams,
        Admin
    }

    class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public MenuItemAccess Access { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
