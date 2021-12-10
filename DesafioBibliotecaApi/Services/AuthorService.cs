﻿using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorService(AuthorRepository repository)
        {
            _authorRepository = repository;
        }

        public AuthorDTO Create(Author author)
        {
            var authorExists = _authorRepository.GetByUsername(author.Name);

            if (authorExists != null)
                throw new Exception("The author is already registered, try another one!");

            var userCreated = _authorRepository.Create(author);

            return new AuthorDTO
            {
                Name = userCreated.Name,
                Lastname = userCreated.Lastname,
                Nacionality = userCreated.Nacionality,
                Age = userCreated.Age,
                Id = userCreated.Id
            };

        }

        public IEnumerable<AuthorDTO> Get()
        {
            var authors = _authorRepository.Get();

            return authors.Select(a =>
            {
                return new AuthorDTO
                {
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Nacionality = a.Nacionality,
                    Age = a.Age,
                    Id = a.Id
                };
            });
        }

        public IEnumerable<AuthorDTO> GetFilter(string name, string nationality, int age, int page, int itens)
        {
            var authors = _authorRepository.Get();

            return authors.Select(a =>
            {
                return new AuthorDTO
                {
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Nacionality = a.Nacionality,
                    Age = a.Age,
                    Id = a.Id
                };
            });

        }

        public AuthorDTO Get(Guid id)
        {
            var author = _authorRepository.Get(id);

            return new AuthorDTO
            {
                Name = author.Name,
                Lastname = author.Lastname,
                Nacionality = author.Nacionality,
                Age = author.Age,
                Id = author.Id
            };
        }

        public bool Delete(Guid id)
        {
            return _authorRepository.Remove(id);

        }
        public Author UpdateAuthor(Guid id, Author author)
        {
            var authors = _authorRepository.Get(id);

            authors.Update(author);
            return authors;

        }

    }
}
