using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using MicroServicioUser.Dom.Patterns;
using MicroServicoUser.Inf;
<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> AnalisisSonarEstablishment

namespace MicroServicioUser.App.Services
{
    public class UserService : IRepositoryService<User>
    {
        private readonly IRepository userRepository;

        public UserService(IRepository userRepository)
        {
            this.userRepository = userRepository;
        }
<<<<<<< HEAD

=======
>>>>>>> AnalisisSonarEstablishment
        public async Task<Result<int>> Insert(User t)
        {
            t.CreatedDate = DateTime.Now;
            t.LastUpdate = DateTime.Now;
            t.Status = true;
            t.FirstLogin = 1;
<<<<<<< HEAD

=======
            
>>>>>>> AnalisisSonarEstablishment
            return await userRepository.Insert(t);
        }

        public async Task<Result<int>> Update(User t)
        {
            t.LastUpdate = DateTime.Now;

            return await userRepository.Update(t);
        }

<<<<<<< HEAD
        public async Task<Result<User?>> GetById(int id)
=======
        public async Task<Result<User>> GetById(int id)
>>>>>>> AnalisisSonarEstablishment
        {
            return await userRepository.GetById(id);
        }

        public async Task<Result<int>> Delete(int id)
        {
            return await userRepository.Delete(id);
        }

        public async Task<Result<IEnumerable<User>>> GetAll()
        {
<<<<<<< HEAD
            return await userRepository.GetAll();
=======
            return  await userRepository.GetAll();
>>>>>>> AnalisisSonarEstablishment
        }

        public async Task<Result<IEnumerable<User>>> Search(string property)
        {
            return await userRepository.Search(property);
        }
    }
}