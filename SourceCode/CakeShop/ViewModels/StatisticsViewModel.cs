using System.Collections.Generic;

namespace CakeShop.ViewModels
{
    class StatisticsViewModel
    {



        public StatisticsViewModel() { }
    }

    public static class WeekOfMonth
    {
        private static List<int> DayEndWeek = new List<int> { 7, 14, 21 };

        public static int WeekVerify(int date)
        {
            var result = -1;

            if (date < DayEndWeek[0])
            {
                result = 0;
            }
            else if (date < DayEndWeek[1])
            {
                result = 1;
            }
            else if (date < DayEndWeek[2])
            {
                result = 2;
            }
            else { 
                result = 3;
            }

            return result;
        }
    }
}
