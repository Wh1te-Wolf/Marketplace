using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Data.Interfaces
{
    public interface ITransactionEventData
    {
        public Guid TransactionUUID { get; set; }
    }
}
