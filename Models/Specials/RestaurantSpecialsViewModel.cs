using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Specials
{
    [Serializable]
    public class RestaurantSpecialsViewModel
    {
        public SpecialsEntity SpecialsEntity { set; get; }

        public List<SpecialsEntity> SpecialsList { set; get; }
    }
}