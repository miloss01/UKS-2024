﻿using DockerHubBackend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace DockerHubBackend.Filters
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                ForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                Error = context.Exception.Message              
            };

            context.HttpContext.Response.StatusCode = statusCode;
            context.HttpContext.Response.ContentType = "application/json";

            
            var jsonResponse = JsonSerializer.Serialize(response);
            context.HttpContext.Response.WriteAsync(jsonResponse).ConfigureAwait(false);

            context.ExceptionHandled = true;
        }
    }
}