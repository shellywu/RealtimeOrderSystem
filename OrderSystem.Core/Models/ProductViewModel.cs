using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public ProductType Type { get; set; }

        public string TypeName
        {
            get
            {
                string typeName = string.Empty;
                switch (this.Type)
                {
                    case ProductType.Combo: typeName = "套餐"; break;
                    case ProductType.Titcket: typeName = "门票"; break;
                    case ProductType.Hotel: typeName = "酒店"; break;
                    case ProductType.Tax: typeName = "租车"; break;
                    case ProductType.Travel: typeName = "旅游"; break;
                }
                return typeName;
            }
        }

        public string Name { get; set; }

        public string ProductCode { get; set; }

        public float Price { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Provider { get; set; }
    }
}