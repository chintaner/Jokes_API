using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Rest;
using Swashbuckle.Swagger.Annotations;
using JokesAPI.Controllers;
using JokesAPI.Models;
using ContactsList.API;

namespace JokesAPI.Controllers
{
    public class HttpOperationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is HttpOperationException)
            {
                var ex = (HttpOperationException)context.Exception;
                context.Response = ex.Response;
            }
        }
    }
}

namespace JokesAPI.Controllers
{
    [HttpOperationExceptionFilter]
    public class JokesController : ApiController
    {
        private const string FILENAME = "jokes.json";
        private GenericStorage _storage;

        public JokesController()
        {
            _storage = new GenericStorage();
        }

        private async Task<IEnumerable<Joke>> GetJokes()
        {
            var jokes = await _storage.Get(FILENAME);

            if (jokes == null)
            {
                jokes = await _storage.Save(new[]{
                        new Joke { Id = 1, Message = "डॉक्टर-रात में टेंशन लेकर नही सोना चाहिये । मरीज – तो क्या मायके भेज दें..? 😉", Category = "DR"}
                }
                , FILENAME);

            }

            var jokesList = jokes.ToList();
            return jokesList;
        }
           
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(IEnumerable<Joke>))]
        public async Task<IEnumerable<Joke>> Get()
        {
            return await GetJokes();
        }

        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(IEnumerable<Joke>))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Joke not found",
            Type = typeof(IEnumerable<Joke>))]
        [SwaggerOperation("GetJokeById")]
        public async Task<Joke> Get([FromUri] int id)
        {
            var jokes = await GetJokes();
            return jokes.FirstOrDefault(x => x.Id == id);
        }

     
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created,
            Description = "Created",
            Type = typeof(Joke))]
        public async Task<Joke> Post([FromBody] Joke joke)
        {
            var jokes = await GetJokes();
            var jokeList = jokes.ToList();

            jokeList.Add(joke);
            await _storage.Save(jokeList, FILENAME);
            return joke;
        }

      
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "Updated",
            Type = typeof(Joke))]
        public async Task<Joke> Put([FromBody] Joke joke)
        {
            await Delete(joke.Id);
            await Post(joke);
            return joke;
        }

      
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(bool))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(bool))]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var jokes = await GetJokes();
            var jokeList = jokes.ToList();

            if (!jokeList.Any(x => x.Id == id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, false);
            }
            jokeList.RemoveAll(x => x.Id == id);
            await _storage.Save(jokeList, FILENAME);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
