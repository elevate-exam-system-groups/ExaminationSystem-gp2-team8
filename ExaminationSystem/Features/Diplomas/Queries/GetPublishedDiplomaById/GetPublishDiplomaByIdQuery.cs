using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Enrollments;
using ExaminationSystem.Features.Quizzes.DTOS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetPublishedDiplomaById
{
    public record GetPublishDiplomaByIdQuery(int id) : IRequest<DiplomasummaryDTO?>;


  
}
