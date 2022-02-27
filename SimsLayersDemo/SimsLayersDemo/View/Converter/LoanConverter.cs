using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimsLayersDemo.Model;
using SimsLayersDemo.View.Model;

namespace SimsLayersDemo.View.Converter
{
    class LoanConverter : AbstractConverter
    {
        public static LoanView ConvertLoanToLoanView(Loan loan)
            => new LoanView
            {
                ApprovalDate = loan.ApprovalDate,
                Deadline = loan.Deadline,
                Client = loan.Client.FirstName + " " + loan.Client.LastName,
                ClientAccount = loan.Client.Account.Number,
                Base = loan.Base,
                InterestRate = loan.InterestRate,
                NumberOfInstallments = loan.NumberOfInstallments,
                InstallmentAmount = loan.InstallmentAmount,
                NumberOfPaidInstallments = loan.NumberOfPaidInstallments
            };


        public static IList<LoanView> ConvertLoanListToLoanViewList(IList<Loan> loans)
            => ConvertEntityListToViewList(loans, ConvertLoanToLoanView);
    }
}
