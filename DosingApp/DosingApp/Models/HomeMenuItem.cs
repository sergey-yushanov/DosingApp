using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
    public enum MenuItemType
    {
        Jobs,
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
        Users,
        Mixers,
        Login
    }

    public static class MenuItemGroup
    {
        public static string MainMenu = "Главное меню";
        public static string MainParams = "Основные параметры";
        public static string AdditionalParams = "Дополнительные параметры";
        public static string AdminParams = "Инженерное меню";
        public static string LoginMenu = "Авторизация";
    }

    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Group { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
