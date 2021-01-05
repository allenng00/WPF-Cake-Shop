using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.ViewModels
{
    public class SplashScreenViewModel:INotifyPropertyChanged
    {
        public Data.CAKE MainCake { get; set; }
        public string MaxTime { get; set; }
        private string timeLeft = null;
        public string TimeLeft
        {
            get { return this.timeLeft; }
            set
            {
                if (value != this.timeLeft)
                {
                    this.timeLeft = value;
                    OnPropertyChanged("TimeLeft");
                }
                else { return; }
            }
        }

        /// <summary>
        /// Hàm cập nhật thay đổi thuộc tính
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
            else { }
        }

        public SplashScreenViewModel(CakeShop.Data.CAKE cake, string maxtime)
        {
            MainCake = cake;
            MaxTime = maxtime;
            TimeLeft = maxtime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
