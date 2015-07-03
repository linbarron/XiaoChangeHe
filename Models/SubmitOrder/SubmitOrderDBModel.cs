using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentData;
using Senparc.Weixin.MP.Helpers;
using WitBird.XiaoChangHe.Models.Specials;

namespace WitBird.XiaoChangHe.Models.SubmitOrder
{
    public class SubmitOrderDBModel
    {
        public static bool UpdateOrderInfo(SubmitOrderEntity entity)
        {
            bool result = false;
            try
            {
                using (var context =
                    new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
                {
                    var rowsAffected = context.StoredProcedure("sp_UpdateOrderInfo")
                        .Parameter("ContactName", entity.ContactName)
                        .Parameter("ContactPhone", entity.ContactPhone)
                        .Parameter("DiningDate", entity.DiningDate)
                        .Parameter("OrderId", entity.OrderId)
                        .Parameter("PersonCount", entity.PersonCount)
                        .Parameter("sex", entity.sex).Execute();

                    result = rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                //TODO
            }
            return result;
        }
    }
}