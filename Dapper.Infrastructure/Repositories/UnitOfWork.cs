using Dapper.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(IProductRepository productRepository, IUserRepository userRepository)
        {
            UserRepository = userRepository;
            ProductRepository = productRepository;
        }
    }
}
