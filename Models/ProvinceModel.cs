using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.XiaoChangHe.Models.Info;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace WitBird.XiaoChangHe.Models

{
    public class ProvinceModel : DbHelper
    {
        ///<summary>
        ///根据Province对象查询Province
        ///</summary>
        ///<param name="Model"></param>
        /// <returns></returns>
        public String Get_Province_By_Province(Province Model,string type)
        {
            try
            {
               string strSql="";
                DataAccessor<Province> tableAccessor;
               // if (string.IsNullOrEmpty(type))
               // {
                    strSql = string.Format("select * from Province s where 1=1 ");
                    
               // } else {
                //    strSql = string.Format("select * from Province s where 1=1 and s.Id=s.ParentId");
                  
                //}
                
                if (!string.IsNullOrEmpty(Model.Id))
                {
                   
                    strSql += string.Format(" and s.Id='{0}'", Model.Id);
                }
                if (!string.IsNullOrEmpty(Model.Name))
                {
                   
                    strSql += string.Format(" and s.Name='{0}'", Model.Name);
                }
                if (!string.IsNullOrEmpty(Model.ParentId))
                {
                  
                    strSql += string.Format(" and s.ParentId='{0}'", Model.ParentId);
                }
                if (Model.SortNo != null)
                {
                    strSql += string.Format(" and s.SortNo='{0}'", Model.SortNo);
                }
                if (Model.IsUse != null)
                {
                    strSql += string.Format(" and s.IsUse='{0}'", Model.IsUse);
                }
                tableAccessor = db.CreateSqlStringAccessor(strSql, MapBuilder<Province>.MapAllProperties().Build());
                List<Province> result = tableAccessor.Execute().ToList();
                string html = "";
                if (string.IsNullOrEmpty(type))
                {
                    html = string.Format("<select name='province' onchange='GetCity(this.value)'> <option value='-1'>--省--</option>");
                }
                else {
                    html = string.Format("<select name='province' onchange='page(this.value,this.options[this.selectedIndex].innerText)'> <option value='-1'>--市--</option>");

                }
                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            html += string.Format(" <option value='{0}'>{1}</option>", result[i].Id, result[i].Name);
                        }
                    }
                   html += string.Format(" </select>");
               
               // else {

                //  html += string.Format("<nav id='city'>");
                  // if (result.Count > 0)
                //    {
                //        for (int i = 0; i < result.Count; i++)
                //        {
                //            html += string.Format(" <a href='javascript:void(0)' onclick='page('{0}')' >{1}</a>", result[i].Id,result[i].Name);
                //        }
                //    }
                //    html += string.Format(" </nav>");
                //}
               
                return html;
       
            }
            catch
            {
                return null;
            }
        }



        public List<Province> getCityIdByCityName(string id)
        {

            string strSql = "select * from Province s where s.name='" + id.Trim() + "' and s.IsUse=1";
            DataAccessor<Province> tableAccessor;
            tableAccessor = db.CreateSqlStringAccessor(strSql, MapBuilder<Province>.MapAllProperties().Build());
            List<Province> result = tableAccessor.Execute().ToList();
            return result;
           // return result.First().Id.ToString();
        }
        



    }
}