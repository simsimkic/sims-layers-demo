using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimsLayersDemo.Model;

namespace SimsLayersDemo.Repository
{
    public class TransactionRepository
    {
        private readonly string _path;
        private readonly string _delimiter;
        private readonly string _datetimeFormat;
        private long _transactionMaxId;

        public TransactionRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
            _transactionMaxId = GetMaxId(GetAll());
        }

        private long GetMaxId(IEnumerable<Transaction> transactions)
        {
            return transactions.Count() == 0 ? 0 : transactions.Max(transaction => transaction.Id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToTransaction)
                .ToList();
        }

        public Transaction Create(Transaction transaction)
        {
            transaction.Id = ++_transactionMaxId;
            AppendLineToFile(_path, ConvertTransactionToCSVFormat(transaction));
            return transaction;
        }

        private Transaction ConvertCSVFormatToTransaction(string transactionCSVFormat)
        {
            var tokens = transactionCSVFormat.Split(_delimiter.ToCharArray());
            return new Transaction(
                long.Parse(tokens[0]),
                tokens[1],
                DateTime.Parse(tokens[2]),
                double.Parse(tokens[3]),
                new Client(long.Parse(tokens[4])),
                new Client(long.Parse(tokens[5])));
        }

        private string ConvertTransactionToCSVFormat(Transaction transaction)
        {
            return string.Join(_delimiter,
                transaction.Id,
                transaction.Purpose,
                transaction.Date.ToString(_datetimeFormat),
                transaction.Amount,
                transaction.Payer.Id,
                transaction.Receiver.Id);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
