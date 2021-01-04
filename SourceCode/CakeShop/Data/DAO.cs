using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeShop.Data
{
    public class CakeShopDAO
    {
        private OurCakeShopEntities Database;

        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh của cửa hàng có bán
        /// </summary>
        /// <param name="catName">Tên loại (Category Name)</param>
        /// <param name="inventoryNum">Số lượng tồn (Iventory Number)</param>
        /// <param name="dataAdded">Ngày thêm (Date Added)</param>
        /// <returns></returns>
        public List<CAKE> CakeList()
        {
            var cakes = Database.CAKEs.ToList();
            return cakes;
        }

        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh của cửa hàng có bán
        /// </summary>
        /// <param name="catName">Tên loại (Category Name)</param>
        /// <returns></returns>
        public List<CAKE> CakeList(string catName)
        {
            var categories = Database.CATEGORies;

            var query = from c in categories
                        where c.Name == catName
                        select c;

            var cat = query.ToList()[0];
            var cakes = cat.CAKEs.ToList();

            return cakes;
        }

        /// <summary>
        /// Hàm lấy dánh sách bánh theo tên loại và lượng tồn
        /// </summary>
        /// <param name="catName">Tên loại (Category Name)</param>
        /// <param name="inventoryNum">Số lượng tồn (Iventory Number)</param>
        /// <returns></returns>
        public List<CAKE> CakeList(string catName, long inventoryNum)
        {
            var categories = Database.CATEGORies;

            var cat = (from c in categories
                       where c.Name == catName
                       select c)
                        .ToList()[0];

            var cakes = (from cake in cat.CAKEs
                         where cake.InventoryNum > inventoryNum
                         select cake)
                         .ToList();

            return cakes;
        }

        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh theo tên loại, lượng tồn và ngày thêm
        /// </summary>
        /// <param name="catName">Tên loại (Category Name)</param>
        /// <param name="inventoryNum">Số lượng tồn (Iventory Number)</param>
        /// <param name="dateAdded">Ngày thêm (Date Added)</param>
        /// <returns></returns>
        public List<CAKE> CakeList(string catName, long inventoryNum, DateTime dateAdded)
        {
            var categories = Database.CATEGORies;

            var cat = (from c in categories
                       where c.Name == catName
                       select c)
                        .ToList()[0];

            var cakes = (from cake in cat.CAKEs
                         where (cake.InventoryNum > inventoryNum && cake.DateAdded > dateAdded)
                         select cake)
                         .ToList();

            return cakes;
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

        /// <summary>
        /// Hàm khởi tạo kết nối cơ sở dữ liệu
        /// </summary>
        public CakeShopDAO()
        {
            Database = new OurCakeShopEntities();
        }

        /// <summary>
        /// Hàm cập nhật cơ sở dữ liệu
        /// </summary>
        //public void UpdateDatabase()
        //{
        //    Database.SaveChanges();

        //}
    }
}
