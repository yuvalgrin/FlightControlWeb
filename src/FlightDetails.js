import * as React from 'react';
import Table from 'react-bootstrap/Table';

const FlightDetails = ({flightClicked,
                       flightsList}) => {
    let updatedFlight;
    //Create the detail rows
    const createRows = (flight) => Object.entries(flight).map(([key, value]) => getRow(key, value))

    const getRow = (key, value) => {
        return (
            <tr key={key}>
                <td width='100'>{key}</td>
                <td>{value.toString()}</td>
            </tr>)
    }


    const getFlight = () => Object.values(flightsList).map(value =>
        {if (flightClicked && flightClicked.flight_id === value.flight_id)
            updatedFlight = value; return;})


    const getTable = () => {
        if (flightClicked && flightClicked.length !== 0) {
            getFlight();
            return (
                <Table striped bordered hover variant="dark">
                <thead>
                </thead>
                <tbody>
                {createRows(updatedFlight)}
                </tbody>
            </Table>)
        }
    }

    return (<div>
        {getTable()}
        </div>
    );
};

export default FlightDetails;
