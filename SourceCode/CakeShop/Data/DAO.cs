using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeShop.Data
{
    public class CakeShopDAO
    {
        private OurCakeShopEntities Database;
        /// <summary>
        /// Hàm khởi tạo kết nối cơ sở dữ liệu
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        
        public CakeShopDAO()
        {
            Database = new OurCakeShopEntities();
        }

        /// <summary>
        /// Hàm lấy dánh sách tất cả Category
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<CATEGORY> CategoryList()
        {
            var categories = Database.CATEGORies.ToList();
            return categories;
        }

        /// <summary>
        /// Hàm lấy dánh sách tất cả hình thức thanh toán
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<STATUS> StatusList()
        {
            var sTATUs = Database.STATUS.ToList();
            return sTATUs;
            
        }


        /// <summary>
        /// Hàm lấy dánh sách tất cả hình thức thanh toán
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public long OrderCount()
        {
            long count =(long) Database.ORDERs.Count();
            return count;
        }


        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh của cửa hàng theo Category Id
        /// </summary>
        /// <param name="CatID">ID  (CatID)</param>
        /// <returns></returns>
        public List<CAKE> CakeList(long CatID)
        {
            var categories = Database.CATEGORies;

            var query = from c in categories
                        where c.ID == CatID
                        select c;

            var cat = query.ToList()[0];
            var cakes = cat.CAKEs.ToList();

            return cakes;
        }

        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh của cửa hàng theo Category name
        /// Hàm cập nhật cơ sở dữ liệu
        /// </summary>
        public void UpdateDatabase()
        {
            Database.SaveChanges();

        }

        #region Cake
        /// <summary>
        /// Hàm lấy dánh sách bánh theo tên loại và lượng tồn
        /// </summary>
        /// <param name="catName">Tên loại (Category Name)</param>
        /// <param name="inventoryNum">Số lượng tồn (Iventory Number)</param>
        /// <returns></returns>
        public List<CAKE> CakeList(
            string[] catNames = null,
            long inventoryNum = -1,
            DateTime dateAdded = default(DateTime),
            int arrangeMode = -1)
        {
            List<CAKE> result;
            var cakes = Database.CAKEs;
            IEnumerable<CAKE> tmp;

            // Lọc theo loại
            if (catNames != null)
            {
                var catList = catNames.AsEnumerable()
                    .Join(Database.CATEGORies,
                    c1 => c1,
                    c2 => c2.Name,
                    (c1, c2) => new { ID = c2.ID, Name = c2.Name });

                var cakebycat = cakes
                    .Join(catList,
                    cake => cake.ID,
                    cat => cat.ID,
                    (cake, cat) => cake);

                tmp = cakebycat;
            }
            else
            {
                tmp = cakes;
            }

            // Lọc theo số lượng
            if (inventoryNum > -1)
            {
                var cakebynum = tmp.Where(t => t.InventoryNum >= inventoryNum);
                tmp = cakebynum;
            }
            else { }

            // Lọc theo ngày thêm
            if (dateAdded != default(DateTime))
            {

            }
            else { }

            var ordered = tmp;
            // Không sắp xếp
            if (arrangeMode == -1)
            { }
            // Theo Alphabet tăng
            else if (arrangeMode == 0)
            {
                ordered = tmp.OrderBy(t => t.Name);
            }
            // Theo Alphabet giảm
            else if (arrangeMode == 1)
            {
                ordered = tmp.OrderByDescending(t => t.Name);
            }
            // Theo giá tăng
            else if (arrangeMode == 2)
            {
                ordered = tmp.OrderBy(t => t.SellPrice);
            }
            // Theo giá giảm
            else if (arrangeMode == 3)
            {
                ordered = tmp.OrderByDescending(t => t.SellPrice);
            }

            result = ordered.ToList();
            return result;
        }

        /// <summary>
        /// Hàm thêm một loại bánh mới vào cơ sở dữ liệu
        /// </summary>
        /// <param name="tempCake"></param>
        public void AddCake(CAKE tempCake)
        {
            var cakes = CakeList();

            cakes.Add(tempCake);
            Database.SaveChanges();
        }
        #endregion Cake

        #region Order
        public List<ORDER> OrderList(
            )
        {
            List<ORDER> result = Database.ORDERs.ToList();
            return result;
        }
        #endregion Order
        #region Receive
        #endregion Receive


        #region Statistics
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void Statistics(DateTime start, DateTime end = default(DateTime))
        {
            var newEnd = (end == default(DateTime)) ? DateTime.Now : end;
            var orderlist = Database.ORDERs
                .Select(o => o.DateCompleted >= start && o.DateCompleted <= newEnd)
                ;

            var orderlist2 = Database.ORDERs
                //.GroupBy(o=> o.DateCompleted.Month == 1)
                .Select(o => o.DateCompleted >= start && o.DateCompleted <= newEnd)
                ;

            var receivelist = Database.RECEIVEs
               .Select(r => r.DateAdded >= start && r.DateAdded <= newEnd);
        }
        #endregion Statistics

        
    }
}
