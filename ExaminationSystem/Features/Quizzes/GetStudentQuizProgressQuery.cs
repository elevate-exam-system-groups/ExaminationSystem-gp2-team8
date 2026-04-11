using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using MediatR;

namespace ExaminationSystem.Features.Quizzes
{
    public record GetStudentQuizProgressQuery(int studentId):IRequest<Dictionary<int,int>>;
    
    //public class GetStudentQuizProgressQueryHandler : IRequestHandler<GetStudentQuizProgressQuery, Dictionary<int,int>>
    //{
    //    private readonly IGeneralRepository<Attempts> _repository;
    //    public GetStudentQuizProgressQueryHandler(IGeneralRepository<A> repository)
    //    {
    //        _repository = repository;
    //    }

    //    public Task<Dictionary<int, int>> Handle(GetStudentQuizProgressQuery request, CancellationToken cancellationToken)
    //    {
            
    //        throw new NotImplementedException();
    //    }
    //}
}
