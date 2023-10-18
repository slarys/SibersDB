using AutoMapper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Sibers.Entities;
using Sibers.Entities.Commands;
using Sibers.Entities.Models;
using Sibers.Entities.Queries;
using Sibers.Entities.Request;
using Sibers.Entities.Responses;
using Sibers.Entities.Validation;

namespace SibersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public ProjectsController(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        // Получение списка проектов с использованием OData
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            ODataQueryOptions<Project> queryOptions,
            CancellationToken cancellationToken) =>
            await sender
                .Send(new GetProjectsQuery(queryOptions, ProjectValidation.Default), cancellationToken)
                .Map(Ok);

        // Получение проекта по идентификатору
        [HttpGet("{projectId:guid}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(
            Guid projectId, CancellationToken cancellationToken) =>
            await Catch<GetProjectByIdQuery>
                .From(new GetProjectByIdQuery(projectId))
                .Bind(projectByIdQuery => sender.Send(projectByIdQuery, cancellationToken))
                .Match(Ok, NotFound);

        // Создание проекта на основе запроса
        [HttpPost]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateProject(
            [FromBody] CreateProjectRequest createProjectRequest,
            CancellationToken cancellationToken) =>
            await Result
                .Create(createProjectRequest, Sibers.Entities.Strings.ApiErrors_UnProcessableRequest)
                .Map(request => mapper.Map<CreateProjectCommand>(request))
                .Bind(command => sender.Send(command, cancellationToken))
                .Match(CreatedAtAction, BadRequest);

        // Обновление проекта на основе запроса
        [HttpPut]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateBudget(
            [FromBody] UpdateProjectRequest updateProjectRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(updateProjectRequest, Sibers.Entities.Strings.ApiErrors_UnProcessableRequest)
                .Map(request => mapper.Map<UpdateProjectCommand>(request))
                .Bind(command => sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        // Удаление проекта по идентификатору
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudget(Guid budgetId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteBudgetCommand(budgetId))
                .Bind(command => sender.Send(command, cancellationToken))
                .Match(NoContent, NotFound);

        // Создание объекта OkObjectResult с кодом 200
        protected new IActionResult Ok(object value) => base.Ok(value);

        protected new IActionResult NoContent() => base.NoContent();

        // Создание объекта CreatedAtAction с указанием имени метода и значениями
        protected IActionResult CreatedAtAction<TId>(BaseResponse<TId> value) =>
            base.CreatedAtAction(nameof(GetProjectById), new { id = value.Id }, value);

        // Создание объекта NotFoundResult с кодом 404
        protected new IActionResult NotFound() => base.NotFound();

        // Создание объекта BadRequestObjectResult с кодом 400 на основе ошибки
        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    }
}
