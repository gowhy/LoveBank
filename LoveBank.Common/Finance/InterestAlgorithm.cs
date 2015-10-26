using System;

namespace LoveBank.Common.Finance
{
    public static class InterestAlgorithm
    {
        public static decimal DailyInterest(int day, decimal money, double annualRate)
        {
            return Math.Round(money * (decimal)(annualRate / 360) * day,2);
        }

        public static decimal MonthInterest(decimal money, double annualRate)
        {
            return Math.Round(money * (decimal)(annualRate / 12),2);
        }
    }
}
