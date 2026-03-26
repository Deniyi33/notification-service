using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feex.Core.Request;
using FluentValidation;

namespace Feex.Application.Validators
{

    public class DivisionRequestDtoValidator : AbstractValidator<DivisionRequestDto>
    {
        public DivisionRequestDtoValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.DivisionName) || x.DivisionalHeadId.HasValue)
                .WithMessage("Either DivisionName or DivisionalHeadId must be provided.");
        }
    }
}
