using System;
using System.Collections.Generic;
using System.Threading;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using FlightControlWeb.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace FlightControlTests
{
    public class TestControllers
    {
        [Fact]
        public void Test_GetFlight()
        {
            // Setup
            Location location = new Location(38.112375, 23.879437, DateTime.UtcNow);

            List<Segment> segments = new List<Segment>();
            segments.Add(new Segment(31.922629, 31.522594, 50)); // Egypt
            segments.Add(new Segment(32.426506, 34.743033, 50)); // Cyprus
            segments.Add(new Segment(26.209199, 35.055211, 50)); // Greece
            FlightPlan flightPlan = new FlightPlan(8, "Company", location, segments);

            var stubRemoteServersConnector = new RemoteServersConnector();
            var stubFlightsManager = new FlightsManager(stubRemoteServersConnector);
            stubFlightsManager.AddFlightPlan(flightPlan);
            var flightsController = new FlightsController(stubFlightsManager);
            Thread.Sleep(1000);

            // Act
            IActionResult getAction = flightsController.GetFlights(DateTime.UtcNow.ToString());

            // Assert
            OkObjectResult okAction = Assert.IsType<OkObjectResult>(getAction);
            string flightsStr = Assert.IsType<string>(okAction.Value);
            var flights = JsonConvert.DeserializeObject(flightsStr);
            Assert.Single((System.Collections.IEnumerable)flights);
        }

        [Fact]
        public void Test_AddFlight()
        {
            // Setup
            Location location = new Location(38.112375, 23.879437, DateTime.UtcNow);

            List<Segment> segments = new List<Segment>();
            segments.Add(new Segment(31.922629, 31.522594, 50)); // Egypt
            segments.Add(new Segment(32.426506, 34.743033, 50)); // Cyprus
            segments.Add(new Segment(26.209199, 35.055211, 50)); // Greece
            FlightPlan flightPlan = new FlightPlan(8, "Company", location, segments);

            var stubRemoteServersConnector = new RemoteServersConnector();
            var mockFlightsManager = new FlightsManager(stubRemoteServersConnector);
            var flightPlanController = new FlightPlanController(mockFlightsManager);

            // Act
            IActionResult addAction = flightPlanController.AddFlightPlan(flightPlan);

            // Assert
            Assert.IsType<OkResult>(addAction);
            Assert.Single(mockFlightsManager.GetRelativeFlights(DateTime.UtcNow, false));
        }

        [Fact]
        public void Test_DeleteFlight()
        {
            // Setup
            Location location = new Location(38.112375, 23.879437, DateTime.UtcNow);

            List<Segment> segments = new List<Segment>();
            segments.Add(new Segment(31.922629, 31.522594, 50)); // Egypt
            segments.Add(new Segment(32.426506, 34.743033, 50)); // Cyprus
            segments.Add(new Segment(26.209199, 35.055211, 50)); // Greece
            FlightPlan flightPlan = new FlightPlan(8, "Company", location, segments);

            var stubRemoteServersConnector = new RemoteServersConnector();
            var mockFlightsManager = new FlightsManager(stubRemoteServersConnector);
            mockFlightsManager.AddFlightPlan(flightPlan);
            List<Flight> flights = mockFlightsManager.GetRelativeFlights(DateTime.UtcNow, false);
            string flightId = flights.ToArray()[0].Flight_Id;
            var flightsController = new FlightsController(mockFlightsManager);

            // Act
            IActionResult delAction = flightsController.DeleteFlight(flightId);

            // Assert
            Assert.IsType<OkResult>(delAction);
            Assert.Empty(mockFlightsManager.GetRelativeFlights(DateTime.UtcNow, false));
        }

        [Fact]
        public void Test_GetServer()
        {
            // Setup
            string serverUrl = "http://test1.com";
            string serverId = "test_server1";
            Server server = new Server(serverId, serverUrl);

            var stubRemoteServersConnector = new RemoteServersConnector();
            stubRemoteServersConnector.AddServer(server);
            var serversController = new ServersController(stubRemoteServersConnector);

            // Act
            IActionResult getAction = serversController.GetServers();


            // Assert
            OkObjectResult okResultGet = Assert.IsType<OkObjectResult>(getAction);
            string serversStr = Assert.IsType<string>(okResultGet.Value);
            var servers = JsonConvert.DeserializeObject(serversStr);
            Assert.Single((System.Collections.ICollection)servers);
        }

        [Fact]
        public void Test_AddServer()
        {
            // Setup
            string serverUrl = "http://test2.com";
            string serverId = "test_server2";
            Server server = new Server(serverId, serverUrl);

            var mockRemoteServersConnector = new RemoteServersConnector();
            var serversController = new ServersController(mockRemoteServersConnector);

            // Act
            IActionResult addAction = serversController.AddServer(server);


            // Assert
            Assert.IsType<OkResult>(addAction);
            Assert.Single(mockRemoteServersConnector.GetAllServers());
        }


        [Fact]
        public void Test_DeleteServer()
        {
            // Setup
            string serverUrl = "http://test3.com";
            string serverId = "test_server3";
            Server server = new Server(serverId, serverUrl);

            var mockRemoteServersConnector = new RemoteServersConnector();
            mockRemoteServersConnector.AddServer(server);
            var serversController = new ServersController(mockRemoteServersConnector);

            // Act
            IActionResult delAction = serversController.DeleteServer(server.ServerId);


            // Assert
            Assert.IsType<OkResult>(delAction);
            Assert.Empty(mockRemoteServersConnector.GetAllServers());
        }
    }
}
