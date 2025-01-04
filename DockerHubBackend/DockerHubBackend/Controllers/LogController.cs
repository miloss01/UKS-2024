using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Linq;

namespace DockerHubBackend.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogSearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        // Konstruktor za injectovanje ElasticClient-a
        public LogSearchController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpPost("search")]
        public IActionResult SearchLogs([FromBody] LogSearchDto request)
        {
            try
            {
                // Validacija unosa
                if (request == null || (string.IsNullOrWhiteSpace(request.Query) && !request.StartDate.HasValue && !request.EndDate.HasValue))
                {
                    return BadRequest("At least one search criterion (query or date range) is required.");
                }

                // Osnovni QueryContainer za ElasticSearch
                QueryContainer query = new QueryContainer();

                // 1. Tekstualni pretrazivacki upit (ako postoji)
                if (!string.IsNullOrWhiteSpace(request.Query))
                {
                    query &= new QueryContainerDescriptor<LogDto>()
                        .QueryString(q => q.Query(request.Query));
                }

                // 2. Filter po datumu i vremenu (ako postoji)
                if (request.StartDate.HasValue || request.EndDate.HasValue)
                {
                    query &= new QueryContainerDescriptor<LogDto>()
                        .DateRange(dr => dr
                            .Field(f => f.Timestamp)
                            .GreaterThanOrEquals(request.StartDate)
                            .LessThanOrEquals(request.EndDate));
                }

                // Slanje upita ElasticSearch serveru
                var response = _elasticClient.Search<LogDto>(s => s
                    .Query(q => query)
                    .Sort(sort => sort.Descending(f => f.Timestamp)) // Sortiranje po vremenu
                    .Size(100) // Maksimalan broj rezultata
                );

                if (!response.IsValid)
                {
                    return BadRequest(response.DebugInformation);
                }

                // Mapiranje rezultata na DTO
                var logs = response.Hits.Select(hit => hit.Source).ToList();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                // Greska tokom pretrage
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("information")]
        public IActionResult GetInformationLogs()
        {
            try
            {
                // Pretraga logova sa nivoom 'information'
                var response = _elasticClient.Search<LogDto>(s => s
                    .Query(q => q.Term(t => t.Field(f => f.Level).Value("information"))) // Filtrira po nivou
                    .Sort(sort => sort.Descending(f => f.Timestamp)) // Sortira po vremenu
                    .Size(100) // Ogranicava broj rezultata na 100
                );

                if (!response.IsValid)
                {
                    return BadRequest(response.DebugInformation);
                }

                // Vracanje rezultata
                return Ok(response.Hits.Select(hit => hit.Source));
            }
            catch (Exception ex)
            {
                // Ako dodje do greske u procesu
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
