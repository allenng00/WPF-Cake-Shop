//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CakeShop.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ORDER_DETAIL
    {
        public long OrderID { get; set; }
        public long No_ { get; set; }
        public long ProductID { get; set; }
        public long ProductNum { get; set; }
        public long Price { get; set; }
    
        public virtual CAKE CAKE { get; set; }
        public virtual ORDER ORDER { get; set; }
    }
}
