using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomatesBuenos.Data;
using TomatesBuenos.Models;

namespace TomatesBuenos.Controllers
{
    public class TVshowsController : Controller
    {
        private readonly TomatesBuenosContext _context;

        public TVshowsController(TomatesBuenosContext context)
        {
            _context = context;
        }

        // GET: TVshows
        public async Task<IActionResult> Index()
        {
              return _context.TVshow != null ? 
                          View(await _context.TVshow.ToListAsync()) :
                          Problem("Entity set 'TomatesBuenosContext.TVshow'  is null.");
        }

        // GET: TVshows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TVshow == null)
            {
                return NotFound();
            }

            var tVshow = await _context.TVshow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tVshow == null)
            {
                return NotFound();
            }

            return View(tVshow);
        }

        // GET: TVshows/Create
        public IActionResult Create()
        {
            return View();
        }

       

        // GET: TVshows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TVshow == null)
            {
                return NotFound();
            }

            var tVshow = await _context.TVshow.FindAsync(id);
            if (tVshow == null)
            {
                return NotFound();
            }
            return View(tVshow);
        }

        // POST: TVshows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Title,ImageURL,AudienceRating,CriticsRating,AvaliablePlatforms,Synopsis,Genre,Premiere,Creators,Starring")] TVshow tVshow)
        {
            if (id != tVshow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tVshow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVshowExists(tVshow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tVshow);
        }

        // GET: TVshows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TVshow == null)
            {
                return NotFound();
            }

            var tVshow = await _context.TVshow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tVshow == null)
            {
                return NotFound();
            }

            return View(tVshow);
        }

        // POST: TVshows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TVshow == null)
            {
                return Problem("Entity set 'TomatesBuenosContext.TVshow'  is null.");
            }
            var tVshow = await _context.TVshow.FindAsync(id);
            if (tVshow != null)
            {
                _context.TVshow.Remove(tVshow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TVshowExists(int id)
        {
          return (_context.TVshow?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles = "ExpertoEnCine")]
        public async Task<IActionResult> Create(string url)
        {
            var TVshow = scraper(url);

            _context.Add(TVshow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public static TVshow scraper(string url)
        {
            //Accediendo a url
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }
            //parseando datos
            TVshow parse_html(string html, string url)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                //title

                var ShowNameCoded = htmlDoc.DocumentNode.SelectSingleNode("//h1[@slot='title']").InnerText;
                var ShowName = WebUtility.HtmlDecode(ShowNameCoded); //HtmlDecode sirve para mantener los caracteres especiales en el titulo, como - &  
                                                                     //IMAGE URL
                string ImageUrl = null;
                try
                {
                    ImageUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@data-qa='poster-image' and @slot='image']")
                                        .GetAttributeValue("src", "");
                    //ImageUrl = htmlDocument.DocumentNode.SelectSingleNode("//img[@data-qa='Watch trailer for " + SerieTitle + "' and @slot='image']")
                    //.GetAttributeValue("src", "");
                }
                catch (NullReferenceException)
                {
                    // La selección del nodo falló, se asigna un valor nulo a ImageUrl
                }

                if (string.IsNullOrEmpty(ImageUrl))
                {
                    // Casos especiales donde el título de la serie tenga caracteres especiales como "&"
                    ImageUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@alt='Watch trailer for " + ShowName.Replace("&", "&amp;") + "' and @slot='image']")
                                        ?.GetAttributeValue("src", "");
                }

                var ScoreBoardSection = htmlDoc.DocumentNode.SelectSingleNode("//score-board");

                //critics rationg
                var TomatometerScore = ScoreBoardSection.GetAttributeValue("tomatometerscore", "");
                //audience rating
                var AudienceScore = ScoreBoardSection.GetAttributeValue("audiencescore", "");

                //avaliable platforms
                var AvaliablePlatforms = htmlDoc.DocumentNode.SelectSingleNode("//section[@id='where-to-watch']");
              
                var WhereToWatch = AvaliablePlatforms?.SelectNodes(".//where-to-watch-meta");

                List<String> platforms = new List<String>();
                if (WhereToWatch != null)
                {
                    foreach (var platform in WhereToWatch)
                    {
                        var platformUrl = platform.GetAttributeValue("href", "");
                        //En el atributo image se encuentra el nombre de la plataforma donde se puede ver el Show
                        var platform_ = platform.SelectSingleNode(".//where-to-watch-bubble").GetAttributeValue("image", "");
                        platforms.Add(platform_);
                    }
                }

                //sinopsis
                var synopsisSection = htmlDoc.DocumentNode.Descendants()
                    .FirstOrDefault(n => n.GetAttributeValue("data-qa", "") == "series-info-description");
                var synopsis = synopsisSection?.InnerText?.Trim();

                //directionteam
                var creatorValue = htmlDoc.DocumentNode.SelectSingleNode("//li[b[@data-qa='series-info-creators']]//span[@class='info-item-value']/a/span");
                var creator = creatorValue?.InnerText.Trim();
                if (creator == null)
                {
                    creator = "No disponible";
                }
                //main actors
                var starringNode = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(., 'Starring: ')]");
                var starringLinks = starringNode?.SelectNodes(".//a");

                // Iterar sobre los elementos <a> y extraer el texto de los elementos <span>
                List<string> actors = new List<string>();
                if (starringLinks != null)
                {
                    foreach (HtmlNode starring in starringLinks)
                    {
                        var span = starring.SelectSingleNode(".//span");
                        string actorName = span.InnerText.Trim();
                        actors.Add(actorName);
                    }
                }

                //plataformas
                var tvNetworkValue = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(., 'TV Network: ')]");
                var tvNetwork = tvNetworkValue?.InnerText.Replace("TV Network: ", "").Trim(); //con el replace hacemos que no se guarde el "TV network"
                                                                                              //y borramos espacios en blanco
                                                                                              //release date
                var premiereDateValue = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(., 'Premiere Date: ')]");
                var premiereDate = premiereDateValue?.InnerText.Replace("Premiere Date: ", "").Trim();

                //genre
                var genreValue = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(., 'Genre: ')]");
                var genre = genreValue?.InnerText.Replace("Genre: ", "").Trim();

                //plataformas
                string platforms_string = "";
                if (platforms.Count != 0)
                {
                    platforms_string = string.Join(", ", platforms);
                }
                else
                {
                    platforms_string = "No disponible";
                }
                string actors_string = "";
                if (actors.Count != 0)
                {
                    actors_string = string.Join("\n", actors);
                }
                else
                {
                    actors_string = "No disponible";
                }

                return new TVshow
                {
                    Title = ShowName,
                    Url = url,
                    ImageURL = ImageUrl,
                    AudienceRating = AudienceScore,
                    CriticsRating = TomatometerScore,
                    AvaliablePlatforms = platforms_string,
                    Synopsis = synopsis,
                    Genre = genre,
                    Premiere = premiereDate,
                    Creators = creator,
                    Starring = actors_string,
                };
            }

            var response = call_url(url).Result;
            TVshow tvshow = parse_html(response, url);
            return tvshow;

        }
        List<String> scrape_link()
        {
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }

            List<String> parse_html(string html)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                List<String> linkseries = new List<String>();
                for (int i = 11; i <= 20; i++)
                {
                    var serieValue = htmlDoc.DocumentNode.SelectSingleNode($"(//span[@class='dynamic-text-list__item-title clamp clamp-1'])[{i}]");
                    var topSerie_ = serieValue?.InnerText.Trim();
                    var TopSerie_ = WebUtility.HtmlDecode(topSerie_);
                    TopSerie_ = Regex.Replace(TopSerie_, @"[ ,]+", "_");

                    var link = "https://www.rottentomatoes.com/tv/" + TopSerie_;

                    linkseries.Add(link);
                }

                return linkseries;
            }

            string url = "https://www.rottentomatoes.com";
            var response = call_url(url).Result;
            List<String> data = parse_html(response);
            return data;
        }
        public async Task <List<TVshow>> ScrapeSeriesFromLinks()
        {
            List<string> links = scrape_link(); // Obtienes la lista de enlaces
            List<TVshow> scrapedSeries = new  List<TVshow>(); // Lista para almacenar las películas raspadas

            foreach (var link in links)
            {
                var show = scraper(link); // Realizas el raspado de la película utilizando el enlace
                scrapedSeries.Add(show); // Agregas la película a la lista

                var topTVshow = new TopTVshow
                {
                    TVshow = show,
                };

                _context.TopTVshow.Add(topTVshow);
                await _context.SaveChangesAsync();
            }

            return scrapedSeries; // Devuelves la lista de películas raspadas
        }
        public async Task<IActionResult> TopShows()
        {
            List<TVshow> topShows = await ScrapeSeriesFromLinks(); // Obtiene las películas del top
            return View("TopShows", topShows); // Pasa las películas a la vista
        }


    }
}
