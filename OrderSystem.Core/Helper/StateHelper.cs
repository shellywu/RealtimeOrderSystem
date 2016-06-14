using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Helper
{
    public class StateHelper
    {
        public static string GetProductTypeByState(ProductType productType)
        {
            string pType = string.Empty;

            switch (productType)
            {
                case ProductType.Combo: pType = "套餐"; break;
                case ProductType.Hotel: pType = "酒店"; break;
                case ProductType.Titcket: pType = "门票"; break;
                case ProductType.Travel: pType = "旅游"; break;
                case ProductType.Tax: pType = "租车"; break;
            }
            return pType;
        }

        public static string GetOrderStateByState(OrderStatus os) {
            string pType = string.Empty;

            switch (os)
            {
                case OrderStatus.Create: pType = "刚创建"; break;
                case OrderStatus.Processing: pType = "处理中"; break;
                case OrderStatus.Change: pType = "取消"; break;
                case OrderStatus.Done: pType = "完成"; break;
            }
            return pType;
        }
    }
}