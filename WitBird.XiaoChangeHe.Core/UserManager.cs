using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal;

namespace WitBird.XiaoChangeHe.Core
{
    public class UserManager
    {
        public string GetUid(Guid companyId, string sourceAccountId)
        {
            var userDal = new UserDal();

            var uid = userDal.GetUid(companyId, sourceAccountId);

            return uid;
        }
    }
}
