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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">1.正在进行中的。2，已经结束的</param>
        /// <returns></returns>
        public List<Activity> GetActivityList(int state)
        {
            List<Activity> activities = null;

            ActivityDal activityDal = new ActivityDal();

            var entities = activityDal.GetActivities(state);

            if (entities != null && entities.Count > 0)
            {
                activities = new List<Activity>();

                foreach (var entity in entities)
                {
                    var activity = new Activity();

                    activity.Id = entity.Id;
                    activity.Title = entity.Title;
                    activity.ImageUrl = entity.ImageUrl;
                    activity.Description = entity.Description;

                    activities.Add(activity);
                }
            }

            return activities;
        }

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
