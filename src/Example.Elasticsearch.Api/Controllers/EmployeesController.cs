using Example.Elasticsearch.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Elasticsearch.Api.Controllers
{
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _elasticClient;

        public EmployeesController(IConfiguration configuration, IElasticClient elasticClient)
        {            
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // Query by .Term()
            //var esResponse = await _elasticClient.SearchAsync<Employee>(s =>
            //                                s.Index(_configuration["Elasticsearch:EmployeesIndexName"])
            //                                .Query(q => q.Term(t => t.Id, id)));

            // Query by .Match()
            var esResponse = await _elasticClient.SearchAsync<Employee>(s =>
                                            s.Index(_configuration["Elasticsearch:EmployeesIndexName"])
                                            .Query(q => q.Match(m => m.Field(f => f.Id).Query(id.ToString()))
                                            ));

            var employee = esResponse?.Documents?.FirstOrDefault();

            return employee == null ? NotFound() : new JsonResult(employee);
        }

        [HttpGet("esid/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var esResponse = await _elasticClient.GetAsync<Employee>(new DocumentPath<Employee>(new Id(id)),
                x => x.Index(_configuration["Elasticsearch:EmployeesIndexName"]));

            var employee = esResponse?.Source;

            return employee == null ? NotFound() : new JsonResult(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            var esResponse = await _elasticClient.IndexAsync<Employee>(employee, 
                x => x.Index(_configuration["Elasticsearch:EmployeesIndexName"]));

            return esResponse?.Id == null ? StatusCode(500) : Ok(esResponse.Id);
        }
    }
}
