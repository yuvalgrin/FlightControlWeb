import React, {useEffect, useState} from 'react';
import { format } from 'date-fns'
import './App.css';
import './bootstrap.min.css';
import FlightMap from "./FlightMap";
import FlightsTable from "./FlightsTable";
import FlightDetails from "./FlightDetails";
import {getAndUpdate} from './utils/RequestUtil';
import alertPng from './resources/alert.png';
import { Container, Toast } from 'react-bootstrap';

const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights?relative_to=";
const flightPlanApi = "flightplan/";


function App() {
    const [flightsList, setFlightList] = useState([]);
    const [flightClicked, setFlightClicked] = useState([]);
    const [flightClickedPlan, setFlightClickedPlan] = useState();
    const [errorAlert, setErrorAlert] = useState();

    const onErrorAlert = (error) => {
        setErrorAlert(error);
        setTimeout(() => {
            setErrorAlert(undefined);
        }, 5000);
    }

    // const [isFlightListLoaded, setIsFlightListLoaded] = useState(false);

    const onFlightClick = (flight) => {
        setFlightClicked(flight);
        getFlightPlan(flight.flight_id);
    }

    const getFlightList = () =>  {
        let now = new Date();
        let utc = new Date(now.getTime() + now.getTimezoneOffset() * 60000);
        let dateUtc = format(utc, 'yyyy-MM-dd HH:mm:ss');
        let url = urlPrefix + flightsApi + dateUtc;
        getAndUpdate(url, setFlightList, undefined, onErrorAlert);
    }

    const getFlightPlan = (id) => {
        let url = urlPrefix + flightPlanApi + id;
        getAndUpdate(url, setFlightClickedPlan, undefined, onErrorAlert);
    }

    const getError = () => {
        return (<div className={'alert'}>
            <Toast>
                <Toast.Header>
                    <img src={alertPng} alt=''/>
                    <strong>Oops...</strong>
                    <small className={'alert-time'}>0 mins ago</small>
                </Toast.Header>
                <Toast.Body>{errorAlert}</Toast.Body>
            </Toast>

        {/*    <Alert key={'alert'} variant={'danger'}>*/}
        {/*    <Alert.Heading>Oops</Alert.Heading>*/}
        {/*    {errorAlert}*/}
        {/*</Alert>*/}
        </div>);
    }

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
            <div className={'flight-table'} color={'black'}>
                <FlightsTable flightsList={flightsList} flightClicked={flightClicked} setFlightClick={onFlightClick}
                              setErrorAlert={onErrorAlert}/>
            </div>
        </Container>
        </div>
  );
}

export default App;
