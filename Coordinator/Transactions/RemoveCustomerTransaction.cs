using Coordinator.Entities;
using Coordinator.Entities.TransactionData;
using Coordinator.Interfaces;
using Events.Services.Interfaces;
using TwoPhaseCommitEntities.Entities;
using TwoPhaseCommitUtils.Entities.Enums;

namespace Coordinator.Transactions;

public class RemoveCustomerTransaction : AbstractTransaction, ITransaction
{
    private string _userServiceName = "UserService";
    private string _productCartServiceName = "ProductCartService";

    private string _customerTypeName = "Customer";
    private string _productCartTypeName = "ProductCart";

    public object TransactionData { get; set; }
    public RemoveCustomerTransaction(IEventManager eventManager) : base(eventManager)
    {
        _serviceTypeMappings = new Dictionary<string, List<string>>()
        {
            { _userServiceName, new List<string>() { _customerTypeName } },
            { _productCartServiceName, new List<string>() { _productCartTypeName } }
        };
    }

    public ICollection<TransactionLocalRequestInfo> GetProcessCommitData()
    {
        CustomerRemoveTransactionData customerRemoveTransactionData = TransactionData as CustomerRemoveTransactionData;
        if (customerRemoveTransactionData is null)
            return new List<TransactionLocalRequestInfo>();

        List<TransactionLocalRequestInfo> transactionLocalRequestInfos = new List<TransactionLocalRequestInfo>()
        {
            GetTransactionLocalRequestInfo(_userServiceName, _customerTypeName),
            GetTransactionLocalRequestInfo(_productCartServiceName, _productCartTypeName)
        };

        CustomerCommitData customer = new CustomerCommitData()
        { 
            UUID = customerRemoveTransactionData.CustomerUUID
        };

        ProductCartCommitData productCart = new ProductCartCommitData()
        {
            UUID = customerRemoveTransactionData.ProductCartUUID
        };

        AddCommitTask(_customerTypeName, _userServiceName, customer, CUDAction.Delete);
        AddCommitTask(_productCartTypeName, _productCartServiceName, productCart, CUDAction.Delete);

        return transactionLocalRequestInfos;
    }

    public ICollection<TransactionLocalRequestInfo> GetProcessPrepareData()
    {
        CustomerRemoveTransactionData customerRemoveTransactionData = TransactionData as CustomerRemoveTransactionData;
        if (customerRemoveTransactionData is null)
            return new List<TransactionLocalRequestInfo>();

        List<TransactionLocalRequestInfo> transactionLocalRequestInfos = new List<TransactionLocalRequestInfo>()
        {
            GetTransactionLocalRequestInfo(_userServiceName, _customerTypeName),
            GetTransactionLocalRequestInfo(_productCartServiceName, _productCartTypeName)
        };

        AddPrepareTask(_customerTypeName, customerRemoveTransactionData.CustomerUUID, _userServiceName);
        AddPrepareTask(_productCartTypeName, customerRemoveTransactionData.ProductCartUUID, _productCartServiceName);

        return transactionLocalRequestInfos;
    }

    public async Task ReleaseLocksAsync()
    {
        CustomerRemoveTransactionData customerRemoveTransactionData = TransactionData as CustomerRemoveTransactionData;
        await ReleaseLockForEntityAsync(_customerTypeName, customerRemoveTransactionData.CustomerUUID);
        await ReleaseLockForEntityAsync(_productCartTypeName, customerRemoveTransactionData.ProductCartUUID);
    }

    public Task RollbackAsync()
    {
        throw new NotImplementedException();
    }
}
