using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Models
{
    public class VMDashboard
    {
        public List<ChartDashboard> ChartDashboards { get; set; }

        #region Account
        public List<RegisterNewAccount> NewAccountsInThisMonth { get; set; }
        public List<RegisterNewAccount> NewAccountsInPrevMonth { get; set; }
        public int CountNewAccount { get; set; }
        public int TotalAccount { get; set; }
        public float RateRegisterAccount { get; set; }
        #endregion

        #region Product
        public int TotalProduct { get; set; }
        #endregion

        #region Don hang
        public int TotalOrders { get; set; }
        #endregion

        #region Bai viet
        public int TotalPosts { get; set; }
        public List<PostInMonth> PostInMonth { get; set; }
        #endregion

        public VMDashboard()
        {
            ChartDashboards = new List<ChartDashboard>();
            NewAccountsInThisMonth = new List<RegisterNewAccount>();
            NewAccountsInPrevMonth = new List<RegisterNewAccount>();
            PostInMonth = new List<PostInMonth>();
        }
    }

    public class RegisterNewAccount
    {
        public int Count { get; set; }

        public string Day { get; set; }
    }

    public class PostInMonth
    {
        public int Count { get; set; }

        public string Day { get; set; }
    }

    public class ChartDashboard
    {
        public int Count { get; set; }

        public string Name { get; set; }
    }
}