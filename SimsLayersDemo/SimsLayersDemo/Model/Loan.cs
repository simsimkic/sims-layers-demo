﻿using System;

namespace SimsLayersDemo.Model
{
    public class Loan
    {
        public long Id { get; set; }
        internal Client Client { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime Deadline { get; set; }
        public double Base { get; set; }
        public double InterestRate { get; set; }
        public long NumberOfInstallments { get; set; }
        public double InstallmentAmount { get; set; }
        public long NumberOfPaidInstallments { get; set; }

        public Loan(Client client, DateTime deadline, double @base, double interestRate)
        {
            Client = client;
            Deadline = deadline;
            Base = @base;
            InterestRate = interestRate;
        }

        public Loan(long id, Client client, DateTime approvalDate, DateTime deadline,
            double @base, double interestRate, long numberOfInstallments,
            double installmentAmount, long numberOfPaidInstallments)
        {
            Id = id;
            Client = client;
            ApprovalDate = approvalDate;
            Deadline = deadline;
            Base = @base;
            InterestRate = interestRate;
            NumberOfInstallments = numberOfInstallments;
            InstallmentAmount = installmentAmount;
            NumberOfPaidInstallments = numberOfPaidInstallments;
        }
    }
}