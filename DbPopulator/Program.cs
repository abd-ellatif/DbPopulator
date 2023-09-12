using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Json;

public class DbPopulator
{
    static async Task Main()
    {
        string filePath = "Articles.json";
        string apiUrl = "https://localhost:7292/api/Catalogue/AjouterArticle";

        try
        {
            string jsonContent = File.ReadAllText(filePath);

            var jsonArray = JsonConvert.DeserializeObject<JArray>(jsonContent);
            foreach (var article in jsonArray)
            {

                dynamic dynamicItem = article as JObject;
                var articleDto = new CompositeArticleDto
                {
                    Article = new ArticleDto
                    {
                        Nom = dynamicItem.name,
                        Description = dynamicItem.description,
                        Marque = dynamicItem.marque,
                        Visible = true,
                        CategorieID = convertId(dynamicItem.categoryId.ToString()) ?? 1
                    },
                    Images = new List<ImageDto>(),
                    Variantes = new List<VarianteDto>()

                };

                foreach(var image in dynamicItem.images)
                {
                    articleDto.Images.Add(new ImageDto
                    {
                        Url = image.url,
                    });
                }
                foreach (var variante in dynamicItem.variantes)
                {
                    articleDto.Variantes.Add(new VarianteDto
                    { 
                        Nom = variante.name,
                        Couleur = variante.color,
                        Reference = variante.reference,
                        Visible = true,
                        Prix = variante.price ?? 0.0d
                    });
                }

                await PostArticle(articleDto, apiUrl);
                //Task.Run(() => PostArticle(articleDto, apiUrl)).Wait();










            }
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }

    }

    public static async Task PostArticle(CompositeArticleDto articleDto,string apiUrl)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {


                string jsonContentToPost = JsonConvert.SerializeObject(articleDto);


                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, new StringContent(jsonContentToPost, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Request successful");
                }
                else
                {
                    Console.WriteLine("Request failed: " + response.ReasonPhrase);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }


    public record ArticleDto
    {
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Marque { get; set; }
        public bool Visible { get; set; }
        public int CategorieID { get; set; }

    }

    public record VarianteDto
    {
        public string Nom { get; set; }
        public string Couleur { get; set; }
        public string Reference { get; set; }
        public bool Visible { get; set; }
        public decimal Prix { get; set; }
    }



    public record ImageDto
    {
        public string Url { get; set; }
    }

    public record CompositeArticleDto
    {
        public ArticleDto Article { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<VarianteDto> Variantes { get; set; }
    }

    static int convertId(string stringId)
    {
        switch(stringId)

        {
            case "08da8f28-426a-4236-8502-e0d2f415597c":
                return 2;
            case "08da8f28-4dcf-46da-8a1b-72fdf287930b":
                return 3;
            case "08da8f28-5b86-400b-8ae1-e585ab1e4994":
                return 4;
            case "08da8f28-6937-4df0-87ca-bdffb9fcc4a5":
                return 5;
            case "08da8f28-7b99-41a4-890c-cadcc2c652e3":  
                return 6;
            case "08daa163-dcdb-4e51-8c7a-def79d939bae":
                return 7;
            case "08daa164-7c33-47bc-81a8-bb8eec475b3e":
                return 8;
            case "08db64e7-2310-4cb1-8759-6d74b10a3900":
                return 9;
            case "08da8f28-743b-4a8d-8bf1-dc65d4bf4083":
                return 10;
            default:
                return 1;
       
        }
    }




}