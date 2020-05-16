import React, {useEffect, useState} from 'react';
import { format } from 'date-fns'
import './App.css';
import './bootstrap.min.css';
import FlightMap from "./FlightMap";
import FlightsTable from "./FlightsTable";
import FlightDetails from "./FlightDetails";
import {getAndUpdate} from './utils/RequestUtil';
import { Container } from 'react-bootstrap';

const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights?relative_to=";
const flightPlanApi = "flightplan/";


function App() {
    const [flightsList, setFlightList] = useState([]);
    const [flightClicked, setFlightClicked] = useState([]);
    const [flightClickedPlan, setFlightClickedPlan] = useState(undefined);
    // const [isFlightListLoaded, setIsFlightListLoaded] = useState(false);

    const onFlightClick = (flight) => {
        setFlightClicked(flight);
        getFlightPlan(flight.flight_id);
    }

    const getFlightList = () =>  {
        let date = format(new Date(), 'yyyy-MM-dd_HH:mm:ss');
        let url = urlPrefix + flightsApi + date;
        getAndUpdate(url, setFlightList, undefined, undefined);
    }

    const getFlightPlan = (id) => {
        let url = urlPrefix + flightPlanApi + id;
        getAndUpdate(url, setFlightClickedPlan, undefined, undefined);
    }

    useEffect(() => {
        getFlightList();
        // setInterval(getFlightList(), 1000);
    }, [])

    return (
        <div className={'body'}>
        <Container className={'grid-container'}>
            <div className={'header'}>
                <h1>Flight Simulator Web</h1>
            </div>
            <div className={'map'}>
                <FlightMap flightsList={flightsList} flightClicked={flightClicked} setFlightClick={onFlightClick}
                           flightClickedPlan={flightClickedPlan}/>
            </div>
            <div className={'details'}>
                <FlightDetails flightClicked={flightClicked}/>
            </div>
            <div className={'flight-table'} color={'black'}>
                <FlightsTable flightsList={flightsList} flightClicked={flightClicked} setFlightClick={onFlightClick}/>
            </div>
        </Container>
        </div>
  );
}

export default App;
