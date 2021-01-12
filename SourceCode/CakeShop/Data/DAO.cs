using CakeShop.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            long[] catIDs = null,
            int arrangeMode = -1,
            long inventoryNum = -1,
            DateTime dateAdded = default(DateTime),
            string searchText = ""
            )
        {
            List<CAKE> result = new List<CAKE>();
            var cakes = Database.CAKEs;
            IQueryable<CAKE> tmp;

            // Lọc theo loại
            if (catIDs != null)
            {
                var catList = catIDs.AsQueryable()
                    .Join(Database.CATEGORies,
                    c1 => c1,
                    c2 => c2.ID,
                    (c1, c2) => new { ID = c2.ID, Name = c2.Name });

                var cakebycat = cakes
                    .Join(catList,
                    cake => cake.CatID,
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
            else if (arrangeMode == 4)
            {
                ordered = tmp.OrderBy(t => t.InventoryNum);
            }
            else if (arrangeMode == 5)
            {
                ordered = tmp.OrderByDescending(t => t.InventoryNum);
            }

            var searched = ordered.Where(x => x.Name.Contains(searchText));
            
            if (searched != null)
            {
                result = searched.Cast<CAKE>().ToList();
            }
            else if(tmp !=null)
            {
                result = ordered.Cast<CAKE>().ToList();
            }
            else
            {
                result = cakes.ToList();
            }
            return result;
        }

        /// <summary>
        /// Tìm kiếm Cake theo tên
        /// </summary>
        /// <param name="text">name  cần tìm kiếm</param>
        /// <returns></returns>
        public List<CAKE> SearchCakeByName(string text)
        {
            List<CAKE> result = new List<CAKE>();

            var query = Database.CAKEs.Where(x => x.Name.Contains(text));
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Update Cake theo ID
        /// </summary>
        /// <param name="CakeID">ID (CakeID</param>
        /// <param name="Cake">Cake (Cake)</param>
        public bool UpdateCake(CAKE updateCake, long CakeID)
        {
            bool check = true;
            var cur = (from c in Database.CAKEs
                       where c.ID == CakeID
                       select c).SingleOrDefault();
            try
            {
                cur.AvatarImage = updateCake.AvatarImage.ToArray();
                cur.Name = updateCake.Name;
                cur.BasePrice = updateCake.BasePrice;
                cur.SellPrice = updateCake.SellPrice;
                cur.Introduction = updateCake.Introduction;
                cur.DateAdded = updateCake.DateAdded;
                cur.CatID = updateCake.CatID;
            }
            catch (Exception ex)
            {
                check = false;
            }
            Database.SaveChanges();
            return check;
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

        /// <summary>
        /// Hàm lấy tên bánh theo Id 
        /// </summary>
        /// <param name="ID">ID  (CatID)</param>
        /// <returns></returns>
        public string CakeName(long ID)
        {
            var cake = Database.CAKEs;

            var query = from c in cake
                        where c.ID == ID
                        select c.Name;

            var name = query.ToList()[0].ToString();


            return name;
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
        /// Hàm lấy cake theo ID
        /// </summary>
        /// <param name="CakeID">ID  (CakeID)</param>
        /// <returns></returns>
        public CAKE GetCAKEs(long CakeID)
        {
            var cakes = Database.CAKEs;

            var query = (from c in cakes
                         where c.ID == CakeID
                         select c).SingleOrDefault();

            CAKE result = (CAKE)query;
            return result;
        }

        /// <summary>
        /// Hàm cập nhật lượng bánh tồn kho 
        /// </summary>
        /// <param name="CatID">ID  (CatID)</param>
        /// <returns></returns>
        public bool UpdateInvetoryCake(long CakeId, long NewInventoryNumber)
        {
            bool check = true;
            try
            {
                var cake = (from c in Database.CAKEs
                            where c.ID == CakeId
                            select c).SingleOrDefault();
                cake.InventoryNum = NewInventoryNumber;
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        /// <summary>
        /// Hàm lấy số lượng bánh theo loại category
        /// </summary>
        /// <param name="CatID">ID  (CatID)</param>
        /// <returns></returns>

        public long CountCakesByCategory(long CatID)
        {
            var query = (from c in Database.CAKEs
                         where c.CatID == CatID
                         select c.ID);
            var count = query.ToList().Count();

            return count;
        }
        /// <summary>
        /// Hàm lấy tổng số lương bánh
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public long CountAllCakes()
        {
            var query = (from c in Database.CAKEs
                         select c.ID);
            var count = query.ToList().Count();
            return count;
        }

        #endregion Cake

        #region Order
        /// <summary>
        /// Hàm lấy dánh sách tất cả Order
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ORDER> OrderList()
        {
            var oRDERs = Database.ORDERs;
            var query = oRDERs.OrderByDescending(x => x.DateCompleted);
            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// Hàm lấy danh sách tất cả
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(long OrderID, string Status)
        {
            bool check = true;
            try
            {
                var order = (from o in Database.ORDERs
                             where o.ID == OrderID
                             select o).SingleOrDefault();
                order.Status = Status;
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        /// <summary>
        /// Hàm lấy số lượng order hiện tại
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public long OrderCount()
        {
            long count = (long)Database.ORDERs.Count();
            return count;
        }
        #endregion Order

        #region Category
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

        public string CategoryNameByID(long CatID)
        {
            var query = (from c in Database.CATEGORies
                         where c.ID == CatID
                         select c).SingleOrDefault();
            string name = query.Name;
            return name;
        }
        #endregion

        #region Receive
        /// <summary>
        /// Ham lay danh sach don nhap hang
        /// </summary>
        /// <returns></returns>
        public List<ReceiveModel> ReceiveList()
        {
            var result = new List<ReceiveModel>();
            var receives = Database.RECEIVEs
                .Join(Database.CAKEs,
                r => r.CakeID,
                c => c.ID,
                (r, c) => new { r.ID, r.DateAdded, r.CakeID, c.Name, c.AvatarImage, r.CakeNum, r.Price, r.TotalBill })
                 .GroupBy(e => new { e.ID, e.DateAdded, e.TotalBill })
                .Select(g =>
                new
                {
                    ID = g.Select(r => r.ID).FirstOrDefault(),
                    Date = g.Select(r => r.DateAdded).FirstOrDefault(),
                    CountCake = g.Select(r => new { r.CakeID, r.Name, r.AvatarImage, r.CakeNum, r.Price })
                        .GroupBy(r => r.CakeID)
                        .Select(r => new
                        {
                            CakeID = r.Select(e => e.CakeID).FirstOrDefault(),
                            Name = r.Select(e => e.Name).FirstOrDefault(),
                            AvatarImage = r.Select(e => e.Name).FirstOrDefault(),
                            Num = r.Sum(e => e.CakeNum),
                            Price = r.Sum(e => e.Price)
                        }).Count(),
                    SumCake = g.Sum(s => s.CakeNum),
                    Total = g.Select(r => r.TotalBill).FirstOrDefault(),
                    CakeList = g.Select(r => new { r.CakeID, r.Name, r.AvatarImage, r.CakeNum, r.Price })
                        .GroupBy(r => r.CakeID)
                        .Select(r => new
                        {
                            CakeID = r.Select(e => e.CakeID).FirstOrDefault(),
                            Name = r.Select(e => e.Name).FirstOrDefault(),
                            AvatarImage = r.Select(e => e.AvatarImage).FirstOrDefault(),
                            Num = r.Sum(e => e.CakeNum),
                            Price = r.Sum(e => e.Price)
                        }).ToList()
                });

            foreach (var r in receives.ToList())
            {
                var tempCakeList = new List<CakeModel_ReceiveModel>();

                foreach (var cake in r.CakeList)
                {
                    tempCakeList.Add(new CakeModel_ReceiveModel()
                    {
                        ID = cake.CakeID,
                        AvatarImage = cake.AvatarImage,
                        Name = cake.Name,
                        Num = cake.Num,
                        Price = cake.Price
                    });
                }
                var tempReceive = new ReceiveModel
                {
                    ID = r.ID,
                    Date = r.Date,
                    CountCake = r.CountCake,
                    SumCake = r.SumCake,
                    Total = r.Total,
                    CakeList = tempCakeList
                };

                result.Add(tempReceive);
            }

            return result;
        }
        #endregion Receive

        #region Order_Detail
        /// <summary>
        /// Hàm lấy dánh sách tất cả bánh của đơn hàng theo OrderId
        /// </summary>
        /// <param name="OrderId">ID  (OrderId)</param>
        /// <returns></returns>
        public List<ORDER_DETAIL> OrderDetailList(long OrderId)
        {
            var order_details = Database.ORDER_DETAIL;

            var query = from o in order_details
                        where o.OrderID == OrderId
                        select o;
            var list = query.ToList();

            return list;
        }


        #endregion

        #region Status
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

        public string GetSTATUSsName(string ID)
        {
            var status = Database.STATUS;
            var query = from s in status
                        where s.ID == ID
                        select s.Name;
            string name = query.ToList()[0].ToString();
            return name;
        }

        public STATUS GetStatusByID(string ID)
        {
            var status = Database.STATUS;
            var query = from s in status
                        where s.ID == ID
                        select s;
            STATUS result = query.ToList()[0];
            return result;
        }
        #endregion

        #region Statistics
        public long TotalOrders(int week, int month, int year)
        {
            var result = 0;

            var orders = Database.ORDERs
                .Where(o => o.DateCompleted.Month == month
                && o.DateCompleted.Year == year
                && WeekOfMonth.WeekVerify(o.DateCompleted.Day) == week)
                .GroupBy(o => o.ID)
                .Select(o => new
                {
                    SUM = o.Sum(or => or.TotalBill)
                });

            return result;
        }

        public long TotalReceives(int week, int month, int year)
        {
            var result = 0;

            var orders = Database.RECEIVEs
                .Where(o => o.DateAdded.Month == month
                && o.DateAdded.Year == year
                && WeekOfMonth.WeekVerify(o.DateAdded.Day) == week)
                .GroupBy(o => o.ID)
                .Select(o => new
                {
                    SUM = o.Sum(or => or.TotalBill)
                });

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// 
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
