using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.Transportation.UX.Contracts.Route.DataContracts;
using Imi.SupplyChain.Transportation.UX.Views.Route;
using Imi.SupplyChain.UX.Infrastructure.Services; // new

namespace Imi.SupplyChain.Transportation.UX.Views.RouteMap
{
    public class RouteMapViewToRouteServiceTranslator
    {
        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }

        public FindRouteNodesParameters TranslateFromViewToService(RouteMapViewParameters viewParameters)
        {
            FindRouteNodesParameters serviceParameters = new FindRouteNodesParameters();

            serviceParameters.RouteId = viewParameters.RouteId;
            serviceParameters.NodeId = viewParameters.NodeId;
            serviceParameters.LanguageCode = UserSessionService.LanguageCode;

            return serviceParameters;
        }

        public IEnumerable<RouteMapViewResult> TranslateFromServiceToView(FindRouteNodesResultCollection serviceResultCollection)
        {
            ICollection<RouteMapViewResult> viewResultCollection = new List<RouteMapViewResult>();

            foreach (FindRouteNodesResult serviceResult in serviceResultCollection)
            {
                RouteMapViewResult viewResult = new RouteMapViewResult();

                viewResult.NodeId = serviceResult.NodeId;
                viewResult.RouteId = serviceResult.RouteId;
                viewResult.SequenceNumber = serviceResult.SequenceNumber;
                viewResult.ToNodeId = serviceResult.ToNodeId;
                viewResult.StopSequenceNumber = serviceResult.StopSequenceNumber;
                viewResult.TypeOfStop = serviceResult.TypeOfStop;
                viewResult.DrivingTime = serviceResult.DrivingTime;
                viewResult.StopTime = serviceResult.StopTime;
                viewResult.Instructions1 = serviceResult.Instructions1;
                viewResult.DrivingDistanceFromPreviousNode = serviceResult.DrivingDistanceFromPreviousNode;
                viewResult.EarliestArrivalTime = serviceResult.EarliestArrivalTime;
                viewResult.LatestArrivalTime = serviceResult.LatestArrivalTime;
                viewResult.Instructions2 = serviceResult.Instructions2;
                viewResult.WeekInterval = serviceResult.WeekInterval;
                viewResult.StartWeek = serviceResult.StartWeek;
                viewResult.NodeDescription = serviceResult.NodeDescription;
                viewResult.TypeOfStopText = serviceResult.TypeOfStopText;
                viewResult.Latitude = serviceResult.Latitude;
                viewResult.Longitude = serviceResult.Longitude;

                viewResult.IsModified = false;
                viewResultCollection.Add(viewResult);
            }

            return viewResultCollection;
        }
    }
}	