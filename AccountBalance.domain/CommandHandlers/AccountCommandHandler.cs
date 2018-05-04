﻿using System.Reactive.Disposables;

namespace AccountBalance.Domain.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using ReactiveDomain.Foundation;
    using ReactiveDomain.Messaging;
    using ReactiveDomain.Messaging.Bus;

    public class AccountCommandHandler :
        IHandleCommand<CreateAccount>,
        IHandleCommand<LimitOverdraft>,
        IHandleCommand<SetDailyWireTransfertLimit>,
        IDisposable
    {
        private readonly IRepository _repository;
        //private readonly IList<IDisposable> _subscriptionList;
        readonly CompositeDisposable _disposable;
        public AccountCommandHandler(IRepository repository, ICommandSubscriber dispatcher)
        {
            _repository = repository;
            _disposable = new CompositeDisposable
            {
                dispatcher.Subscribe<CreateAccount>(this),
                dispatcher.Subscribe<LimitOverdraft>(this),
                dispatcher.Subscribe<SetDailyWireTransfertLimit>(this),
            };
        }
        public void Dispose()
        {
            _disposable?.Dispose();
        }

        //public AccountCommandHandler(IRepository repository, ICommandSubscriber dispatcher)
        //{
        //    _repository = repository;
        //    _subscriptionList = new List<IDisposable>
        //    {
        //        dispatcher.Subscribe<CreateAccount>(this),
        //        dispatcher.Subscribe<LimitOverdraft>(this),
        //        dispatcher.Subscribe<SetDailyWireTransfertLimit>(this)
        //    };
        //}

        //public void Dispose()
        //{
        //    foreach (var disposable in _subscriptionList)
        //    {
        //        disposable?.Dispose();
        //    }
        //}


        public CommandResponse Handle(CreateAccount command)
        {
            try
            {
                if (_repository.TryGetById<Account>(command.AccountId, out var acc))
                    throw new InvalidOperationException("Account is already exist");

                var account = Account.Create(command.AccountId, command.AccountHolderName, command);

                _repository.Save(account);
                return command.Succeed();
            }
            catch (Exception e)
            {
                return command.Fail(e);
            }
        }

        public CommandResponse Handle(LimitOverdraft command)
        {
            try
            {
                if (!_repository.TryGetById<Account>(command.AccountId, out var account))
                    throw new InvalidOperationException("No Account is already exist");

                account.SetOverdraftLimit(command.AccountId, command.Limit, command);

                _repository.Save(account);
                return command.Succeed();
            }
            catch (Exception e)
            {
                return command.Fail(e);
            }
        }

        public CommandResponse Handle(SetDailyWireTransfertLimit command)
        {
            try
            {
                if (!_repository.TryGetById<Account>(command.AccountId, out var account))
                    throw new InvalidOperationException("No Account is already exist");

                account.SetDailyWireTransfertLimit(command.AccountId, command.Limit, command);

                _repository.Save(account);
                return command.Succeed();
            }
            catch (Exception e)
            {
                return command.Fail(e);
            }
        }
    }
}
