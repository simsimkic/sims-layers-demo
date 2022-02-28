using System;
using System.Collections.Generic;
using System.Linq;
using SimsLayersDemo.Exception;
using SimsLayersDemo.Model;
using SimsLayersDemo.Repository;

namespace SimsLayersDemo.Service
{
    public class LoanService
    {
        private readonly ClientService _clientService;
        private readonly LoanRepository _loanRepo;

        private const string INVALID_DATE_ERROR = "Deadline: {loan.Deadline} is before approval date: {loan.ApprovalDate}";

        public LoanService(ClientService clientService, LoanRepository loanRepo)
        {
            _clientService = clientService;
            _loanRepo = loanRepo;
        }

        internal IEnumerable<Loan> GetAll()
        {
            var clients = _clientService.GetAll();
            var loans = _loanRepo.GetAll();
            BindClientsWithLoans(clients, loans);
            return loans;
        }

        private void BindClientsWithLoans(IEnumerable<Client> clients, IEnumerable<Loan> loans)
        {
            loans.ToList().ForEach(loan => loan.Client = FindClientById(clients, loan.Client.Id));
        }

        private Client FindClientById(IEnumerable<Client> clients, long id)
        {
            return clients.SingleOrDefault(client => client.Id == id);
        }

        internal Loan Create(Loan loan)
        {
            loan.ApprovalDate = DateTime.Now;
            if (IsDeadlineAfterApprovalDate(loan))
            {
                loan.NumberOfPaidInstallments = 0;
                loan.NumberOfInstallments = CalculateNumberOfInstallments(loan);
                loan.InstallmentAmount = CalculateInstallmentAmount(loan);
                ApproveLoan(loan);

                //save accounts
                return _loanRepo.Create(loan);
            }
            else
            {
                throw new InvalidDateException(string.Format(INVALID_DATE_ERROR,
                    loan.Deadline, loan.ApprovalDate));
            }
        }

        private bool IsDeadlineAfterApprovalDate(Loan loan)
        {
            return loan.Deadline > loan.ApprovalDate;
        }

        private long CalculateNumberOfInstallments(Loan loan)
        {
            return ((loan.Deadline.Year - loan.ApprovalDate.Year) * 12) + loan.Deadline.Month - loan.ApprovalDate.Month;
        }

        private double CalculateInstallmentAmount(Loan loan)
        {
            return (loan.Base * (1 + loan.InterestRate / 100)) / loan.NumberOfInstallments;
        }

        private void ApproveLoan(Loan loan)
        {
            loan.Client.Account.Balance += loan.Base;
        }
    }
}
