using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Repository;

namespace MarketPlace.Application.Services.Implementations
{
   public class UserService : IUserService
   {
       #region constractore

       private readonly IGenericRepository<User> _usesRepository ;

       public UserService(IGenericRepository<User> usesRepository)
       {
           _usesRepository = usesRepository;
       }
        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _usesRepository.DisposeAsync();
        }

        #endregion
        
    }
}
