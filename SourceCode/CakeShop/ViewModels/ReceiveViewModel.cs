using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CakeShop.ViewModels
{
    public class ReceiveViewModel : INotifyPropertyChanged

    {
        public List<ReceiveModel> ReceiveList { get; set; }
        private List<CakeModel_ReceiveModel> currentCakeList;
        public List<CakeModel_ReceiveModel> CurrentCakeList
        {
            get { return this.currentCakeList; }
            set
            {
                if (value != this.currentCakeList)
                {
                    this.currentCakeList = value;
                    OnPropertyChanged("CurrentCakeList");
                }
                else { return; }
            }
        }
        private long currentReceiveID;
        public long CurrentReceiveID
        {
            get { return this.currentReceiveID; }
            set
            {
                if (value != this.currentReceiveID)
                {
                    this.currentReceiveID = value;
                    OnPropertyChanged("CurrentReceiveID");
                }
                else { return; }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
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


        public ReceiveViewModel()
        {
            ReceiveList = new List<ReceiveModel>();
            ReceiveList = App.appDAO.ReceiveList();
        }
    }

    /// <summary>
    /// Mô hình cho đơn nhập hàng
    /// </summary>
    public class ReceiveModel : INotifyPropertyChanged
    {
        public long ID { get; set; } // Mã đơn
        public DateTime Date { get; set; } = DateTime.Now;// Ngày tạo
        public long CountCake { get; set; } = 0; // Số loại bánh
        public long SumCake { get; set; } = 0; // Tổng số bánh
        public long Total { get; set; } = 0;// Tổng tiền
        private List<CakeModel_ReceiveModel> cakeList;
        public List<CakeModel_ReceiveModel> CakeList
        {
            get { return this.cakeList; }
            set
            {
                if (value != this.cakeList)
                {
                    this.cakeList = value;
                    OnPropertyChanged("CakeList");
                }
                else { return; }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
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

        /// <summary>
        /// Hàm khởi tạo mặc định
        /// </summary>
        public ReceiveModel()
        {
            CakeList = new List<CakeModel_ReceiveModel>();
        }

        //public static ReceiveModel ConvertToReceive(
        //    DAOReceiveModel tempReceive)
        //{
        //    var rm = new ReceiveModel()
        //    {
        //        ID = tempReceive.ID,
        //        Date = tempReceive.Date,
        //        CountCake = tempReceive.CountCake,
        //        SumCake = tempReceive.SumCake,
        //        Total = tempReceive.Total
        //    };

        //    for (int pos = 0; pos < tempReceive.CountCake; pos++)
        //    {
        //        var tempCake = new CakeModel_ReceiveModel()
        //        {
        //            ID = tempReceive.CakeIDs[pos],
        //            Name = tempReceive.CakeNames[pos],
        //            AvatarImage = tempReceive.CakeAvatars[pos],
        //            Num = tempReceive.CakeNums[pos],
        //            Price = tempReceive.CakePrices[pos]
        //        };

        //        rm.CakeList.Add(tempCake);
        //    }

        //    return rm;
        //}
    }

    public class CakeModel_ReceiveModel: INotifyPropertyChanged
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public byte[] AvatarImage { get; set; }
        private long num { get; set; } = 1;
        public long Num
        {
            get { return this.num; }
            set
            {
                if (value != this.num)
                {
                    this.num = value;
                    OnPropertyChanged("Num");
                }
                else { return; }
            }
        }
        private long price = 0;
        public long Price
        {
            get { return this.price; }
            set
            {
                if (value != this.price)
                {
                    this.price = value;
                    OnPropertyChanged("Price");
                }
                else { return; }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
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
        public CakeModel_ReceiveModel() { }
    }


}
