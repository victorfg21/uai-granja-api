using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UaiGranja.Core.DomainObjects
{
    public class AggregateDomainException : AggregateException
    {
        public AggregateDomainException()
        { }

        public AggregateDomainException(AggregateException aggregate) : base(aggregate)
        { }

        public AggregateDomainException(List<Exception> errors) : base(errors)
        { }

        public static List<Exception> GetAggregateDomainException(List<ValidationFailure> errors)
        {
            var exceptions = new List<Exception>();
            errors.ForEach(error =>
            {
                exceptions.Add(new Exception(error.ErrorMessage));
            });

            return exceptions;
        }
    }
}
