//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace automated_classreport.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_account
    {
        public int accId { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string acc_password { get; set; }
        public Nullable<int> brute_count { get; set; }
        public string brute_stat { get; set; }
    }
}
