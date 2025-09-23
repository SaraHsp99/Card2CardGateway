using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Services
{
    public class BankServiceFactory : IBankServiceFactory
    {
        private readonly Dictionary<string, IBankTransferService> _binToService;

        public BankServiceFactory(IEnumerable<IBankTransferService> services)
        {
           
            _binToService = new Dictionary<string, IBankTransferService>();

            foreach (var service in services)
            {
                switch (service.GetBankName())
                {
                    case "Saman":
                        _binToService["621986"] = service;
                        break;
                    case "Pasargad":
                        _binToService["502229"] = service;
                        break;
                        
                }
            }
        }

        public IBankTransferService Resolve(string sourceCardNumber)
        {
            if (string.IsNullOrWhiteSpace(sourceCardNumber) || sourceCardNumber.Length < 6)
                throw new ArgumentException("Invalid card number");

            var bin = sourceCardNumber.Substring(0, 6);

            if (_binToService.TryGetValue(bin, out var service))
                return service;

            throw new NotSupportedException($"No bank service found for BIN {bin}");
        }
    }
}

