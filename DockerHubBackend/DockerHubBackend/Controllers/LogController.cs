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
        public IActionResult SearchLogs([FromBody] LogCriteriaDto criteria)
        {
            try
            {
                // Kreiranje Elasticsearch upita na osnovu kriterijuma
                var query = BuildQuery(criteria);

                // Slanje upita Elasticsearch-u
                var response = _elasticClient.Search<LogDto>(s => s
                    .Query(q => q.Bool(b => b
                        .Must(query)
                    ))
                    .Sort(s => s.Descending(f => f.Timestamp))
                    .Size(100) // Ogranicite broj rezultata
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Pomocna metoda za kreiranje upita na osnovu kriterijuma
        private static QueryContainer BuildQuery(LogCriteriaDto criteria)
        {
            var queryContainer = new QueryContainer();

            // Datum filtriranje (pre i posle datuma)
            if (criteria.StartDate != null && criteria.EndDate != null)
            {
                queryContainer &= new DateRangeQuery
                {
                    Field = Infer.Field<LogDto>(f => f.Timestamp),
                    GreaterThanOrEqualTo = criteria.StartDate,
                    LessThanOrEqualTo = criteria.EndDate
                };
            }

            // Nivo filtriranje (info, warning, error, itd.)
            if (!string.IsNullOrEmpty(criteria.Level))
            {
                queryContainer &= new TermQuery
                {
                    Field = Infer.Field<LogDto>(f => f.Level),
                    Value = criteria.Level
                };
            }

            // Tekstualni sadrzaj pretrage
            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                queryContainer &= new MatchQuery
                {
                    Field = Infer.Field<LogDto>(f => f.Message),
                    Query = criteria.SearchText
                };
            }

            // Ako je korisnik definisao slozeni logicki upit
            if (!string.IsNullOrEmpty(criteria.ComplexQuery))
            {
                // Parsiranje slozenog upita u odgovarajuci Elasticsearch upit
                queryContainer &= new QueryStringQuery
                {
                    Query = criteria.ComplexQuery
                };
            }

            return queryContainer;
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
