using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderBLL
    {
        OrderDAL orderDAL = new OrderDAL();

        // Hàm này sẽ nhận vào 1 Order và một danh sách các OrderDetail
        public bool SaveFullOrder(OrderDTO order, List<OrderDetailDTO> details)
        {
            // 1. Lưu Order trước để lấy ID
            int newOrderID = orderDAL.InsertOrder(order);

            if (newOrderID > 0)
            {
                // 2. Gán OrderID vừa tạo cho các chi tiết và lưu từng cái
                foreach (var item in details)
                {
                    item.OrderID = newOrderID;
                    orderDAL.InsertOrderDetail(item);
                }
                return true;
            }
            return false;
        }
        public DataTable GetLastOrderDetails()
        {
            return orderDAL.GetLastOrderDetails();
        }
    }
}