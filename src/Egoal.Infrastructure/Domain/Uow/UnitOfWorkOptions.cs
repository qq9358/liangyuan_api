using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Egoal.Domain.Uow
{
    public class UnitOfWorkOptions
    {
        public TransactionScopeOption? Scope { get; set; }
        public bool? IsTransactional { get; set; }
        public TimeSpan? Timeout { get; set; }
        public System.Data.IsolationLevel? IsolationLevel { get; set; }
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }
        public List<DataFilterConfiguration> FilterOverrides { get; }

        public UnitOfWorkOptions()
        {
            FilterOverrides = new List<DataFilterConfiguration>();
        }

        internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        {
            //TODO: Do not change options object..?

            if (!IsTransactional.HasValue)
            {
                IsTransactional = defaultOptions.IsTransactional;
            }

            if (!Scope.HasValue)
            {
                Scope = defaultOptions.Scope;
            }

            if (!Timeout.HasValue && defaultOptions.Timeout.HasValue)
            {
                Timeout = defaultOptions.Timeout.Value;
            }

            if (!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
            {
                IsolationLevel = defaultOptions.IsolationLevel.Value;
            }
        }

        internal void FillOuterUowFiltersForNonProvidedOptions(List<DataFilterConfiguration> filterOverrides)
        {
            foreach (var filterOverride in filterOverrides)
            {
                if (FilterOverrides.Any(fo => fo.FilterName == filterOverride.FilterName))
                {
                    continue;
                }

                FilterOverrides.Add(filterOverride);
            }
        }
    }
}
