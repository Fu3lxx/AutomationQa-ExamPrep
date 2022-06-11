using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;

namespace ContactBook.ApiTests
{
    public class ApiTests
    {
        private RestClient client;
        private const string url = "https://contactbook.fu3lxx.repl.co/api";
        private RestRequest request;
        private Random random;
        [SetUp]
        public void Setup()
        {
            client = new RestClient();
            random = new Random();
        }


        [Test]
        public void Test_GetAllClients_CheckFirstClient()
        {
            this.request = new RestRequest(url + "/contacts");

            var response = this.client.Execute(request);
            var clients = JsonSerializer.Deserialize<List<Contact>>(response.Content);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            Assert.That(clients[0].firstName, Is.EqualTo("Steve"));
            Assert.That(clients[0].lastName, Is.EqualTo("Jobs"));

        }

        [Test]
        public void Test_SearchClientsByKeyword_CheckFirstNameAndLastName()
        {
            var keyword = "albert";
            this.request = new RestRequest(url + "/contacts/search/" + keyword);

            var response = this.client.Execute(request);
            var clients = JsonSerializer.Deserialize<List<Contact>>(response.Content);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clients[0].firstName, Is.EqualTo("Albert"));
            Assert.That(clients[0].lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchClientsByInvalidKeyword_CheckThatResultIsEmpty()
        {
            
            var keyword = "missing" + random.Next(0,1000).ToString();
            this.request = new RestRequest(url + "/contacts/search/" + keyword);

            var response = this.client.Execute(request);
            var clients = JsonSerializer.Deserialize<List<Contact>>(response.Content);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clients.Count, Is.EqualTo(0));
            
        }

        [Test]
        public void Test_CreateClientWithInvalidData_CheckErrorMessage()
        {
            request = new RestRequest(url + "/contacts");
            var body = new
            {
                firstName = "Olivie",
                email = "Shampen@gmail.com",
                phone = "0888888888",
                comments = "nomerEdno"

            };
            request.AddBody(body);

            var response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateValidClient_CheckClientsList()
        {
            request = new RestRequest(url + "/contacts");
            
            var body = new
            {
                firstName = "Olivie" + random.Next(0, 1000),
                lastName = "Shampen" + random.Next(0, 1000),
                email = "Shampen@gmail.com",
                phone = "0888888888",
                comments = "nomerEdno"

            };
            request.AddBody(body);

            var response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            var client = contacts.Last();

            Assert.That(client.firstName, Is.EqualTo(body.firstName));
            Assert.That(client.lastName, Is.EqualTo(body.lastName));

        }
    }
}