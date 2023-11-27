using ApiMongo.Data;
using ApiMongo.Data.Collections;
using ApiMongo.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        MongoDb _mongoDB;

        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(MongoDb mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectato = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _infectadosCollection.InsertOne(infectato);
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtuluizarSexo([FromBody] InfectadoDto dto)
        {
            var infectato = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));
            return Ok("Atualizado Com Sucesso");
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] DateTime dataNasc)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dataNasc));
            return Ok("Deletado Com Sucesso");
        }
    }
}
