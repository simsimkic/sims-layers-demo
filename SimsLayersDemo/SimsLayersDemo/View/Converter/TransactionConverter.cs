using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimsLayersDemo.Model;
using SimsLayersDemo.View.Model;

namespace SimsLayersDemo.View.Converter
{
    class TransactionConverter : AbstractConverter
    {
        public static TransactionView ConvertTransactionToTransactionView(Transaction transaction)
            => new TransactionView
            {
                Date = transaction.Date,
                Purpose = transaction.Purpose,
                Payer = transaction.Payer.FirstName + " " + transaction.Payer.LastName,
                PayerAccount = transaction.Payer.Account.Number,
                Receiver = transaction.Receiver.FirstName + " " + transaction.Receiver.LastName,
                ReceiverAccount = transaction.Receiver.Account.Number,
                Amount = transaction.Amount
            };


        public static IList<TransactionView> ConvertTransactionListToTransactionViewList(IList<Transaction> transactions)
            => ConvertEntityListToViewList(transactions, ConvertTransactionToTransactionView);
    }
}
