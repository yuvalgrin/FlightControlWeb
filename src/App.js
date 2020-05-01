import React, {useEffect, useState} from 'react';
import { format } from 'date-fns'
import './App.css';
import './bootstrap.min.css';
import FlightsTable from "./FlightsTable";
import getAndUpdate from './utils/RequestUtil';

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
    <div className="App">
      <header className="Flight-Control-Web">
      <FlightsTable flightsList={flightsList} flightClicked={flightClicked} setFlightClick={setFlightClicked}/>
      </header>
    </div>
  );
}

export default App;
