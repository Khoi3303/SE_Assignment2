using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using DTO;
using DAL;

namespace BLL
{
    public class ItemBLL
    {
        ItemDAL itemDAL = new ItemDAL();

        public DataTable GetAllItems()
        {
            return itemDAL.GetAllItems();
        }

        public double? GetPriceById(int itemId)
        {
            return itemDAL.GetPriceById(itemId);
        }

        public bool AddItem(ItemDTO item)
        {
            // Kiểm tra logic nghiệp vụ cơ bản
            if (string.IsNullOrEmpty(item.ItemName) || item.Price <= 0)
                return false;

            return itemDAL.InsertItem(item);
        }
    }
}