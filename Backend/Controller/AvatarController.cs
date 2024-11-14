using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [ApiController]
    [Route("avatar")]
    public class AvatarController : ControllerBase
    {
        private const string lastCharacter6789 ="https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/";
        private const string stringContainsVowel ="https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";
        private const string nonAlphaNumericCharacter ="https://api.dicebear.com/8.x/pixel-art/png";
        private const string defaultAvatar ="https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        char[] vowelsArray = { 'a', 'e', 'i', 'o', 'u'};

        private readonly ILogger<AvatarController> _logger;
        private readonly IImageRepository _imageRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public AvatarController(ILogger<AvatarController> logger,IImageRepository imageRepository,IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _imageRepository = imageRepository;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvatar([Required] string userIdentifier)
        {
            string result=null;
            if(int.TryParse(userIdentifier[^1]+"", out int number) && number!=0)
            {
                _logger.Log(LogLevel.Information,"last character is a non-zero number");
                if (number > 5)
                {
                    //Retrieving from 3rd party server
                    _logger.Log(LogLevel.Information,"Retrieving from 3rd party server");
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync(lastCharacter6789 + number);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Images>(data);
                        if (apiResponse != null)
                            result = apiResponse.url;
                        else
                            return BadRequest(new { error = "Invalid response from the 3rd party server" });
                    }
                    else
                        return StatusCode((int)response.StatusCode, new { error = "Request failed" });
                }
                else{
                    _logger.Log(LogLevel.Information,"Retrieving from Database");
                    //retrieve from db
                    var image = await _imageRepository.GetImage(number);
                    result = image?.url;
                }
            }
            else if(userIdentifier.ToLower().Any(c => vowelsArray.Contains(c)))
                result=stringContainsVowel; //for string conataining vowels
            else if(userIdentifier.Any(c => !char.IsLetterOrDigit(c))){
                //for non-alphanumeric strings
                Random random =new();
                var randomNumber= random.Next(1,5);
                result=$"{nonAlphaNumericCharacter}?seed={randomNumber}&size=150";
            }
            else
                result=defaultAvatar; //default case
            return new JsonResult(new { url = result });
        }
    }
}
