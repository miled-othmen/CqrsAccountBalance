namespace AccountBalance.Domain.CommandHandlers
{
    using System;
    using ReactiveDomain.Foundation;
    using ReactiveDomain.Messaging;
    using ReactiveDomain.Messaging.Bus;

    public class AccountCommandHandler : IHandleCommand<CreateAccount>, IDisposable
    {
         readonly IRepository _repository;
         readonly IDisposable _disposable;

        public AccountCommandHandler(IRepository repository, ICommandSubscriber dispatcher)
        {
            _repository = repository;
            _disposable = dispatcher.Subscribe(this);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public CommandResponse Handle(CreateAccount command)
        {
            try
            {
                if (_repository.TryGetById<Account>(command.AccountId, out var ex))
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
    }
}
