using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using APISearchInJson.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace APISearchInJson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // GET api/Search
        [HttpGet]
        public ContentResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><body><h1>Hello team!!!</h1></body></html>"
            };
        }

        // GET api/Search/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/Search
        [HttpPost]
        public JsonResult Post([FromBody] Search search)
        {
            int fileCount = 0;
            var sw = Stopwatch.StartNew();

            Result_Data result_Data;
            Result_List result_List = new Result_List();
            List<Result_Data> results = new List<Result_Data>();
            List<string> listFile = new List<string>();

            int amountFind = 0;

            var files = Directory.EnumerateFiles(search.Root, "*.json", SearchOption.AllDirectories);
           
            Parallel.ForEach(files, (file) =>
            {
               
                StreamReader sr = new StreamReader(file);
                string json = sr.ReadToEnd();
                try
                {
                    JObject jo = JObject.Parse(json);
                    foreach (var searchContent in search.Contents)
                    {
                        string s = "$.." + searchContent.NameField;
                        IEnumerable<JToken> findFilds = jo.SelectTokens(s);

                       Parallel.ForEach(findFilds, (contentField) =>
                           {
                            if (searchContent.ContentField == contentField.ToString())
                            {
                                result_Data = new Result_Data() { Path = file, Field = searchContent.NameField, Value = contentField.ToString() };
                                results.Add(result_Data);
                                amountFind++;
                            }
                        });
                    }
                }
                catch (Exception)
                {

                }

                 try
                {
                    JArray jj = JArray.Parse(json);
                    foreach (var searchContent in search.Contents)
                    {
                        string s = "$.." + searchContent.NameField;
                        IEnumerable<JToken> findFilds = jj.SelectTokens(s);
                        Parallel.ForEach(findFilds, (contentField) =>
                        {
                            if (searchContent.ContentField == contentField.ToString())
                            {
                                result_Data = new Result_Data() { Path = file, Field = searchContent.NameField, Value = contentField.ToString() };
                                results.Add(result_Data);
                                amountFind++;
                            }
                        });
                    }
                }
                catch (Exception)
                {

                }

                fileCount++;
            });
            Console.WriteLine("Processed {0} files in {1} milliseconds", fileCount, sw.ElapsedMilliseconds);

            return new JsonResult(results);
        }

        // PUT api/Search/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/Search/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
