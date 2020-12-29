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
        /// Hàm lấy dánh sách bánh của cửa hàng có bán
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
        /// Hàm lấy dánh sách tất cả bánh của cửa hàng có bán
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
        /// 
        /// </summary>
        public CakeShopDAO()
        {
            Database = new OurCakeShopEntities();
        }
    }
}
