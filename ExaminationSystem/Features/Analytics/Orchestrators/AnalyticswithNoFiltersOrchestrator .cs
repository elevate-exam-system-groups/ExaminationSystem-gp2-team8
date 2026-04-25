using ExaminationSystem.Features.Analytics.DTO;
using MediatR;

namespace ExaminationSystem.Features.Analytics.Orchestrators
{
    public record AnalyticswithNoFiltersOrchestrator(
       FiltersElement Filters


        ) :IRequest<AnalysticsWithNoFilterDTO>;
    
}
