using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using IntroductionMVC5.Data;
using IntroductionMVC5.Models.Integrator;
using IntroductionMVC5.Service.DTOs;
using RustiviaSolutions.PDFGenerator;

namespace IntroductionMVC5.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class DataService : IDataService
    {
        private readonly ApplicationUnit _unit = new ApplicationUnit();

        public void AddPurchase(PurchaseDto purchaseDto)
        {
            var weighBridgeInfo = new WeighBridgeInfo
            {
                FirstMass = purchaseDto.WeighBridgeInfo.FirstMass,
                DateIn = purchaseDto.WeighBridgeInfo.DateIn,
                NettMass = purchaseDto.WeighBridgeInfo.NettMass,
                Product = purchaseDto.WeighBridgeInfo.Product,
                Comments = purchaseDto.WeighBridgeInfo.Comments
            };

            var purchase = new Purchase
            {
                Status = purchaseDto.Status,
                WeighBridgeInfo = weighBridgeInfo,
                Truck = GetTruck(purchaseDto.Truck),
                Driver = GetDriver(purchaseDto.Driver),
                Price = purchaseDto.Price,
                TotalPrice = purchaseDto.TotalPrice
            };
            _unit.Purchase.Add(purchase);
            _unit.SaveChanges();
        }

        public void AddSale(SaleDto saleDto)
        {
            var weighBridgeInfo = new WeighBridgeInfo
            {
                FirstMass = saleDto.WeighBridgeInfo.FirstMass,
                DateIn = saleDto.WeighBridgeInfo.DateIn,
                NettMass = saleDto.WeighBridgeInfo.NettMass,
                Product = saleDto.WeighBridgeInfo.Product,
                Comments = saleDto.WeighBridgeInfo.Comments
            };

            var sale = new Sale
            {
                Status = saleDto.Status,
                TruckRegNumber = saleDto.TruckRegNumber,
                ExtraInfo = saleDto.ExtraInfo,
                WeighBridgeInfo = weighBridgeInfo,
                Customer = GetCustomer(saleDto.Customer)
            };

            _unit.Sale.Add(sale);
            _unit.SaveChanges();
        }

        public void UpdateSale(SaleDto saleDto)
        {
            Sale sale = _unit.Sale.GetAll()
                .Include(a => a.WeighBridgeInfo).Include(d => d.Customer)
                .FirstOrDefault(x => x.Id == saleDto.Id);

            sale.WeighBridgeInfo.DateOut = saleDto.WeighBridgeInfo.DateOut;
            sale.WeighBridgeInfo.NettMass = saleDto.WeighBridgeInfo.NettMass;
            sale.WeighBridgeInfo.Product = saleDto.WeighBridgeInfo.Product;
            sale.WeighBridgeInfo.Comments = saleDto.WeighBridgeInfo.Comments;
            sale.WeighBridgeInfo.SecondMass = saleDto.WeighBridgeInfo.SecondMass;
            sale.Status = saleDto.Status;
            sale.ExtraInfo = saleDto.ExtraInfo;
            _unit.Sale.Update(sale);
            _unit.SaveChanges();
        }

        public void UpdatePurchase(PurchaseDto purchaseDto)
        {
            Purchase purchase = _unit.Purchase.GetAll()
                .Include(a => a.WeighBridgeInfo)
                .Include(d => d.Driver)
                .Include(t => t.Truck)
                .FirstOrDefault(x => x.WeighBridgeInfo.Id == purchaseDto.WeighBridgeInfo.Id);

            purchase.WeighBridgeInfo.DateOut = purchaseDto.WeighBridgeInfo.DateOut;
            purchase.WeighBridgeInfo.NettMass = purchaseDto.WeighBridgeInfo.NettMass;
            purchase.WeighBridgeInfo.Product = purchaseDto.WeighBridgeInfo.Product;
            purchase.WeighBridgeInfo.Comments = purchaseDto.WeighBridgeInfo.Comments;
            purchase.WeighBridgeInfo.SecondMass = purchaseDto.WeighBridgeInfo.SecondMass;
            purchase.Status = purchaseDto.Status;
            purchase.Price = purchaseDto.Price;
            purchase.TotalPrice = purchaseDto.TotalPrice;

            _unit.Purchase.Update(purchase);
            _unit.SaveChanges();
        }

        public void AddContainerWeight(ContainerDto containerDto)
        {
            var weighBridgeInfo = new WeighBridgeInfo
            {
                FirstMass = containerDto.WeighBridgeInfo.FirstMass,
                DateIn = containerDto.WeighBridgeInfo.DateIn,
                NettMass = containerDto.WeighBridgeInfo.NettMass,
                Product = containerDto.WeighBridgeInfo.Product,
                Comments = containerDto.WeighBridgeInfo.Comments
            };

            Container container = _unit.Containers.GetAll()
                .FirstOrDefault(x => x.ContainerNumber == containerDto.ContainerNumber);

            if (container != null)
            {
                container.TruckRegNumber = containerDto.TruckRegNumber;
                container.GrossWeight = containerDto.WeighBridgeInfo.SecondMass;
                container.Status = containerDto.Status;
                container.Sealnumber = containerDto.Sealnumber;
                container.WeighBridgeInfo = weighBridgeInfo;
                _unit.Containers.Update(container);
            }
            _unit.SaveChanges();
        }

        public void UpdateContainer(ContainerDto containerDto)
        {
            Container container = _unit.Containers.GetAll()
                .Include(a => a.WeighBridgeInfo)
                .Include(b => b.Booking.Customer)
                .Include(b => b.Booking.Transporter)
                .FirstOrDefault(x => x.ContainerNumber == containerDto.ContainerNumber);

            if (container != null)
            {
                container.NettWeight = containerDto.NettWeight;
                container.Product = containerDto.Product;
                container.TruckRegNumber = containerDto.TruckRegNumber;
                container.GrossWeight = containerDto.WeighBridgeInfo.SecondMass;
                container.Status = containerDto.Status;
                container.WeighBridgeInfo.DateOut = containerDto.WeighBridgeInfo.DateOut;
                container.WeighBridgeInfo.NettMass = containerDto.WeighBridgeInfo.NettMass;
                container.WeighBridgeInfo.Product = containerDto.WeighBridgeInfo.Product;
                container.WeighBridgeInfo.Comments = containerDto.WeighBridgeInfo.Comments;
                _unit.Containers.Update(container);
            }
            _unit.SaveChanges();

            PrintContainerInformat(container);
        }

        public List<TruckDto> GetTrucks()
        {
            List<Truck> trucks = _unit.Truck.GetAll().ToList();
            return trucks.Select(truck => new TruckDto
            {
                Id = truck.Id,
                TruckRegNumber = truck.TruckRegNumber,
                Own = truck.Own
            }).ToList();
        }

        public List<DriverDto> GetDrivers()
        {
            List<Driver> drivers = _unit.Drivers.GetAll().OrderBy(d => d.Firstname).ToList();
            return drivers.Select(driver => new DriverDto
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber,
                SupplierInfo = GetSupplierInfoDto(driver.SupplierInfo)
            }).ToList();
        }

        public List<SupplierInfoDto> GetSuppliers()
        {
            List<SupplierInfo> suppliers = _unit.SupplierInfo.GetAll().OrderBy(s => s.SupplierName)
                .Include(d => d.Drivers)
                .Include(t => t.Trucks)
                .Include(sp => sp.SupplierProducts)
                .Where(s => !s.IsBlocked)
                .ToList();

            return suppliers.Select(supplier => new SupplierInfoDto
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                SupplierCode = supplier.Suppliercode,
                Trucks = supplier.Trucks != null ? GetTrucksDtos(supplier.Trucks) : new List<TruckDto>(),
                Drivers = supplier.Drivers != null ? GetDriversDtos(supplier.Drivers) : new List<DriverDto>(),
                SupplierProductDtos =
                    supplier.SupplierProducts != null
                        ? GetSupplierProductDto(supplier.SupplierProducts)
                        : new List<SupplierProductDto>(),
            }).ToList();
        }

        public List<ProductDto> GetProducts()
        {
            List<Product> products = _unit.Product.GetAll().OrderBy(s => s.Name).ToList();

            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                RustiviaPrice = product.RustiviaPrice
            }).ToList();
        }

        public List<CustomerDto> GetCustomers()
        {
            List<Customer> customers =
                _unit.Customers.GetAll()
                    .OrderBy(c => c.CustomerName)
                    .Include(b => b.Bookings.Select(c => c.Containers))
                    .ToList();

            return customers.Select(customer => new CustomerDto
            {
                Id = customer.Id,
                CustomerCode = customer.Suppliercode,
                CustomerName = customer.CustomerName,
                Address = customer.Address,
                Bookings = customer.Bookings != null ? GetBookingDtos(customer.Bookings) : new List<BookingDto>()
            }).ToList();
        }

        public List<PurchaseDto> GetPurchase()
        {
            IEnumerable<Purchase> purchases =
                _unit.Purchase.GetAll()
                    .Where(wb => wb.Status != Statuses.FirstWeight)
                    .OrderByDescending(d => d.WeighBridgeInfo.Id)
                    .Include(t => t.Truck)
                    .Include(d => d.Driver)
                    .Include(d => d.WeighBridgeInfo).Take(50).ToList();

            return purchases.Select(purchase => new PurchaseDto
            {
                Id = purchase.Id,
                WeighBridgeInfo = GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status,
                Driver = GetDriverDto(purchase.Driver),
                Truck = GettruckDto(purchase.Truck),
            }).ToList();
        }

        public List<SaleDto> GetSales()
        {
            IEnumerable<Sale> purchases =
                _unit.Sale.GetAll().Include(w => w.WeighBridgeInfo).Include(c => c.Customer)
                    .Where(wb => wb.Status != Statuses.FirstWeight)
                    .Take(50).ToList();

            return purchases.Select(sale => new SaleDto
            {
                Id = sale.Id,
                WeighBridgeInfo = GetWeighBridgeInfoDto(sale.WeighBridgeInfo),
                Status = sale.Status,
                ExtraInfo = sale.ExtraInfo,
                TruckRegNumber = sale.TruckRegNumber,
                Customer = GetCustomerDto(sale.Customer)
            }).ToList();
        }

        public List<PurchaseDto> GetIncompletePurchase()
        {
            IEnumerable<Purchase> purchases =
                _unit.Purchase.GetAll()
                    .Where(wb => wb.Status == Statuses.FirstWeight)
                    .OrderByDescending(w => w.Id)
                    .Include(t => t.Truck)
                    .Include(d => d.Driver)
                    .Include(d => d.WeighBridgeInfo).ToList();


            return purchases.Select(purchase => new PurchaseDto
            {
                Id = purchase.Id,
                WeighBridgeInfo = GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status,
                Driver = GetDriverDto(purchase.Driver),
                Truck = GettruckDto(purchase.Truck),
            }).ToList();
        }

        public List<ContainerDto> GetIncompleteContainers()
        {
            IEnumerable<Container> containers =
                _unit.Containers.GetAll().Include(b => b.Booking)
                    .Where(wb => wb.Status == Statuses.FirstWeight)
                    .OrderByDescending(w => w.Id).ToList();

            return containers.Select(container => new ContainerDto
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                TruckRegNumber = container.TruckRegNumber,
                TareWeight = container.TareWeight,
                NettWeight = container.NettWeight,
                Product = container.Product,
                DateIn = container.DateIn,
                WeighBridgeInfo = GetWeighBridgeInfoDto(container.WeighBridgeInfo),
                Booking = GetBookingDto(container.Booking)
            }).ToList();
        }

        public List<ContainerDto> GetCompleteContainers()
        {
            IEnumerable<Container> containers =
                _unit.Containers.GetAll().Include(b => b.Booking)
                    .Where(wb => wb.Status == Statuses.Processed)
                    .OrderByDescending(w => w.Id).Take(50).ToList();

            return containers.Select(container => new ContainerDto
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                TruckRegNumber = container.TruckRegNumber,
                TareWeight = container.TareWeight,
                NettWeight = container.NettWeight,
                Product = container.Product,
                DateIn = container.DateIn,
                WeighBridgeInfo = GetWeighBridgeInfoDto(container.WeighBridgeInfo),
                Booking = GetBookingDto(container.Booking)
            }).ToList();
        }

        public List<SaleDto> GetIncompleteSales()
        {
            IEnumerable<Sale> sales =
                _unit.Sale.GetAll().Include(d => d.WeighBridgeInfo).Include(cust => cust.Customer)
                    .Where(wb => wb.Status == Statuses.FirstWeight)
                    .OrderByDescending(w => w.Id).ToList();

            return sales.Select(sale => new SaleDto
            {
                Id = sale.Id,
                ExtraInfo = sale.ExtraInfo,
                TruckRegNumber = sale.TruckRegNumber,
                WeighBridgeInfo = GetWeighBridgeInfoDto(sale.WeighBridgeInfo),
                Customer = GetCustomerDto(sale.Customer)
            }).ToList();
        }

        public bool IsValidUser(string userName, string password)
        {
            List<User> users = _unit.Users.GetAll().ToList();

            User isUser = users.FirstOrDefault(user => user.Password == password && user.UserName == userName);

            return isUser != null;
        }

        public List<PurchaseDto> SearchPurchase(DateTime fromDate, DateTime toDate)
        {
            IEnumerable<Purchase> purchases =
                _unit.Purchase.GetAll()
                    .Where(wb => wb.Status != Statuses.FirstWeight &&
                                 EntityFunctions.TruncateTime(fromDate) <=
                                 EntityFunctions.TruncateTime(wb.WeighBridgeInfo.DateIn)
                                 &&
                                 EntityFunctions.TruncateTime(wb.WeighBridgeInfo.DateIn) <=
                                 EntityFunctions.TruncateTime(toDate))
                    .OrderByDescending(d => d.WeighBridgeInfo.Id)
                    .Include(t => t.Truck)
                    .Include(d => d.Driver)
                    .Include(d => d.WeighBridgeInfo).ToList();

            return purchases.Select(purchase => new PurchaseDto
            {
                Id = purchase.Id,
                WeighBridgeInfo = GetWeighBridgeInfoDto(purchase.WeighBridgeInfo),
                Status = purchase.Status,
                Driver = GetDriverDto(purchase.Driver),
                Truck = GettruckDto(purchase.Truck),
            }).ToList();
        }

        private CustomerDto GetCustomerDto(Customer customer)
        {
            var cus = new CustomerDto();
            if (customer != null)
            {
                cus = new CustomerDto
                {
                    Id = customer.Id
                };
            }
            return cus;
        }

        private List<SupplierProductDto> GetSupplierProductDto(IEnumerable<SupplierProduct> list)
        {
            return list.Select(sp => new SupplierProductDto
            {
                SupplierId = sp.SupplierId,
                ProductId = sp.ProductId,
                SupplierPrice = sp.SupplierPrice
            }).ToList();
        }

        private void PrintContainerInformat(Container container)
        {
            var printGenerator = new ContainerInfomationGenerator();
            printGenerator.PrintContainerInfomation(container);
        }

        private BookingDto GetBookingDto(Booking booking)
        {
            return new BookingDto
            {
                Containers = GetContainerDtos(booking.Reference),
                ReferenceNumber = booking.Reference
            };
        }

        private WeighBridgeInfoDto GetWeighBridgeInfoDto(WeighBridgeInfo weighBridge)
        {
            var wb = new WeighBridgeInfoDto();

            if (weighBridge != null)
            {
                wb = new WeighBridgeInfoDto
                {
                    FirstMass = weighBridge.FirstMass,
                    NettMass = weighBridge.NettMass,
                    SecondMass = weighBridge.SecondMass,
                    DateIn = weighBridge.DateIn,
                    DateOut = weighBridge.DateOut,
                    Product = weighBridge.Product,
                    Comments = weighBridge.Comments,
                    Id = weighBridge.Id
                };
            }
            return wb;
        }

        private List<BookingDto> GetBookingDtos(IEnumerable<Booking> bookings)
        {
            return bookings.Select(booking => new BookingDto
            {
                ReferenceNumber = booking.Reference,
                Containers = booking.Containers != null ? GetContainerDtos(booking.Reference) : new List<ContainerDto>()
            }).ToList();
        }

        private List<ContainerDto> GetContainerDtos(string referenceNumber)
        {
            List<Container> containers =
                _unit.Containers.GetAll().Where(c => c.Booking.Reference == referenceNumber
                                                     &&
                                                     (c.Status == Statuses.FirstWeight || c.Status == Statuses.Waiting))
                    .ToList();

            return containers.Select(container => new ContainerDto
            {
                ContainerNumber = container.ContainerNumber,
                Sealnumber = container.Sealnumber,
                Product = container.Product
            }).ToList();
        }

        private SupplierInfoDto GetSupplierInfoDto(SupplierInfo supplierInfo)
        {
            return new SupplierInfoDto
            {
                Id = supplierInfo.Id,
                SupplierName = supplierInfo.SupplierName,
                SupplierCode = supplierInfo.Suppliercode,
                Trucks = supplierInfo.Trucks != null ? GetTrucksDtos(supplierInfo.Trucks) : new List<TruckDto>(),
                Drivers = supplierInfo.Drivers != null ? GetDriversDtos(supplierInfo.Drivers) : new List<DriverDto>()
            };
        }

        private List<DriverDto> GetDriversDtos(IEnumerable<Driver> drivers)
        {
            return drivers.Select(driver => new DriverDto
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber
            }).ToList();
        }

        private List<TruckDto> GetTrucksDtos(IEnumerable<Truck> trucks)
        {
            return trucks.Select(truck => new TruckDto
            {
                Id = truck.Id,
                TruckRegNumber = truck.TruckRegNumber
            }).ToList();
        }

        private Truck GetTruck(TruckDto truckDto)
        {
            return _unit.Truck.GetById(truckDto.Id);
        }

        private Customer GetCustomer(CustomerDto customerDto)
        {
            return _unit.Customers.GetById(customerDto.Id);
        }

        private Driver GetDriver(DriverDto driver)
        {
            return _unit.Drivers.GetById(driver.Id);
        }

        private TruckDto GettruckDto(Truck truck)
        {
            return new TruckDto
            {
                Id = truck.Id,
                TruckRegNumber = truck.TruckRegNumber,
                Own = truck.Own
            };
        }

        private DriverDto GetDriverDto(Driver driver)
        {
            return new DriverDto
            {
                Id = driver.Id,
                Firstname = driver.Firstname,
                Surname = driver.Surname,
                IdNumber = driver.IdNumber
            };
        }

        public void RecoverSale(SaleDto saleDto)
        {
            WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
            {
                FirstMass = saleDto.WeighBridgeInfo.FirstMass,
                DateIn = saleDto.WeighBridgeInfo.DateIn,
                DateOut = saleDto.WeighBridgeInfo.DateOut,
                NettMass = saleDto.WeighBridgeInfo.NettMass,
                Product = saleDto.WeighBridgeInfo.Product,
                Comments = saleDto.WeighBridgeInfo.Comments
            };
            _unit.Sale.Add(new Sale()
            {
                Status = saleDto.Status,
                TruckRegNumber = saleDto.TruckRegNumber,
                ExtraInfo = saleDto.ExtraInfo,
                WeighBridgeInfo = weighBridgeInfo,
                Customer = this.GetCustomer(saleDto.Customer)
            });
            _unit.SaveChanges();
        }

        public void RecoverPurchase(PurchaseDto purchaseDto)
        {
            WeighBridgeInfo weighBridgeInfo = new WeighBridgeInfo()
            {
                FirstMass = purchaseDto.WeighBridgeInfo.FirstMass,
                DateIn = purchaseDto.WeighBridgeInfo.DateIn,
                DateOut = purchaseDto.WeighBridgeInfo.DateOut,
                NettMass = purchaseDto.WeighBridgeInfo.NettMass,
                Product = purchaseDto.WeighBridgeInfo.Product,
                Comments = purchaseDto.WeighBridgeInfo.Comments
            };
            _unit.Purchase.Add(new Purchase()
            {
                Status = purchaseDto.Status,
                WeighBridgeInfo = weighBridgeInfo,
                Truck = this.GetTruck(purchaseDto.Truck),
                Driver = this.GetDriver(purchaseDto.Driver),
                Price = purchaseDto.Price,
                TotalPrice = purchaseDto.TotalPrice
            });
            _unit.SaveChanges();
        }
    }
}