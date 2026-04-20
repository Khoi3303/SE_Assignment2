using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;

namespace BLL
{
    public class AgentBLL
    {
        AgentDAL agentDAL = new AgentDAL();

        public DataTable GetAllAgents()
        {
            return agentDAL.GetAllAgents();
        }
    }
}