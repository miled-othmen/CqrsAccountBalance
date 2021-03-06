﻿namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;
    using ReactiveDomain;
    using System.Collections.Generic;
    using NodaTime;

    public class Account : EventDrivenStateMachine
    {
        private double _overdraftLimit;
        private double _dailyWireTransferLimit;
        private double _dailyWireTransferAmount;
        private List<Cheque> _depositedCheques = new List<Cheque>();
        private double _balance;
        private bool _blocked;

        Account()
        {
            Register<AccountCreated>(e => { Id = e.AccountId; });
            Register<OverdraftLimited>(e => _overdraftLimit = e.Limit);
            Register<DailyWireTransfertLimitSet>(e => _dailyWireTransferLimit = e.Limit);
            Register<ChequeDeposited>(e =>
            {
                _depositedCheques.Add(
                    new Cheque
                    {
                        ChequeId = e.ChequeId,
                        Amount = e.Amount,
                        DepositAt = e.DepositTime,
                        ClearanceAt = e.ClearanceTime,
                        IsCleared = false
                    }
                );
            });
            Register<CashDeposited>(e => _balance = _balance + e.Amount);
            Register<CashWithdrawn>(e => _balance = _balance - e.Amount);
            Register<CashWireTransfered>(e =>
            {
                _balance = _balance - e.Amount;
                _dailyWireTransferAmount = _dailyWireTransferAmount + e.Amount;
            });
            Register<AccountBlocked>(e => _blocked = true);
            Register<AccountUnblocked>(e => _blocked = false);
        }

        public static Account Create(Guid id, string name, CorrelatedMessage source)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("A valid account owner name must be provided");

            var account = new Account();
            account.Raise(new AccountCreated(source)
            {
                AccountId = id,
                AccountHolderName = name
            });
            return account;
        }
        public void SetOverdraftLimit(Guid id, int limit, CorrelatedMessage source)
        {
            if (limit < 0)
                throw new InvalidOperationException("Overdraft limit should be positve");

            Raise(new OverdraftLimited(source)
            {
                AccountId = id,
                Limit = limit
            });
        }

        public void SetDailyWireTransfertLimit(Guid id, int limit, CorrelatedMessage source)
        {
            if (limit < 0)
                throw new InvalidOperationException("Daily transfert limit should be positve");

            Raise(new DailyWireTransfertLimitSet(source)
            {
                AccountId = id,
                Limit = limit
            });
        }

        public void DepositCheque(Guid chequeId, double amount, IClock clock, CorrelatedMessage source)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Cheque's amount must be strictly positive");

            var depositTime = clock.GetCurrentInstant();

            Instant clearanceTime = depositTime;

            switch (clearanceTime.InUtc().DayOfWeek)
            {
                case IsoDayOfWeek.Saturday:
                    clearanceTime.Plus(Duration.FromDays(2));
                    break;
                case IsoDayOfWeek.Sunday:
                    clearanceTime.Plus(Duration.FromDays(1));
                    break;
            }

            Raise(new ChequeDeposited(source)
            {
                AccountId = Id,
                ChequeId = chequeId,
                Amount = amount,
                DepositTime = depositTime,
                ClearanceTime = clearanceTime
            });
        }

        public void DepositCash(Guid id, double amount, IClock clock, CorrelatedMessage source)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Cash's amount must be strictly positive");

            var depositTime = clock.GetCurrentInstant();

            Raise(new CashDeposited(source)
            {
                AccountId = Id,
                Amount = amount,
                DepositedAt = depositTime
            });

            if (_blocked)
                Raise(new AccountUnblocked(source)
                {
                    AccountId = Id,
                    Raison = "Cash Deposited"
                });
        }

        public void WithdrawCash(Guid id, double amount, IClock clock, CorrelatedMessage source)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawn Cash's amount must be strictly positive");

            if (_blocked)
                throw new InvalidOperationException("Account is blocked");

            if (amount > _balance + _overdraftLimit)
                Raise(new AccountBlocked(source)
                {
                    AccountId = Id,
                    Raison = "amount > _balance + _overdraftLimit"
                });

            var withdrawnTime = clock.GetCurrentInstant();

            Raise(new CashWithdrawn(source)
            {
                AccountId = Id,
                Amount = amount,
                WithdrawnAt = withdrawnTime
            });
        }

        public void WireTransferCash(Guid id, double amount, IClock clock, CorrelatedMessage source)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Wire transfer Cash's amount must be strictly positive");

            if (_blocked)
                throw new InvalidOperationException("Account is blocked");

            if (_dailyWireTransferAmount + amount > _dailyWireTransferLimit)
                Raise(new AccountBlocked(source)
                {
                    AccountId = Id,
                    Raison = "_dailyWireTransferLimit attempted"
                });

            var wiretransferTime = clock.GetCurrentInstant();

            Raise(new CashWireTransfered(source)
            {
                AccountId = Id,
                Amount = amount,
                WireTransferedAt = wiretransferTime
            });
        }
    }
}
