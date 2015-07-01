using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal;
using WitBird.XiaoChangeHe.Core.Entity;

namespace WitBird.XiaoChangeHe.Core
{
    public class ActivityManager
    {
        public ActivityDetail GetActivityDetailById(int id)
        {
            ActivityDetail detail = null;

            ActivityDal activityDal = new ActivityDal();

            var entity = activityDal.GetActivityById(id);

            if (entity != null)
            {
                detail = new ActivityDetail();

                detail.Address = entity.Address;
                detail.StartTime = entity.StartTime;
                detail.ContentText = entity.ContentText;
                detail.EndTime = entity.EndTime;
                detail.ImageUrl = entity.ImageUrl;
                detail.Link = entity.Link;
                detail.Title = entity.Title;
            }

            return detail;
        }
    }
}
