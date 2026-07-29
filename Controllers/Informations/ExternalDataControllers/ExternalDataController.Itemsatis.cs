using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class ExternalDataController : ControllerBase
    {
        // URL: GET /external/itemsatis
        [HttpGet("itemsatis")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemsatisData()
        {
            string url = "https://www.itemsatis.com/p/VarietyShop";
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            try
            {
                var htmlResponse = await httpClient.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlResponse);

                var puanNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'reviewTotal')]/span");
                string magazaPuani = puanNode != null ? puanNode.InnerText.Trim() : "5.0";

                var satisNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='userPointPositive']");
                string toplamSatis = satisNode != null ? satisNode.InnerText.Replace("Başarılı İşlem", "").Trim() : "500+";

                var ilanNodlari = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'item-card')]");
                var cekilenIlanlar = new List<object>();
                if (ilanNodlari != null)
                {
                    int idSayaci = 1;
                    foreach (var node in ilanNodlari.Take(8))
                    {
                        var linkNode = node.SelectSingleNode(".//a[@itemprop='url']");
                        var imgNode = node.SelectSingleNode(".//img[@itemprop='image']");
                        var baslikNode = node.SelectSingleNode(".//div[@itemprop='name']");
                        var fiyatNode = node.SelectSingleNode(".//span[contains(@class, 'font-bold') and contains(@class, 'text-lg')]");

                        cekilenIlanlar.Add(new
                        {
                            id = idSayaci++,
                            title = baslikNode?.InnerText.Trim() ?? "Börü Paket",
                            price = fiyatNode?.InnerText.Trim() ?? "0",
                            image = imgNode?.GetAttributeValue("src", "") ?? "",
                            link = linkNode != null ? "https://www.itemsatis.com" + linkNode.GetAttributeValue("href", "") : "#",
                            category = "İtemsatış"
                        });
                    }
                }

                var yorumNodlari = htmlDoc.DocumentNode.SelectNodes("//div[@class='content' and starts-with(@id, 'comment-content-')]");
                var cekilenYorumlar = new List<object>();
                if (yorumNodlari != null)
                {
                    foreach (var node in yorumNodlari.Take(5))
                    {
                        var yazarNode = node.SelectSingleNode(".//a[@class='author']");
                        var mesajNode = node.SelectSingleNode(".//div[@class='text']");

                        cekilenYorumlar.Add(new
                        {
                            name = yazarNode?.InnerText.Trim() ?? "Gizli Ajan",
                            comment = mesajNode?.InnerText.Trim() ?? "Harika hizmet!",
                            rating = 5
                        });
                    }
                }

                return Ok(new
                {
                    toplamSatis = toplamSatis,
                    magazaPuani = magazaPuani,
                    ilanlar = cekilenIlanlar,
                    yorumlar = cekilenYorumlar
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { hata = "Siber istihbarat çekilemedi: " + ex.Message });
            }
        }
    }
}