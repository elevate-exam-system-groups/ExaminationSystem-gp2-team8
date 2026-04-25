using ExaminationSystem.Features.Analytics.DTO;
using MediatR;

namespace ExaminationSystem.Features.Analytics.Orchestrators
{
    public record AnalyticsOrchestrator(
       FiltersElement Filters


        ) :IRequest<AnalysticsWithNoFilterDTO>;
    
}
