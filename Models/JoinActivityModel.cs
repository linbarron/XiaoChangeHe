using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models
{
    public class JoinActivityModel
    {
        [Required]
        public string VerifyCode { get; set; }
    }
}