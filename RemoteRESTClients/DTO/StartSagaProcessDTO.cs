using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteRESTClients.DTO
{
    public class StartSagaProcessDTO
    {
        public string Name { get; set; }

        public string TransactionEventDataString { get; set; }
    }
}
