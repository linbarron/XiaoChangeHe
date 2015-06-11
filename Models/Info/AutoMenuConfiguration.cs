using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class AutoMenuConfiguration
    {

public Guid AMCID{ get; set; }

public bool? AMCStatus { get; set; }

public int ProductCount{ get; set; }

public string Remark{ get; set; }
public int? TeasSeat { get; set; }

public Guid AMCDID{ get; set; }

public Guid ProductType{ get; set; }

public bool? Status{ get; set; }

public int? ProductTypeCountStart{ get; set; }

public int? ProductTypeCountEnd{ get; set; }

    }
}