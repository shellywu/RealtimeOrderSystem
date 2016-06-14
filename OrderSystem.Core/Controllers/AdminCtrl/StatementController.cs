using OrderSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace OrderSystem.Core.Controllers.AdminCtrl
{
    public class StatementController : Controller
    {
        private OrderDbContext db = new OrderDbContext();
        // GET: Statement
        public ActionResult ExpectOrder()
        {
            var dateBegin = DateTime.Now.Date;
            var dateEnd = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            var orders = db.Orders.Include("Customer").Where(o => o.OrderDate > dateBegin && o.OrderDate < dateEnd);
            return View(orders);
        }
        [HttpPost]
        public ActionResult ExpectOrder(DateTime startDate, DateTime endDate)
        {
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var orders = db.Orders.Include("Customer").Where(o => o.OrderDate > dateBegin && o.OrderDate < dateEnd);

            return PartialView("_OrderDeailts", orders);
        }

        public ActionResult OrderItem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrderItem(DateTime startDate, DateTime endDate)
        {
            List<OrderItemModel> oims = new List<OrderItemModel>();
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var orders = db.Orders.Include("OrderItems").Include("Customer").Where(o => o.OrderDate > dateBegin && o.OrderDate < dateEnd).ToList() ;
            foreach (var item in orders.ToList())
            {
                foreach (var oi in item.OrderItems.Where(oi=>oi.State==0))
                {
                    if (oi.ProductType == ProductType.Combo)
                    {
                        #region 套餐报表
                        var pid = int.Parse(oi.ProductId);
                        var cobomItems = (from cpi in db.ComboProductItems
                                          join product in db.Products on cpi.ProductId equals product.Id
                                          join partner in db.Partners on product.PartnerId equals partner.Id
                                          join suppler in db.Suppliers on product.SupplierId equals suppler.Id
                                          where cpi.ComboProductId == pid
                                          select new
                                          {
                                              partner.Name,
                                              suppler.SupplierName,
                                              product.ProductName,
                                              cpi.ProductPrice,
                                              cpi.Borkerage,
                                              cpi.Quantity,
                                              product.ContractPrice,
                                              product.Price,
                                              product.Settlement

                                          }).AsEnumerable();

                        foreach (var c in cobomItems)
                        {
                            OrderItemModel oim = new OrderItemModel();
                            oim.CreateDate = item.OrderDate.ToString("yyyy-MM-dd");
                            oim.State = item.OrderStatus.ToString();
                            oim.UsedDate = oi.StartDate.ToString("yyyy-MM-dd");
                            oim.UsedEndDate = oi.EndDate.ToString("yyyy-MM-dd");
                            oim.Spot = c.ProductName;
                            oim.CustomrName = item.Customer.CName;
                            oim.Phone = item.Customer.CPhone;
                            oim.PartnerName = c.Name;
                            oim.CertificateNumber = oi.CertificateNum;
                            oim.Quantity = oi.Quantity;
                            oim.CheckDate = oi.CertificateDate == null ? "" : DateTime.Parse(oi.CertificateDate.ToString()).ToString("hh:mm");
                            oim.Price =Math.Round((double)oi.CustomerPrice == 0 ? c.ProductPrice : (double)oi.CustomerPrice,2,MidpointRounding.AwayFromZero);
                            oim.Commision =Math.Round(c.Borkerage,2,MidpointRounding.AwayFromZero);
                            if (oi.CustomerPrice == 0)
                            {
                                oim.Receivables =Math.Round(oi.Quantity * c.Quantity * (oim.Price - c.Borkerage),2,MidpointRounding.AwayFromZero);
                                oim.PayLost =Math.Round(oi.Quantity * c.Quantity * (oim.Price - c.ProductPrice),2,MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                oim.Receivables =Math.Round(oi.Quantity * ((float)oi.CustomerPrice - c.Quantity * c.Borkerage),2,MidpointRounding.AwayFromZero);
                                oim.PayLost =Math.Round(oi.Quantity * ((float)oi.CustomerPrice - c.Quantity * c.ProductPrice),2,MidpointRounding.AwayFromZero);
                            }
                            oim.Cost =FormatDouble(c.ContractPrice);
                            oim.Pay = FormatDouble(c.ContractPrice * c.Quantity * oi.Quantity);
                            oim.Supplier = c.SupplierName;
                            oim.Incomde = oim.Receivables - oim.Pay + oim.PayLost;
                            oim.Remark = "套餐";
                            oims.Add(oim);
                        } 
                        #endregion
                    }
                    else
                    {
                        var pid = int.Parse(oi.ProductId);
                        var product = db.Products.Include("Partner").Include("Supplier").Where(p => p.Id == pid).First();
                        OrderItemModel oim = new OrderItemModel();
                        oim.CreateDate = item.OrderDate.ToString("yyyy-MM-dd");
                        oim.State = item.OrderStatus.ToString();
                        oim.UsedDate = oi.StartDate.ToString("yyyy-MM-dd");
                        oim.UsedEndDate = oi.EndDate.ToString("yyyy-MM-dd");
                        oim.Spot = product.ProductName;
                        oim.CustomrName = item.Customer.CName;
                        oim.Phone = item.Customer.CPhone;
                        oim.PartnerName = product.Partner.Name;
                        oim.CertificateNumber = oi.CertificateNum;
                        oim.Quantity = oi.Quantity;
                        oim.CheckDate = oi.CertificateDate == null ? "" : DateTime.Parse(oi.CertificateDate.ToString()).ToString("hh:mm");
                        oim.Price = Math.Round((double)oi.CustomerPrice == 0 ? (double)product.Price : (double)oi.CustomerPrice, 2, MidpointRounding.AwayFromZero);
                        oim.Commision =Math.Round(product.Borkerage,2,MidpointRounding.AwayFromZero);
                        oim.Receivables = Math.Round(oi.Quantity * ((float)oim.Price - product.Borkerage),2, MidpointRounding.AwayFromZero);
                        oim.Cost = Math.Round( product.ContractPrice,2,MidpointRounding.AwayFromZero);
                        oim.Pay = Math.Round( product.ContractPrice * oi.Quantity,2,MidpointRounding.AwayFromZero);
                        oim.Supplier = product.Supplier.SupplierName;
                        oim.PayLost = 0;
                        oim.Incomde = oim.Receivables - oim.Pay;
                        oim.Remark = "单独产品";
                        oims.Add(oim);
                    }
                }
            }

            return PartialView("_OrderItem", oims);
        }

        private double FormatDouble(double p)
        {
            return Math.Round(p, 2, MidpointRounding.AwayFromZero);
        }

        public ActionResult NormalTask()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NormalTask(DateTime startDate, DateTime endDate)
        {
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var tasks = db.Tasks.Where(ta => ta.CreateDate > dateBegin && ta.CreateDate < dateEnd).Join(db.Users, a => a.TakePerson, b => b.Id, (a, b) => new TaskViewModel
            {
                TaskCode = a.TaskCode,
                ProductName = a.ProductName,
                ProductType = a.ProductType,
                Quantity = a.Quantity,
                OrderCode = a.OrderCode,
                CreateDate = a.CreateDate,
                TakeDate = a.TakeDate,
                CompleteDate = a.CompleteDate,
                State = a.State,
                TakePerson = b.UserName
            });
            return PartialView("_TasksPart", tasks);
        }

        public FileContentResult ExportTask(DateTime startDate, DateTime endDate)
        {
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var tasks = db.Tasks.Where(ta => ta.CreateDate > dateBegin && ta.CreateDate < dateEnd).Join(db.Users, a => a.TakePerson, b => b.Id, (a, b) => new TaskViewModel
            {
                TaskCode = a.TaskCode,
                ProductName = a.ProductName,
                ProductType = a.ProductType,
                Quantity = a.Quantity,
                OrderCode = a.OrderCode,
                CreateDate = a.CreateDate,
                TakeDate = a.TakeDate,
                CompleteDate = a.CompleteDate,
                State = a.State,
                TakePerson = b.UserName,
                Remark = a.Remark
            });

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("异常订单");
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("任务编码");
            header.CreateCell(1).SetCellValue("任务状态");
            header.CreateCell(2).SetCellValue("产品类型");
            header.CreateCell(3).SetCellValue("产品数量");
            header.CreateCell(4).SetCellValue("订单编码");
            header.CreateCell(5).SetCellValue("生成日期");
            header.CreateCell(6).SetCellValue("领取人");
            header.CreateCell(7).SetCellValue("领取时间");
            header.CreateCell(8).SetCellValue("完成时间");
            header.CreateCell(9).SetCellValue("备注");
            var rowNumber = 1;
            foreach (var item in tasks)
            {
                IRow row = sheet.CreateRow(rowNumber);
                row.CreateCell(0).SetCellValue(item.TaskCode);
                row.CreateCell(1).SetCellValue(item.State.ToString());
                row.CreateCell(2).SetCellValue(item.ProductType.ToString());
                row.CreateCell(3).SetCellValue(item.Quantity);
                row.CreateCell(4).SetCellValue(item.OrderCode);
                row.CreateCell(5).SetCellValue(item.CreateDate.ToString("yyyy-MM-dd hh:mm:ss"));
                row.CreateCell(6).SetCellValue(item.TakePerson);
                row.CreateCell(7).SetCellValue(item.TakeDate.ToString("yyyy-MM-dd hh:mm:ss"));
                row.CreateCell(8).SetCellValue(item.CompleteDate.ToString("yyyy-MM-dd hh:mm:ss"));
                row.CreateCell(9).SetCellValue(item.Remark);
                rowNumber++;
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        [HttpGet]
        public FileContentResult ExportOrder(DateTime startDate, DateTime endDate)
        {
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var orders = db.Orders.Include("Customer").Where(o => o.OrderDate > dateBegin && o.OrderDate < dateEnd).Select(o => new
            {
                OrderCode = o.OrderCode,
                OrderDate = o.OrderDate,
                OrderStats = o.OrderStatus,
                Remark = o.Remark,
                CustomeName = o.Customer.CName,
                CustomePhone = o.Customer.CPhone,
                Description = o.Remark
            });

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("异常订单");
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("订单编号");
            header.CreateCell(1).SetCellValue("订单状态");
            header.CreateCell(2).SetCellValue("下单日期");
            header.CreateCell(3).SetCellValue("客户姓名");
            header.CreateCell(4).SetCellValue("联系方式");
            var rowNumber = 1;
            foreach (var item in orders)
            {
                IRow row = sheet.CreateRow(rowNumber);
                row.CreateCell(0).SetCellValue(item.OrderCode);
                row.CreateCell(1).SetCellValue(item.OrderStats.ToString());
                row.CreateCell(2).SetCellValue(item.OrderDate);
                row.CreateCell(3).SetCellValue(item.CustomeName);
                row.CreateCell(4).SetCellValue(item.CustomePhone);
                rowNumber++;
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        [HttpGet]
        public FileContentResult ExportOrderItem(DateTime startDate, DateTime endDate)
        {
            List<OrderItemModel> oims = new List<OrderItemModel>();
            var dateBegin = startDate.Date;
            var dateEnd = endDate.Date.AddHours(23).AddMinutes(59);
            var orders = db.Orders.Include("OrderItems").Include("Customer").Where(o => o.OrderDate > dateBegin && o.OrderDate < dateEnd);
            foreach (var item in orders.ToList())
            {
                foreach (var oi in item.OrderItems.Where(oi=>oi.State==0))
                {
                    if (oi.ProductType == ProductType.Combo)
                    {
                        var pid = int.Parse(oi.ProductId);
                        var cobomItems = (from cpi in db.ComboProductItems
                                          join product in db.Products on cpi.ProductId equals product.Id
                                          join partner in db.Partners on product.PartnerId equals partner.Id
                                          join suppler in db.Suppliers on product.SupplierId equals suppler.Id
                                          where cpi.ComboProductId == pid
                                          select new
                                          {
                                              partner.Name,
                                              suppler.SupplierName,
                                              product.ProductName,
                                              cpi.ProductPrice,
                                              cpi.Borkerage,
                                              cpi.Quantity,
                                              product.ContractPrice,
                                              product.Price,
                                              product.Settlement

                                          }).AsEnumerable();

                        foreach (var c in cobomItems)
                        {
                            OrderItemModel oim = new OrderItemModel();
                            oim.CreateDate = item.OrderDate.ToString("yyyy-MM-dd");
                            oim.State = item.OrderStatus.ToString();
                            oim.UsedDate = oi.StartDate.ToString("yyyy-MM-dd");
                            oim.UsedEndDate = oi.EndDate.ToString("yyyy-MM-dd");
                            oim.Spot = c.ProductName;
                            oim.CustomrName = item.Customer.CName;
                            oim.Phone = item.Customer.CPhone;
                            oim.PartnerName = c.Name;
                            oim.CertificateNumber = oi.CertificateNum;
                            oim.Quantity = oi.Quantity;
                            oim.CheckDate = oi.CertificateDate == null ? "" : DateTime.Parse(oi.CertificateDate.ToString()).ToString("hh:mm");
                            oim.Price =FormatDouble((double)oi.CustomerPrice == 0 ? c.ProductPrice : (double)oi.CustomerPrice);
                            oim.Commision = FormatDouble(c.Borkerage);
                            if (oi.CustomerPrice == 0)
                            {
                                oim.Receivables =FormatDouble(oi.Quantity * c.Quantity * (oim.Price - c.Borkerage));
                                oim.PayLost =FormatDouble(oi.Quantity * c.Quantity * (oim.Price - c.ProductPrice));
                            }
                            else
                            {
                                oim.Receivables =FormatDouble(oi.Quantity * ((float)oi.CustomerPrice - c.Quantity * c.Borkerage));
                                oim.PayLost =FormatDouble(oi.Quantity * ((float)oi.CustomerPrice - c.Quantity * c.ProductPrice)); 
                            }
                            oim.Cost =FormatDouble( c.ContractPrice);
                            oim.Pay =FormatDouble(c.ContractPrice * c.Quantity * oi.Quantity);
                            oim.Supplier = c.SupplierName;
                           
                            oim.Incomde = oim.Receivables - oim.Pay + oim.PayLost;
                            oim.Remark = "套餐";
                            oims.Add(oim);
                        }
                    }
                    else
                    {
                        var pid = int.Parse(oi.ProductId);
                        var product = db.Products.Include("Partner").Include("Supplier").Where(p => p.Id == pid).First();
                        OrderItemModel oim = new OrderItemModel();
                        oim.CreateDate = item.OrderDate.ToString("yyyy-MM-dd");
                        oim.State = item.OrderStatus.ToString();
                        oim.UsedDate = oi.StartDate.ToString("yyyy-MM-dd");
                        oim.UsedEndDate = oi.EndDate.ToString("yyyy-MM-dd");
                        oim.Spot = product.ProductName;
                        oim.CustomrName = item.Customer.CName;
                        oim.Phone = item.Customer.CPhone;
                        oim.PartnerName = product.Partner.Name;
                        oim.CertificateNumber = oi.CertificateNum;
                        oim.Quantity = oi.Quantity;
                        oim.CheckDate = oi.CertificateDate == null ? "" : DateTime.Parse(oi.CertificateDate.ToString()).ToString("hh:mm");
                        oim.Price =FormatDouble( (double)oi.CustomerPrice == 0 ? (double)product.Price : (double)oi.CustomerPrice);
                        oim.Commision =FormatDouble(product.Borkerage);
                        oim.Receivables =FormatDouble(oi.Quantity * ((float)oim.Price - product.Borkerage));
                        oim.Cost =FormatDouble( product.ContractPrice);
                        oim.Pay =FormatDouble( product.ContractPrice * oi.Quantity);
                        oim.Supplier = product.Supplier.SupplierName;
                        oim.PayLost = 0;
                        oim.Incomde = oim.Receivables - oim.Pay;
                        oim.Remark = "单独产品";
                        oims.Add(oim);
                    }
                }
            }

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("异常订单");
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("预定日期");
            header.CreateCell(1).SetCellValue("订单类型");
            header.CreateCell(2).SetCellValue("景区名称");
            header.CreateCell(3).SetCellValue("使用日期");
            header.CreateCell(4).SetCellValue("结束日期");
            header.CreateCell(5).SetCellValue("客人姓名");
            header.CreateCell(6).SetCellValue("联系方式");
            header.CreateCell(7).SetCellValue("订单来源");
            header.CreateCell(8).SetCellValue("券号/密码");
            header.CreateCell(9).SetCellValue("数量");
            header.CreateCell(10).SetCellValue("验证时");
            header.CreateCell(11).SetCellValue("售价");
            header.CreateCell(12).SetCellValue("佣金");
            header.CreateCell(13).SetCellValue("应收渠道");
            header.CreateCell(14).SetCellValue("成本价");
            header.CreateCell(15).SetCellValue("应付资源");
            header.CreateCell(16).SetCellValue("公司");
            header.CreateCell(17).SetCellValue("损益");
            header.CreateCell(18).SetCellValue("总收益");
            header.CreateCell(19).SetCellValue("备注");
            var rowNumber = 1;
            ICellStyle cs = book.CreateCellStyle();
            cs.DataFormat = 0x28;
            foreach (var item in oims)
            {
                IRow row = sheet.CreateRow(rowNumber);
                row.CreateCell(0).SetCellValue(item.CreateDate);
                row.CreateCell(1).SetCellValue(item.State);
                row.CreateCell(2).SetCellValue(item.Spot);
                row.CreateCell(3).SetCellValue(item.UsedDate);
                row.CreateCell(4).SetCellValue(item.UsedEndDate);
                row.CreateCell(5).SetCellValue(item.CustomrName);
                row.CreateCell(6).SetCellValue(item.Phone);
                row.CreateCell(7).SetCellValue(item.PartnerName);
                row.CreateCell(8).SetCellValue(item.CertificateNumber);
                row.CreateCell(9).SetCellValue(item.Quantity);
                row.CreateCell(10).SetCellValue(item.CheckDate);
                row.CreateCell(11).SetCellValue(item.Price);
                row.CreateCell(12).SetCellValue(item.Commision);
                row.CreateCell(13).SetCellValue(item.Receivables);
                row.CreateCell(14).SetCellValue(item.Cost);
                row.CreateCell(15).SetCellValue(item.Pay);
                row.CreateCell(16).SetCellValue(item.Supplier);
                row.CreateCell(17).SetCellValue(item.PayLost);
                row.CreateCell(18).SetCellValue(item.Incomde);
                row.CreateCell(19).SetCellValue(item.Remark);
                rowNumber++;
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}