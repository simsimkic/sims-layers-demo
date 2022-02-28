using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimsLayersDemo.Model;

namespace SimsLayersDemo.Repository
{
    public class LoanRepository
    {
        private readonly string _path;
        private readonly string _delimiter;
        private readonly string _datetimeFormat;
        private long _loanMaxId;

        public LoanRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
            _loanMaxId = GetMaxId(GetAll());
        }

        private long GetMaxId(IEnumerable<Loan> loans)
        {
            return loans.Count() == 0 ? 0 : loans.Max(loan => loan.Id);
        }

        public IEnumerable<Loan> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToLoan)
                .ToList();
        }

        public Loan Create(Loan loan)
        {
            loan.Id = ++_loanMaxId;
            AppendLineToFile(_path, ConvertLoanToCSVFormat(loan));
            return loan;
        }

        private Loan ConvertCSVFormatToLoan(string loanCSVFormat)
        {
            string[] tokens = loanCSVFormat.Split(_delimiter.ToCharArray());
            return new Loan(
                long.Parse(tokens[0]),
                new Client(long.Parse(tokens[1])),
                DateTime.Parse(tokens[2]),
                DateTime.Parse(tokens[3]),
                double.Parse(tokens[4]),
                double.Parse(tokens[5]),
                long.Parse(tokens[6]),
                double.Parse(tokens[7]),
                long.Parse(tokens[8]));
        }

        private string ConvertLoanToCSVFormat(Loan loan)
        {
            return string.Join(_delimiter,
                loan.Id,
                loan.Client.Id,
                loan.ApprovalDate.ToString(_datetimeFormat),
                loan.Deadline.ToString(_datetimeFormat),
                loan.Base,
                loan.InterestRate,
                loan.NumberOfInstallments,
                loan.InstallmentAmount,
                loan.NumberOfPaidInstallments);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
