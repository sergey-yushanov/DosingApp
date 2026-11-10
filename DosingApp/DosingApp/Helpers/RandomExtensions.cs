using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Helpers
{
    public static class RandomExtensions
    {
        private static readonly Random _random = new Random();

        // Перегрузка 1: со своим процентом
        public static double NextNearPercent(double baseValue, double percent)
        {
            double radius = baseValue * percent;
            return baseValue + (_random.NextDouble() * 2 - 1) * radius;
        }

        // Перегрузка 2: процент по умолчанию (2%)
        public static double NextNearPercent(double baseValue)
        {
            return NextNearPercent(baseValue, 0.02);
        }
    }
}
