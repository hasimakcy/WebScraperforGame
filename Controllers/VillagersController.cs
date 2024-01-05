using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;

// C# ile yazıldığı için CssSelector'u kullanmak için eklenmesi gereken iki paket var
// ilki HtmlAgilityPack kurulumu için https://www.nuget.org/packages/HtmlAgilityPack/ linkinden
// ikincisi CssSelectors'e https://www.nuget.org/packages/HtmlAgilityPack.CssSelectors.NetCore/ linkinden ulaşabilirsiniz


namespace stardew.Controllers;

[ApiController]
[Route("[controller]")]
public class VillagersController : ControllerBase
{



    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult> Get()
    {
        var url = "https://stardewvalleywiki.com/Villagers";

        //Bu kod HtmlAgilityPack'i sayesinde siteye ulaşıyor isteği yapıyor ve nodes listesine isteğimize göre ayıklayarak dolduruyor
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        IList<HtmlNode> nodes = doc.QuerySelectorAll("ul.gallery")[1]
        .QuerySelectorAll("li.gallerybox");

        var data = nodes.Select((node) => {
            var name = node.QuerySelector("div.gallerytext p a").InnerText;

            var imageUrl = node.QuerySelector("div.thumb a img").GetAttributeValue("src", 
               "data:image/png;base64,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");
            return new{
               name = name,
               imageUrl = $"https://stardewvalleywiki.com" + imageUrl,
               descriptionUrl = $"https://stardewvalleywiki.com/" + name
            };
        });


        return Ok(data);
    }
}
