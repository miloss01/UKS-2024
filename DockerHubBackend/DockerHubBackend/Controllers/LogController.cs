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

        public LogSearchController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpPost("search")]
        public IActionResult SearchLogs([FromBody] LogSearchDto request)
        {
            try
            {
                bool noCriteriaProvided = (string.IsNullOrWhiteSpace(request.Query) &&
                                           !request.StartDate.HasValue &&
                                           !request.EndDate.HasValue);

                QueryContainer query = new QueryContainer();

                // no critera
                if (noCriteriaProvided)
                {
                    var defaultResponse = _elasticClient.Search<LogDto>(s => s
                        .Sort(sort => sort.Descending(f => f.Timestamp))
                        .Size(100)
                    );

                    if (!defaultResponse.IsValid)
                    {
                        return BadRequest(defaultResponse.DebugInformation);
                    }

                    var defaultLogs = defaultResponse.Hits.Select(hit => hit.Source).ToList();
                    return Ok(defaultLogs);
                }

                // 1. query
                if (!string.IsNullOrWhiteSpace(request.Query))
                {
                    query &= new QueryContainerDescriptor<LogDto>()
                        .QueryString(q => q.Query(request.Query));
                }

                // 2. date and time filter
                if (request.StartDate.HasValue || request.EndDate.HasValue)
                {
                    query &= new QueryContainerDescriptor<LogDto>()
                        .DateRange(dr => dr
                            .Field(f => f.Timestamp)
                            .GreaterThanOrEquals(request.StartDate)
                            .LessThanOrEquals(request.EndDate));
                }

                // send quesry to ElasticSearch
                var response = _elasticClient.Search<LogDto>(s => s
                    .Query(q => query)
                    .Sort(sort => sort.Descending(f => f.Timestamp))
                    .Size(100)
                );

                if (!response.IsValid)
                {
                    return BadRequest(response.DebugInformation);
                }

                var logs = response.Hits.Select(hit => hit.Source).ToList();
                return Ok(logs);
            }
            catch (Exception ex)
            {
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
