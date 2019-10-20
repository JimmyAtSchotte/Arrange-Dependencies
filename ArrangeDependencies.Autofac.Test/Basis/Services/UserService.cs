using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArrangeDependencies.Autofac.Test.Basis.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public void Log()
        {
            _logger.LogInformation("Log test");
        }
    }
}
