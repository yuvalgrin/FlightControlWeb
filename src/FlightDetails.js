import * as React from 'react';
import Table from 'react-bootstrap/Table';

const FlightDetails = ({flightClicked}) => {

    //Create the detail rows
    const createRows = () => Object.entries(flightClicked).map(([key, value]) => getRow(key, value))
    const getRow = (key, value) => {
        return (
            <tr key={key}>
                <td width='100'>{key}</td>
                <td>{value}</td>
            </tr>)
    }

    return (
        <Table striped bordered hover variant="dark">
            <thead>
            </thead>
            <tbody>
            {flightClicked?createRows():''}
            </tbody>
        </Table>

    );
};

export default FlightDetails;
