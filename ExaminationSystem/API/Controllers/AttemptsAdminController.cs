using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.AnswerQuestion;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
﻿using ExaminationSystem.BuildingBlocks.Helpers;
//using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.Attempts;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.Features.Attempts.GetAttempts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExaminationSystem.Features.Attempts.studemtAttemptsForAdmin;
using System.Threading.Tasks;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/student/attempts")]
    [ApiController]
    public class AttemptsAdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AttemptsAdminController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAttemptsForAdmin([FromQuery]int pageIndex,[FromQuery] int pageSize)
        {
            var result =await _mediator.Send(new ViewStudentAttemptsForAdmin(pageIndex, pageSize));
            return Ok(result);
        }
    }
}
