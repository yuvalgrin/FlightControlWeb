import * as React from 'react';
import Table from 'react-bootstrap/Table';

const FlightDetails = ({flightClicked,
                       flightsList}) => {
    let updatedFlight;

    /** Map all wanted rows */
    const createRows = (flight) => Object.entries(flight).map(([key, value]) => getRow(key, value))

    /** Create the detail row */
    const getRow = (key, value) => {
        return (
            <tr key={key}>
                <td width='100'>{key}</td>
                <td>{value.toString()}</td>
            </tr>)
    }

    /** Get the updated flight object */
    const getFlight = () => Object.values(flightsList).map(value =>
        {if (flightClicked && flightClicked.flight_id === value.flight_id)
            updatedFlight = value; return '';})


    /** Creates the details table */
    const getTable = () => {
        if (flightClicked && flightClicked.length !== 0) {
            getFlight();
            if (updatedFlight && updatedFlight !== undefined && updatedFlight.length !== 0) {
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
    }

    return (<div>
        {getTable()}
        </div>
    );
};

export default FlightDetails;
