using System;
using System.Collections.Generic;
using System.ServiceModel;
using IntroductionMVC5.Service.DTOs;

namespace IntroductionMVC5.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        void AddPurchase(PurchaseDto purchase);

        [OperationContract]
        void AddSale(SaleDto sale);

        [OperationContract]
        void UpdateSale(SaleDto sale);

        [OperationContract]
        void UpdatePurchase(PurchaseDto weighBridgeInfo);

        [OperationContract]
        void AddContainerWeight(ContainerDto containerDto);

        [OperationContract]
        void UpdateContainer(ContainerDto containerDto);

        [OperationContract]
        void RecoverSale(SaleDto saleDto);

        [OperationContract]
        void RecoverPurchase(PurchaseDto purchaseDto);

        [OperationContract]
        List<TruckDto> GetTrucks();

        [OperationContract]
        List<DriverDto> GetDrivers();

        [OperationContract]
        List<SupplierInfoDto> GetSuppliers();

        [OperationContract]
        List<ProductDto> GetProducts();

        [OperationContract]
        List<CustomerDto> GetCustomers();

        [OperationContract]
        List<PurchaseDto> GetPurchase();

        [OperationContract]
        List<SaleDto> GetSales();

        [OperationContract]
        List<PurchaseDto> GetIncompletePurchase();

        [OperationContract]
        List<ContainerDto> GetIncompleteContainers();

        [OperationContract]
        List<ContainerDto> GetCompleteContainers();

        [OperationContract]
        List<SaleDto> GetIncompleteSales();

        [OperationContract]
        List<PurchaseDto> SearchPurchase(DateTime fromDate, DateTime toDate);

        [OperationContract]
        bool IsValidUser(string userName, string password);
    }
}