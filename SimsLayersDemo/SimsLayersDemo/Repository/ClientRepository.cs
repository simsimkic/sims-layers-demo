using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimsLayersDemo.Exception;
using SimsLayersDemo.Model;

namespace SimsLayersDemo.Repository
{
    public class ClientRepository
    {
        private const string NOT_FOUND_ERROR = "Loan with {0}:{1} can not be found!";
        private readonly string _path;
        private readonly string _delimiter;

        public ClientRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public IEnumerable<Client> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(CSVFormatToClient)
                .ToList();
        }

        public Client Get(long id)
        {
            try
            {
                return GetAll().SingleOrDefault(client => client.Id == id);
            }
            catch (ArgumentException)
            {
                throw new NotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }
        }

        private Client CSVFormatToClient(string clientCSVFormat)
        {
            var tokens = clientCSVFormat.Split(_delimiter.ToCharArray());
            return new Client(
                long.Parse(tokens[0]),
                tokens[1], tokens[2],
                DateTime.Parse(tokens[3]),
                new Account(long.Parse(tokens[4])));
        }
    }
}
