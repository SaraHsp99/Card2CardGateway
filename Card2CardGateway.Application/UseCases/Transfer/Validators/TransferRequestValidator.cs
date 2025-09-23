using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.Options;
using FluentValidation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Validators
{
    public class TransferRequestValidator : AbstractValidator<TransferRequestDto>
    {
        public TransferRequestValidator(IOptions<TransferOptions> opts)
        {
            var min = opts.Value.MinAmount;
            var max = opts.Value.MaxAmount;

            RuleFor(x => x.RequestTraceId).NotEmpty(); 
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(min).LessThanOrEqualTo(max);

            RuleFor(x => x.SourceCardNumber)
                .NotEmpty()
                .Length(16)
                .Matches(@"^\d{16}$");

            RuleFor(x => x.DestinationCardNumber)
                .NotEmpty()
                .Length(16)
                .Matches(@"^\d{16}$");

            RuleFor(x => x.Cvv2)
                .NotEmpty()
                .Matches(@"^\d{3}$");

            RuleFor(x => x.Pin2)
                .NotEmpty()
                .Matches(@"^\d{4,10}$");

            RuleFor(x => x.ExpiredDate)
                .NotEmpty()
                .Matches(@"^\d{4}$")
                .Must(NotBeExpired).WithMessage("Card expired");
        }

        private bool NotBeExpired(string yyMM)
        {
            if (yyMM?.Length != 4) return false;
            if (!int.TryParse(yyMM.Substring(0, 2), out var yy)) return false;
            if (!int.TryParse(yyMM.Substring(2, 2), out var mm)) return false;
            var year = 2000 + yy;
            try
            {
                var lastDay = DateTime.DaysInMonth(year, mm);
                var exp = new DateTime(year, mm, lastDay, 23, 59, 59);
                return exp >= DateTime.UtcNow;
            }
            catch { return false; }
        }
    }


}
