using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string Description { get; set; }
        public string BankId { get; set; }
        public string BankAccount { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime BusinessStartDate { get; set; }
        public DateTime BusinessEndtDate { get; set; }
        public string RegSN { get; set; }
        public string ReserveModel { get; set; }
        public string UseState { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
      //  public string CodeTypeListName { get; set; }
    }

    public class RestaurantAbstract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public bool isAcceptOrder { get; set; }
        public DateTime BusinessStartDate { get; set; }
        public DateTime BusinessEndtDate { get; set; }
        public int MaxTime { get; set; }
        public string RstType { get; set; }
        public string MapUrl { get; set; }
        public string VirtualUrl { get; set; }
        public byte[] Photo { get; set; }
        public string name { get; set; }
    }

    public class RestaurantAbstract1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public bool isAcceptOrder { get; set; }
        public DateTime BusinessStartDate { get; set; }
        public DateTime BusinessEndtDate { get; set; }
        public int MaxTime { get; set; }
        public string RstType { get; set; }
        public string MapUrl { get; set; }
        public string VirtualUrl { get; set; }
        public byte[] Photo { get; set; }
        //public string ProvinceId { get; set; }
        public string name { get; set; }
        //public string ParentId { get; set; }
        //public int SortNo { get; set; }
        //public bool IsUse { get; set; }
        //public string CityId { get; set; }
    }
}