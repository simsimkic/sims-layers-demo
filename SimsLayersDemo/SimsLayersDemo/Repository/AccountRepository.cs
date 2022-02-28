using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimsLayersDemo.Exception;
using SimsLayersDemo.Model;

namespace SimsLayersDemo.Repository
{
    public class AccountRepository
    {
        private const string NOT_FOUND_ERROR = "Account with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public AccountRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public IEnumerable<Account> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToAccount)
                .ToList();
        }

        public Account Get(long id)
        {
            try
            {
                {
                    return GetAll().SingleOrDefault(account => account.Id == id);
                }
            }
            catch (ArgumentException)
            {
                {
                    throw new NotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
                }
            }
        }

        private Account ConvertCSVFormatToAccount(string acountCSVFormat)
        {
            var tokens = acountCSVFormat.Split(_delimiter.ToCharArray());
            return new Account(long.Parse(tokens[0]), tokens[1], double.Parse(tokens[2]));
        }
    }
}
