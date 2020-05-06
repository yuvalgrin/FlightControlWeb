import * as React from 'react';
import { useState } from 'react';
import Table from 'react-bootstrap/Table';
import Mark_flight from './FlightsTable.css';
import {format} from "date-fns";
import getAndUpdate from "./utils/RequestUtil";

const urlPrefix = "https://localhost:5001/api/";
const flightsApi = "flights/";

const FlightsTable = ({
                         flightsList,
                         flightClicked,
                         setFlightClick
                     }) => {
    const [uploadFileMode, setIsUploadFileMode] = useState(false);

    const deleteFlight = (id) =>  {
        let url = urlPrefix + flightsApi + id;
        fetch(url, {
            method: 'delete'
        })
    }

    let items = [];
    if (flightsList) {
        items = flightsList.map(item => {
            let markFlight = false;
            if (flightClicked && item.flight_id == flightClicked.flight_id){
                markFlight = true;
            }
            return <tr key={item.flight_id}>
                <td onClick={() => setFlightClick(item)}>
                    {/*{markFlight?<p className={Mark_flight}>{item.flight_id}</p>:*/}
                    {markFlight?<b><u>{item.flight_id}</u></b>:
                    item.flight_id}</td>
            <td onClick={() => deleteFlight(item.flight_id)}>X</td>
            </tr>})
    }

    return (
        <Table striped bordered hover variant="dark">
            <thead>
            <tr>
                <th>Flight Id</th>
                <th>Delete</th>
            </tr>
            </thead>
            <tbody>
            {items}
            </tbody>
        </Table>

    );
};

export default FlightsTable;
