//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAn3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BinhLuan
    {
        public int MaNPH { get; set; }
        public Nullable<int> MaGame { get; set; }
        public Nullable<int> UserID { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> ThoiGian { get; set; }
    
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
