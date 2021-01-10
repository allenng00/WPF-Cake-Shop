using CakeShop.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.ViewModels
{
    #region 
    ///Model for Home.xmal
    ///
    public class CategoryModel
    {
        public long CatID { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
    }
    #endregion

    #region
    ///ViewModel for Home.xmal
    ///
    public class HomeViewModel : INotifyPropertyChanged
    {
        public int CurrentArrangeMode;
        private List<CAKE> cakeList;
        public List<CAKE> CakeList
        {
            get { return this.cakeList; }
            set
            {
                if (value != this.cakeList)
                {
                    this.cakeList = new List<CAKE>(value);
                    OnPropertyChanged("CakeList");
                }
                else { return; }
            }
        }
        private List<CategoryModel> categoryList;
        public List<CategoryModel> CategoryList
        {
            get { return this.categoryList; }
            set
            {
                if (value != this.categoryList)
                {
                    this.categoryList = new List<CategoryModel>(value);
                    OnPropertyChanged("CategoryList");
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
        /// Hàm khởi tạo mặc định cho đối tượng thuộc lớp HomeViewModel
        /// </summary>
        public HomeViewModel()
        {
            App.appDAO = new CakeShopDAO();
            CakeList = new List<CAKE>();
            CakeList = App.appDAO.CakeList();
            CategoryList = new List<CategoryModel>();
            CategoryList = GetCATEGORies();
            CurrentArrangeMode = -1;
        }

        /// <summary>
        /// Hàm lấy những loại bánh hiện có
        /// </summary>
        /// <returns></returns>
        private List<CategoryModel> GetCATEGORies()
        {
            List<CategoryModel> result = new List<CategoryModel>();
            List<CATEGORY> list = App.appDAO.CategoryList();
            result.Add(new CategoryModel
            {
                CatID = 0,
                Name = "Tất cả",
                Quantity = App.appDAO.CountAllCakes()
            });

            foreach (var item in list)
            {
                result.Add(new CategoryModel
                {
                    CatID = item.ID,
                    Name = item.Name,
                    Quantity = App.appDAO.CountCakesByCategory(item.ID)
                });
            }
            return result.ToList();
        }

        /// <summary>
        /// Hàm lọc cake theo ID
        /// </summary>
        /// <param name="CatID"></param>
        public void GetCAKEsByCategory(long CatID)
        {
            if (CatID == 0)
            {
                CakeList = App.appDAO.CakeList();
            }
            else
            {
                var CatIDs = new long[] { CatID };
                CakeList = App.appDAO.CakeList(CatIDs);
            }
        }

        /// <summary>
        /// Hàm lọc cake theo ID
        /// </summary>
        /// <param name="CatID"></param>
        public void SearchCAKEsByName(string text)
        {
            if (text == ""||text==null)
            {
                CakeList = App.appDAO.CakeList();
            }
            else
            {
                CakeList = App.appDAO.SearchCakeByName(text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatName"></param>
        /// <param name="ArrangeMode"></param>
        public void GetCakeList(long[] CatIDs = null, int ArrangeMode = -1)
        {
            CakeList = App.appDAO.CakeList(CatIDs, ArrangeMode);
        }
    }
    #endregion
}
