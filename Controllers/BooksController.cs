using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsumeBooksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsumeBooksAPI.Controllers {

    public class BooksController : Controller {
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<Book> Books { get; set; }

        const string BASE_URL = "https://localhost:5005/";

        public BooksController(IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/Books");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                Books = await JsonSerializer.DeserializeAsync<IEnumerable<Book>> (responseStream);
            } else {    
                Books = Array.Empty<Book> ();
            }

            return View (Books);
        }

        public IActionResult Create () {
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind ("id, title, author, description")] Book book) {
            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (book), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Post;
                message.RequestUri = new Uri ($"{BASE_URL}api/books");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync (message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (book);
        }

        public async Task<IActionResult> Details (string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/books/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Book book = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                book = await JsonSerializer.DeserializeAsync<Book> (responseStream);
            }

            if (book == null)
                return NotFound ();

            return View (book);

        }

        public async Task<IActionResult> Delete (string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/books/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Book book = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                book = await JsonSerializer.DeserializeAsync<Book> (responseStream);
            }

            if (book == null)
                return NotFound ();

            return View (book);

        }

        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id) {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Delete;
            message.RequestUri = new Uri ($"{BASE_URL}api/books/{id}");

            HttpClient client = _clientFactory.CreateClient ();
            HttpResponseMessage response = await client.SendAsync (message);

            var result = await response.Content.ReadAsStringAsync ();

            return RedirectToAction (nameof (Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/books/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Book book = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                book = await JsonSerializer.DeserializeAsync<Book> (responseStream);
            } 

            if (book == null)
                return NotFound ();

            return View (book);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind ("id,title,author,description")] Book book) {

            //int bookId = Convert.ToInt32(id);

            //if (bookId > 0)
            //    return NotFound ();

            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (book), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Put;
                message.RequestUri = new Uri ($"{BASE_URL}api/book/{book.id}");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync(message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (book);
        }
    }

}