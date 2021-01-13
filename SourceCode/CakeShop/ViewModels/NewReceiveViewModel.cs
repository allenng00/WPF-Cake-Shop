using System.Collections.Generic;
using System.ComponentModel;

namespace CakeShop.ViewModels
{
    class NewReceiveViewModel: INotifyPropertyChanged
    {
        private ReceiveModel mainReceive;
        public ReceiveModel MainReceive
        {
            get { return this.mainReceive; }
            set
            {
                if (value != this.mainReceive)
                {
                    this.mainReceive = value;
                    OnPropertyChanged("MainReceive");
                }
                else { return; }
            }
        }
        public long CurrentCatID { get; set; } = 1;
        public CakeModel_ReceiveModel CurrentCake { get; set; }
        public List<Data.CATEGORY> CategoryList { get; }
        private List<Data.CAKE> cakeByCatList;
        public List<Data.CAKE> CakeByCatList
        {
            get { return this.cakeByCatList; }
            set
            {
                if (value != this.cakeByCatList)
                {
                    this.cakeByCatList = value;
                    OnPropertyChanged("CakeByCatList");
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
        /// Hàm cập nhật CakeByCatList
        /// </summary>
        /// <param name="catID"></param>
        public void CakeByCat()
        {
            var catIDs = new long[] { CurrentCatID };
            CakeByCatList = App.appDAO.CakeList(catIDs);
        }
        /// <summary>
        /// Hàm khởi tạo mặc định
        /// </summary>
        public NewReceiveViewModel()
        {
            CategoryList = new List<Data.CATEGORY>();
            CategoryList = App.appDAO.CategoryList();

            MainReceive = new ReceiveModel();
            CurrentCake = new CakeModel_ReceiveModel();
            CakeByCatList = new List<Data.CAKE>();
        }
    }
}
