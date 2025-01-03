import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-logs',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './logs.component.html',
  styleUrl: './logs.component.css'
})
export class LogsComponent {
  query: string = ''; 
  startDate: Date | null = null; 
  startTime: string = '';
  endDate: Date | null = null;   
  endTime: string = '';
  logs: any[] = [
    {
        "timestamp": "2025-01-03T16:35:30.6709214+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - 200 null \"application/json;charset=utf-8\" 170.2501ms"
    },
    {
        "timestamp": "2025-01-03T16:35:30.5006128+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:35:29.8347879+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - 200 null \"text/javascript; charset=UTF-8\" 44.3813ms"
    },
    {
        "timestamp": "2025-01-03T16:35:29.8082919+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - 200 13728 \"application/javascript; charset=utf-8\" 13.1607ms"
    },
    {
        "timestamp": "2025-01-03T16:35:29.7949418+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:35:29.7910341+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:35:29.7752429+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - 200 null \"text/html;charset=utf-8\" 186.2008ms"
    },
    {
        "timestamp": "2025-01-03T16:35:29.5893884+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:35:29.112146+01:00",
        "level": "Information",
        "message": "Content root path: \"C:\\Users\\anast\\Desktop\\uks\\UKS-2024\\DockerHubBackend\\DockerHubBackend\""
    },
    {
        "timestamp": "2025-01-03T16:35:29.1076043+01:00",
        "level": "Information",
        "message": "Hosting environment: \"Development\""
    },
    {
        "timestamp": "2025-01-03T16:35:29.1029848+01:00",
        "level": "Information",
        "message": "Application started. Press Ctrl+C to shut down."
    },
    {
        "timestamp": "2025-01-03T16:35:29.0939913+01:00",
        "level": "Information",
        "message": "Now listening on: \"http://localhost:5156\""
    },
    {
        "timestamp": "2025-01-03T16:35:28.8575868+01:00",
        "level": "Information",
        "message": "User profile is available. Using '\"C:\\Users\\anast\\AppData\\Local\\ASP.NET\\DataProtection-Keys\"' as key repository and Windows DPAPI to encrypt keys at rest."
    },
    {
        "timestamp": "2025-01-03T16:31:12.8856678+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 200 null \"application/json; charset=utf-8\" 1304.2103ms"
    },
    {
        "timestamp": "2025-01-03T16:31:12.8825031+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T16:31:12.876472+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 1063.3749ms"
    },
    {
        "timestamp": "2025-01-03T16:31:12.8118369+01:00",
        "level": "Information",
        "message": "Executing \"OkObjectResult\", writing value of type '\"System.Linq.Enumerable+SelectArrayIterator`2[[Nest.IHit`1[[DockerHubBackend.Dto.Response.LogDto, DockerHubBackend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], Nest, Version=7.0.0.0, Culture=neutral, PublicKeyToken=96c599bbe3e70f5d],[DockerHubBackend.Dto.Response.LogDto, DockerHubBackend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]\"'."
    },
    {
        "timestamp": "2025-01-03T16:31:11.7925361+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    },
    {
        "timestamp": "2025-01-03T16:31:11.7165559+01:00",
        "level": "Information",
        "message": "Executing endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T16:31:11.5815276+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:27:56.184043+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - 200 null \"application/json;charset=utf-8\" 159.4601ms"
    },
    {
        "timestamp": "2025-01-03T16:27:56.0246043+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:27:55.8066104+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - 200 null \"text/javascript; charset=UTF-8\" 71.3981ms"
    },
    {
        "timestamp": "2025-01-03T16:27:55.7388162+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - 200 13728 \"application/javascript; charset=utf-8\" 23.421ms"
    },
    {
        "timestamp": "2025-01-03T16:27:55.7351221+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:27:55.7164061+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:27:55.7144562+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - 200 null \"text/html;charset=utf-8\" 301.2076ms"
    },
    {
        "timestamp": "2025-01-03T16:27:55.4128379+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:27:54.8958635+01:00",
        "level": "Information",
        "message": "Content root path: \"C:\\Users\\anast\\Desktop\\uks\\UKS-2024\\DockerHubBackend\\DockerHubBackend\""
    },
    {
        "timestamp": "2025-01-03T16:27:54.8924633+01:00",
        "level": "Information",
        "message": "Hosting environment: \"Development\""
    },
    {
        "timestamp": "2025-01-03T16:27:54.8875129+01:00",
        "level": "Information",
        "message": "Application started. Press Ctrl+C to shut down."
    },
    {
        "timestamp": "2025-01-03T16:27:54.8821339+01:00",
        "level": "Information",
        "message": "Now listening on: \"http://localhost:5156\""
    },
    {
        "timestamp": "2025-01-03T16:27:54.725457+01:00",
        "level": "Information",
        "message": "User profile is available. Using '\"C:\\Users\\anast\\AppData\\Local\\ASP.NET\\DataProtection-Keys\"' as key repository and Windows DPAPI to encrypt keys at rest."
    },
    {
        "timestamp": "2025-01-03T16:25:40.4442338+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 400 null \"text/plain; charset=utf-8\" 868.3844ms"
    },
    {
        "timestamp": "2025-01-03T16:25:40.4395642+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T16:25:40.4360813+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 644.5862ms"
    },
    {
        "timestamp": "2025-01-03T16:25:40.4201037+01:00",
        "level": "Information",
        "message": "Executing \"BadRequestObjectResult\", writing value of type '\"System.String\"'."
    },
    {
        "timestamp": "2025-01-03T16:25:39.7753292+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    },
    {
        "timestamp": "2025-01-03T16:25:39.7150346+01:00",
        "level": "Information",
        "message": "Executing endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T16:25:39.5759156+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:56.8530632+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - 200 null \"application/json;charset=utf-8\" 152.4241ms"
    },
    {
        "timestamp": "2025-01-03T16:24:56.7005113+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:56.4662074+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - 200 null \"text/javascript; charset=UTF-8\" 65.996ms"
    },
    {
        "timestamp": "2025-01-03T16:24:56.4127117+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - 200 13728 \"application/javascript; charset=utf-8\" 12.2444ms"
    },
    {
        "timestamp": "2025-01-03T16:24:56.4012507+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:56.4012694+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:56.3681456+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - 200 null \"text/html;charset=utf-8\" 241.1774ms"
    },
    {
        "timestamp": "2025-01-03T16:24:56.127613+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:55.8350963+01:00",
        "level": "Information",
        "message": "Content root path: \"C:\\Users\\anast\\Desktop\\uks\\UKS-2024\\DockerHubBackend\\DockerHubBackend\""
    },
    {
        "timestamp": "2025-01-03T16:24:55.8334208+01:00",
        "level": "Information",
        "message": "Hosting environment: \"Development\""
    },
    {
        "timestamp": "2025-01-03T16:24:55.8305259+01:00",
        "level": "Information",
        "message": "Application started. Press Ctrl+C to shut down."
    },
    {
        "timestamp": "2025-01-03T16:24:55.8230821+01:00",
        "level": "Information",
        "message": "Now listening on: \"http://localhost:5156\""
    },
    {
        "timestamp": "2025-01-03T16:24:55.5565093+01:00",
        "level": "Information",
        "message": "User profile is available. Using '\"C:\\Users\\anast\\AppData\\Local\\ASP.NET\\DataProtection-Keys\"' as key repository and Windows DPAPI to encrypt keys at rest."
    },
    {
        "timestamp": "2025-01-03T16:24:04.8839483+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - 200 null \"application/json;charset=utf-8\" 31.0727ms"
    },
    {
        "timestamp": "2025-01-03T16:24:04.8528236+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:04.5959268+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - 200 null \"text/javascript; charset=UTF-8\" 24.7955ms"
    },
    {
        "timestamp": "2025-01-03T16:24:04.5803347+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - 200 13728 \"application/javascript; charset=utf-8\" 9.2941ms"
    },
    {
        "timestamp": "2025-01-03T16:24:04.5710019+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:04.5709909+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:24:04.5530627+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - 200 null \"text/html;charset=utf-8\" 18.3139ms"
    },
    {
        "timestamp": "2025-01-03T16:24:04.534757+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:21:00.591161+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - 200 null \"application/json;charset=utf-8\" 163.1823ms"
    },
    {
        "timestamp": "2025-01-03T16:21:00.4279761+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/v1/swagger.json\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:20:59.4891327+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - 200 null \"text/javascript; charset=UTF-8\" 84.2612ms"
    },
    {
        "timestamp": "2025-01-03T16:20:59.4288291+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - 200 13728 \"application/javascript; charset=utf-8\" 22.2034ms"
    },
    {
        "timestamp": "2025-01-03T16:20:59.4062904+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_vs/browserLink\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:20:59.4063987+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/_framework/aspnetcore-browser-refresh.js\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:20:59.2673861+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - 200 null \"text/html;charset=utf-8\" 232.6706ms"
    },
    {
        "timestamp": "2025-01-03T16:20:59.0374685+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/swagger/index.html\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T16:20:58.700639+01:00",
        "level": "Information",
        "message": "Content root path: \"C:\\Users\\anast\\Desktop\\uks\\UKS-2024\\DockerHubBackend\\DockerHubBackend\""
    },
    {
        "timestamp": "2025-01-03T16:20:58.6974748+01:00",
        "level": "Information",
        "message": "Hosting environment: \"Development\""
    },
    {
        "timestamp": "2025-01-03T16:20:58.6945549+01:00",
        "level": "Information",
        "message": "Application started. Press Ctrl+C to shut down."
    },
    {
        "timestamp": "2025-01-03T16:20:58.6888238+01:00",
        "level": "Information",
        "message": "Now listening on: \"http://localhost:5156\""
    },
    {
        "timestamp": "2025-01-03T16:20:58.3519135+01:00",
        "level": "Information",
        "message": "User profile is available. Using '\"C:\\Users\\anast\\AppData\\Local\\ASP.NET\\DataProtection-Keys\"' as key repository and Windows DPAPI to encrypt keys at rest."
    },
    {
        "timestamp": "2025-01-03T15:56:46.0192621+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 400 null \"text/plain; charset=utf-8\" 42.1866ms"
    },
    {
        "timestamp": "2025-01-03T15:56:46.0163229+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:46.0133913+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 21.1481ms"
    },
    {
        "timestamp": "2025-01-03T15:56:46.0079543+01:00",
        "level": "Information",
        "message": "Executing \"BadRequestObjectResult\", writing value of type '\"System.String\"'."
    },
    {
        "timestamp": "2025-01-03T15:56:45.9882687+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    },
    {
        "timestamp": "2025-01-03T15:56:45.9847043+01:00",
        "level": "Information",
        "message": "Executing endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:45.9770561+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T15:56:24.8040081+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 400 null \"text/plain; charset=utf-8\" 63.0833ms"
    },
    {
        "timestamp": "2025-01-03T15:56:24.80125+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:24.7975632+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 22.1708ms"
    },
    {
        "timestamp": "2025-01-03T15:56:24.7920173+01:00",
        "level": "Information",
        "message": "Executing \"BadRequestObjectResult\", writing value of type '\"System.String\"'."
    },
    {
        "timestamp": "2025-01-03T15:56:24.7669843+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    },
    {
        "timestamp": "2025-01-03T15:56:24.761887+01:00",
        "level": "Information",
        "message": "Executing endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:24.7409016+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T15:56:21.3927397+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 400 null \"text/plain; charset=utf-8\" 72.0677ms"
    },
    {
        "timestamp": "2025-01-03T15:56:21.3883916+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:21.3827357+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 25.8764ms"
    },
    {
        "timestamp": "2025-01-03T15:56:21.3760856+01:00",
        "level": "Information",
        "message": "Executing \"BadRequestObjectResult\", writing value of type '\"System.String\"'."
    },
    {
        "timestamp": "2025-01-03T15:56:21.3488647+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    },
    {
        "timestamp": "2025-01-03T15:56:21.3428192+01:00",
        "level": "Information",
        "message": "Executing endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:21.3206738+01:00",
        "level": "Information",
        "message": "Request starting \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - null null"
    },
    {
        "timestamp": "2025-01-03T15:56:16.3216805+01:00",
        "level": "Information",
        "message": "Request finished \"HTTP/1.1\" \"GET\" \"http\"://\"localhost:5156\"\"\"\"/api/log/information\"\"\" - 400 null \"text/plain; charset=utf-8\" 855.9713ms"
    },
    {
        "timestamp": "2025-01-03T15:56:16.3163538+01:00",
        "level": "Information",
        "message": "Executed endpoint '\"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\"'"
    },
    {
        "timestamp": "2025-01-03T15:56:16.3103056+01:00",
        "level": "Information",
        "message": "Executed action \"DockerHubBackend.Controllers.LogSearchController.GetInformationLogs (DockerHubBackend)\" in 643.8828ms"
    },
    {
        "timestamp": "2025-01-03T15:56:16.291285+01:00",
        "level": "Information",
        "message": "Executing \"BadRequestObjectResult\", writing value of type '\"System.String\"'."
    },
    {
        "timestamp": "2025-01-03T15:56:15.6497552+01:00",
        "level": "Information",
        "message": "Route matched with \"{action = \\\"GetInformationLogs\\\", controller = \\\"LogSearch\\\"}\". Executing controller action with signature \"Microsoft.AspNetCore.Mvc.IActionResult GetInformationLogs()\" on controller \"DockerHubBackend.Controllers.LogSearchController\" (\"DockerHubBackend\")."
    }
];   // Rezultati pretrage
  displayedColumns: string[] = ['timestamp', 'level', 'message']; // Kolone u tabeli

  constructor() {}

  // Funkcija koja se poziva kada administrator pošalje upit
  onSubmit() {
    // API poziv za pretragu logova
    // this.http.post('http://localhost:3000/api/logs/search', { query: this.query })
    //   .subscribe(response => {
    //     this.logs = response;  // Čuvanje rezultata u logs
    //   }, error => {
    //     console.error('Došlo je do greške:', error);
    //   });
  }
}
