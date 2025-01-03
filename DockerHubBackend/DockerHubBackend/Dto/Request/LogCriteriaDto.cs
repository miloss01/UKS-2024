using System.Text.Json;

namespace DockerHubBackend.Dto.Request
{
    public class LogCriteriaDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Level { get; set; } // Npr. "ERROR", "WARNING"
        public string SearchText { get; set; } // Tekst koji sadrzi frazu
        public string ComplexQuery { get; set; } // Slozeni logicki upit (npr. "(level:ERROR OR level:WARNING) AND message:error")

        public LogCriteriaDto() { }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
