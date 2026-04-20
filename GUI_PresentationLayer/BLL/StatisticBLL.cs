using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;

namespace BLL
{
    public class StatisticBLL
    {
        StatisticDAL statDAL = new StatisticDAL();

        public DataTable GetBestItems() => statDAL.GetBestItems();
        public DataTable GetItemsByAgent(int agentId) => statDAL.GetItemsByAgent(agentId);
        public DataTable GetAgentsByItem(int itemId) => statDAL.GetAgentsByItem(itemId);
    }
}