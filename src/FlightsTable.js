import * as React from 'react';
import { useState } from 'react';
import Table from 'react-bootstrap/Table';
import buttonPng from './resources/button.png';
import Dropzone from 'react-dropzone';


import Mark_flight from './FlightsTable.css';
import {format} from "date-fns";
import {deleteReq, getAndUpdate} from './utils/RequestUtil';

const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights/";

const FlightsTable = ({
                         flightsList,
                         flightClicked,
                         setFlightClick
                     }) => {
    const [uploadFileMode, setIsUploadFileMode] = useState(false);

    const getLocalFlights = () => {
        if (flightsList)
            return flightsList.map(item => {return createRow(item, true)})
    }

    const getServersFlights = () => {
        if (flightsList)
            return flightsList.map(item => {return createRow(item, false)})
    }

    const createRow = (flight, localFlights) => {
        if (localFlights && flight.is_external || !localFlights && !flight.is_external)
            return;

        let markFlight = false;
        if (flightClicked && flight.flight_id == flightClicked.flight_id){
            markFlight = true;
        }
        return (
            <tr key={flight.flight_id}>
                <td>
                    {/*{markFlight?<p className={Mark_flight}>{item.flight_id}</p>:*/}
                    <a id='flightId' title='click to watch flight'
                       href='#' onClick={() => setFlightClick(flight)}>{markFlight?
                        <b><u>{flight.flight_id +'  '+ flight.company_name}</u></b>:
                        flight.flight_id +' '+ flight.company_name}</a>

                    <a id='flightId' title='click to watch flight'
                       href='#' onClick={() => deleteReq(flight.flight_id)}>
                        <img src={buttonPng} align={'right'}/>
                    </a>
                </td>
            </tr>)
    }

    return (
        <Dropzone>
            <Table striped bordered hover variant="dark">
                <tbody>
                {getLocalFlights()}
                {getServersFlights()}
                </tbody>
            </Table>
        </Dropzone>

    );
};

export default FlightsTable;
