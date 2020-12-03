using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Netcool.Api.Domain.Configuration
{
    public class EfConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        private readonly bool _reloadOnChange = false;

        public EfConfigurationSource(Action<DbContextOptionsBuilder> optionsAction, bool reloadOnChange = true)
        {
            _optionsAction = optionsAction;
            _reloadOnChange = reloadOnChange;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EfConfigurationProvider(_optionsAction, _reloadOnChange);
        }
    }
}