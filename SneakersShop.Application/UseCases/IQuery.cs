namespace SneakersShop.Application.UseCases;

public interface IQuery<TRequest, TResult> : IUseCase
{
    TResult Execute(TRequest search);
}

public interface IQuery<TResult> : IUseCase
{
    TResult Execute();
}