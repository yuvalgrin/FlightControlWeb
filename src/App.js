import React, {useEffect, useState} from 'react';
import { format } from 'date-fns'
import './App.css';
import './bootstrap.min.css';
import FlightMap from "./FlightMap";
import FlightsTable from "./FlightsTable";
import FlightDetails from "./FlightDetails";
import {getAndUpdate} from './utils/RequestUtil';
import alertPng from './resources/alert.png';
import { Container, Alert } from 'react-bootstrap';
import Fade from "react-bootstrap/Fade";
import Collapse from "react-bootstrap/Collapse";


const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights?relative_to=";
const flightPlanApi = "flightplan/";


function App() {
    const [flightsList, setFlightList] = useState([]);
    const [flightClicked, setFlightClicked] = useState([]);
    const [flightClickedPlan, setFlightClickedPlan] = useState();
    const [errorAlert, setErrorAlert] = useState();
    // const [isFlightListLoaded, setIsFlightListLoaded] = useState(false);


    /** Let each error show for 5 secs */
    const onErrorAlert = (error) => {
        if (errorAlert != undefined)
            return

        setErrorAlert(error);
        setTimeout(() => {
            setErrorAlert(undefined);
        }, 5000);
    }

    /** On flight click get its flight plan */
    const onFlightClick = (flight) => {
        setFlightClicked(flight);
        getFlightPlan(flight.flight_id);
    }

    /** Update the flight list with UTC time */
    const getFlightList = () =>  {
        let now = new Date();
        let utc = new Date(now.getTime() + now.getTimezoneOffset() * 60000);
        let dateUtc = format(utc, 'yyyy-MM-dd HH:mm:ss');
        let url = urlPrefix + flightsApi + dateUtc;
        getAndUpdate(url, setFlightList, undefined, onErrorAlert);
    }

    /** Request the flight plan */
    const getFlightPlan = (id) => {
        let url = urlPrefix + flightPlanApi + id;
        getAndUpdate(url, setFlightClickedPlan, undefined, onErrorAlert);
    }


    /** Create error component */
    const getError = () => {
        return (
                <Alert key={'alert'} variant={'danger'}>
                <Alert.Heading><img alt={'alertImg'} src={alertPng}/>Oops</Alert.Heading>
                {errorAlert}
                </Alert>
        );
    }

    /** On application load to screen */
    useEffect(() => {
        getFlightList();
        setInterval(() => getFlightList(), 1000);
    }, [])

    return (
        <div className={'body'}>
            {errorAlert?getError():''}
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
            <div className={'flight-table'}>
                <FlightsTable flightsList={flightsList} flightClicked={flightClicked} setFlightClick={onFlightClick}
                              setErrorAlert={onErrorAlert}/>
            </div>
        </Container>
        </div>
  );
}

export default App;
