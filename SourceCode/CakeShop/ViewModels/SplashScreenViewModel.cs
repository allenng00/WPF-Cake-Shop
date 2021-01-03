using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.ViewModels
{
    public class SplashScreenViewModel
    {
        public Data.CAKE MainCake { get; set; }
        public string MaxTime { get; set; }
        
        public SplashScreenViewModel(CakeShop.Data.CAKE cake, string maxtime)
        {
            MainCake = cake;
            MaxTime = maxtime;
        }
    }
}
