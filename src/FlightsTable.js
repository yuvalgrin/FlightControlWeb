import * as React from 'react';
import { useState } from 'react';
import Table from 'react-bootstrap/Table';
import Mark_flight from './FlightsTable.css';

const FlightsTable = ({
                         flightsList,
                         flightClicked,
                         setFlightClick,
                     }) => {
    const [uploadFileMode, setIsUploadFileMode] = useState(false);

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
            <td>X</td>
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
