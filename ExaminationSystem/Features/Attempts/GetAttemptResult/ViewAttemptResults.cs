using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptResult
{
    public record ViewAttemptResults(int studentId, int id) : IRequest<ViewResultDTO>;

    


}
