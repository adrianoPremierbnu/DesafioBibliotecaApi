using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class EmployeeService
    {
        private readonly UserRepository _userRepository;
        private readonly ClientRepository _clientRepository;

        public EmployeeService(UserRepository userRepository, ClientRepository clientRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
        }

        public UserDTO Create(User user)
        {
            var userExists = _userRepository.GetByUsername(user.UserName);

            if (userExists != null)
                throw new Exception("The username is already in use, try another one!");

            if (!_userRepository.Create(user))
                throw new Exception("User cannot be created!");

            return new UserDTO
            {
                Role = user.Role,
                Username = user.UserName,
                Id = user.Id

            };

        }

        public IEnumerable<UserResultDTO> GetFilter(string? name = null, DateTime? birthdate = null, string? document = null, int page = 1, int itens = 50)
        {
            var users = _userRepository.Get();
            var clients = _clientRepository.Get();

            var query = from user in users
                        join client in clients on user.Id equals client.IdUser
                        select new { Username = user.UserName, Role = user.Role, Id = user.Id, Client = client };

            if (!String.IsNullOrEmpty(name))
                query = query.Where(x => x.Client.Name == name);

            if (!String.IsNullOrEmpty(document))
                query = query.Where(x => x.Client.Document == document);

            if (birthdate.HasValue)
                query = query.Where(x => x.Client.Birthdate.ToString("MM/dd/yyyy") == birthdate.Value.ToString("MM/dd/yyyy"));

            return query.Select(u =>
            {
                return new UserResultDTO
                {
                    Role = u.Role,
                    Username = u.Username,
                    Id = u.Id,
                    Client = new ClientDTO
                    {
                        Adress = new AdressDTO
                        {
                            Street = u.Client.Adress.Street,
                            Complement = u.Client.Adress.Complement,
                            District = u.Client.Adress.District,
                            Location = u.Client.Adress.Location,
                            State = u.Client.Adress.State
                        },
                        Document = u.Client.Document,
                        Lastname = u.Client.Lastname,
                        Name = u.Client.Name,
                        ZipCode = u.Client.ZipCode,
                        Birthdate = u.Client.Birthdate,
                        Id = u.Client.Id,

                    }

                };
            }).Skip((page - 1) * itens).Take(itens);

        }

        public ClientDTO Get(Guid idUser)
        {
            var user = _userRepository.Get(idUser);
            var client = _clientRepository.GetIdUser(user.Id);

            return new ClientDTO
            {
                Adress = new AdressDTO
                {
                    Street = client.Adress.Street,
                    Complement = client.Adress.Complement,
                    District = client.Adress.District,
                    Location = client.Adress.Location,
                    State = client.Adress.State
                },
                Document = client.Document,
                Lastname = client.Lastname,
                Name = client.Name,
                ZipCode = client.ZipCode,
                Birthdate = client.Birthdate,
                Id = client.Id

            };

        }

    }
}
