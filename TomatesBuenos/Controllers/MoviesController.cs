using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomatesBuenos.Data;
using TomatesBuenos.Models;
using HtmlAgilityPack;

namespace TomatesBuenos.Controllers
{
    public class MoviesController : Controller
    {
        private readonly TomatesBuenosContext _context;

        public MoviesController(TomatesBuenosContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
              return _context.Movie != null ? 
                          View(await _context.Movie.ToListAsync()) :
                          Problem("Entity set 'TomatesBuenosContext.Movie'  is null.");
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Title,ImageURL,AudienceRating,CriticsRating,AvaliablePlatfomrms,Synopsis,Clasification,Genre,ReleaseDate,Duration,DirectionTeam,MainActors,AudienceComments,CriticsComments")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'TomatesBuenosContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string url)
        {
            var movie = scraper(url);

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public static Movie scraper(string url)
        {
            //Accediendo a url
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }
            //parseando datos
            Movie parse_html(string html, string url)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var title = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='title']").InnerText;
                var imageUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@alt='Watch trailer for " + title + "' and @slot='image']")
                    .GetAttributeValue("src", ""); //"src" es la URL de la imagen

                var scoreboardSection = htmlDoc.DocumentNode.SelectSingleNode("//score-board");
                var audienceScore = scoreboardSection.GetAttributeValue("audiencescore", "");
                var tomatometerScore = scoreboardSection.GetAttributeValue("tomatometerscore", "");

                var AvaliablePlatforms = htmlDoc.DocumentNode.SelectSingleNode("//section[@id='where-to-watch']");

                List<String> platforms = new List<String>();
                var WhereToWatch = AvaliablePlatforms.SelectNodes(".//where-to-watch-meta"); //El punto "." selecciona el nodo y los "//" match con la seccion
                if (WhereToWatch != null)
                {
                    foreach (var platform in WhereToWatch)
                    {
                        var PlatformName = platform.GetAttributeValue("affiliate", "");
                        platforms.Add(PlatformName);
                    }
                }

                var SynopsisSection = htmlDoc.DocumentNode.Descendants().FirstOrDefault(n => n.GetAttributeValue("data-qa", "") == "movie-info-synopsis");
                var synopsis = SynopsisSection?.InnerText?.Trim();
                var MovieInfoSection = htmlDoc.DocumentNode.SelectSingleNode("//ul[@id='info']");
                var ratingLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Rating:']");
                var ratingValue = ratingLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
                var rating = ratingValue?.InnerText.Trim();

                var genreLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Genre:']");
                var genreValue = genreLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
                var genre = genreValue?.InnerText.Trim();

                var languageLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Original Language:']");
                var languageValue = languageLabel?.ParentNode?.SelectSingleNode(".//span[contains(@data-qa, 'movie-info-item-value')]");
                var language = languageValue?.InnerText.Trim();

                var directorLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Director:']");
                var directorValue = directorLabel?.ParentNode?.SelectSingleNode(".//a[contains(@data-qa, 'movie-info-director')]");
                var director = directorValue?.InnerText.Trim();

                var releaseLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Release Date (Theaters):']");
                var releaseValue = releaseLabel?.ParentNode?.SelectSingleNode(".//time");
                var releaseDate = releaseValue?.Attributes["datetime"]?.Value.Trim();

                var runtimeLabel = MovieInfoSection.SelectSingleNode(".//b[contains(@data-qa, 'movie-info-item-label') and text()='Runtime:']");
                var runtimeValue = runtimeLabel?.ParentNode?.SelectSingleNode(".//time");
                var runtime = runtimeValue?.InnerText.Trim();

                Dictionary<String, String> actorRolList = new Dictionary<String, String>();
                var castSection = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-qa='cast-section']");
                if (castSection != null)
                {
                    //Console.WriteLine("Actores principales:");
                    var castCrewItems = castSection.SelectNodes(".//div[@class='cast-and-crew-item ']");
                    if (castCrewItems != null)
                    {
                        //moreCasts hide
                        foreach (var item in castCrewItems)
                        {
                            String actorName = "";
                            String actorRol = "";
                            var actorImg = item.SelectSingleNode(".//img");
                            if (actorImg != null)
                            {
                                actorName = actorImg.GetAttributeValue("alt", "");//El valor del atributo alt contiene el nombre del actor

                            }
                            var actorRolNode = item.SelectSingleNode(".//p[@class='p--small']");
                            if (actorRolNode != null)
                            {
                                var actorRolEspaciado = actorRolNode.InnerText.Trim();
                                //regex para eliminar espacios innecesarios entre palabras y dejar solo uno
                                actorRol = Regex.Replace(actorRolEspaciado, @"\s+", " ");

                            }
                            actorRolList.Add(actorName, actorRol);
                        }
                    }
                }
                //Console.WriteLine("");
                var criticReview = htmlDoc.DocumentNode.SelectNodes("//review-speech-balloon[@data-qa='critic-review' and @istopcritic= 'true']");

                List<String> criticReviews = new List<String>();

                List<String> audienceReviews = new List<String>();
                if (criticReview != null)
                {
                    for (int i = 0; i < 3 && i < criticReview.Count; i++)
                    {
                        var reviewQuote = criticReview[i]?.GetAttributeValue("reviewquote", "");
                        reviewQuote = WebUtility.HtmlDecode(reviewQuote);
                        reviewQuote = reviewQuote?.Trim();
                        criticReviews.Add(reviewQuote);
                    }
                    var audienceReview = htmlDoc.DocumentNode.SelectNodes("//review-speech-balloon[@data-qa='critic-review' and @istopcritic= 'false']");
                    if (audienceReview != null)
                    {
                        for (int i = 0; i < 3 && i < audienceReview.Count; i++)
                        {
                            var audienceQuote = audienceReview[i]?.GetAttributeValue("reviewquote", "");
                            audienceQuote = WebUtility.HtmlDecode(audienceQuote);
                            audienceReviews.Add(audienceQuote);
                        }
                    }
                }

                string platforms_string = string.Join(", ", platforms);
                string criticReview_string = string.Join("\n" +
                    "", criticReviews);
                string audienceReview_string = string.Join("\n" +
                    " ", audienceReviews);
                string actorRol_string = "";
                foreach (KeyValuePair<string, string> par in actorRolList)
                {
                    actorRol_string += par.Key + " - " + par.Value + "\n";
                }
                actorRol_string = actorRol_string.TrimEnd();

                return new Movie
                {
                    Title = title,
                    Url = url,
                    ImageURL = imageUrl,
                    AudienceRating = audienceScore,
                    CriticsComments = criticReview_string,
                    AvaliablePlatfomrms = platforms_string,
                    Synopsis = synopsis,
                    Clasification = rating,
                    Genre = genre,
                    ReleaseDate = releaseDate,
                    DirectionTeam = director,
                    Duration = runtime,
                    MainActors = actorRol_string,
                    AudienceComments = audienceReview_string,
                    CriticsRating = tomatometerScore,
                };

            }
            var response = call_url(url).Result;
            Movie movie = parse_html(response, url);
            return movie;
        }
        public List<String> scrape_link()
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

                List<String> Links = new List<String>();
                for (int i = 1; i <= 10; i++)
                {
                    //Iteramos sobre cada nodo que cumple con la condicion de ese value de class
                    //Solamente iteraremos 10 veces debido a que en estos 10 primeros nodos está el top de peliculas
                    
                    var linkNode = htmlDoc.DocumentNode.SelectSingleNode($"(//a[@class='dynamic-text-list__tomatometer-group'])[{i}]");
                    var movieLink = linkNode?.GetAttributeValue("href", "");//aqui solamente se almacena "/m/{nombre_pelicula}", asi que falta completar el link
                    var fullLink = "https://www.rottentomatoes.com" + movieLink;
                    Links.Add(fullLink);

                }

                return Links;
            }

            string url = "https://www.rottentomatoes.com";
            var response = call_url(url).Result;
            List<String> data = parse_html(response);
            return data;
        }
        public async Task<List<Movie>> ScrapeMoviesFromLinks()
        {
            List<string> links = scrape_link(); // Obtienes la lista de enlaces
            List<Movie> scrapedMovies = new List<Movie>(); // Lista para almacenar las películas raspadas

            foreach (var link in links)
            {
                var movie = scraper(link); // Realizas el raspado de la película utilizando el enlace
                scrapedMovies.Add(movie); // Agregas la película a la lista

                var topMovie = new TopMovie
                {
                    Movie = movie // Asigna la película al modelo TopMovie
                };
                // Agrega el objeto TopMovie al contexto y guarda los cambios
                _context.TopMovie.Add(topMovie);
                await _context.SaveChangesAsync();

            }

            return scrapedMovies; // Devuelves la lista de películas raspadas
        }
        public async Task<IActionResult> TopMovies()
        {
            List<Movie> topMovies = await ScrapeMoviesFromLinks(); // Obtiene las películas del top
            return View("TopMovies", topMovies); // Pasa las películas a la vista
        }

    }
}
