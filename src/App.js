import React, {useEffect, useState} from 'react';
import { format } from 'date-fns'
import './App.css';
import './bootstrap.min.css';
import FlightMap from "./FlightMap";
import FlightsTable from "./FlightsTable";
import FlightDetails from "./FlightDetails";
import getAndUpdate from './utils/RequestUtil';
import { Container, Row, Col } from 'react-bootstrap';

const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights?relative_to=";


function App() {
    const [flightsList, setFlightList] = useState([]);
    const [flightClicked, setFlightClicked] = useState([]);
    const [isFlightListLoaded, setIsFlightListLoaded] = useState(false);

    const getFlightList = () =>  {
        let date = format(new Date(), 'yyyy-MM-dd_HH:mm:ss');
        let url = urlPrefix + flightsApi + date;
        getAndUpdate(url, setFlightList, setIsFlightListLoaded, undefined);
    }

    useEffect(() => {
        getFlightList();
        // setInterval(getFlightList(), 1000);
    }, [])

    return (
        <div className={'app'}>
            <Container className={'flexContainer'}>
                <div className={'flexColumn1'}>
                    <FlightMap flightsList={flightsList} flightClicked={flightClicked} setFlightClick={setFlightClicked}/>
                    <FlightDetails flightClicked={flightClicked}/>
                </div>
                <div className={'flexColumn2'} color={'black'}>
                    <FlightsTable className={'flightsTable'} flightsList={flightsList} flightClicked={flightClicked} setFlightClick={setFlightClicked}/>
                </div>
            </Container>
        </div>
  );
}

export default App;
