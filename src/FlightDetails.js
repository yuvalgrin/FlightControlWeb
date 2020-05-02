import * as React from 'react';
import { useState } from 'react';
import Table from 'react-bootstrap/Table';
import Mark_flight from './FlightDetails.css';

const FlightDetails = ({flightClicked}) => {
    let items = [];
    if (flightClicked) {
        for (let [key, value] of Object.entries(flightClicked)) {
            items.push(
            <tr key={key}>
                <td>{key}</td>
                <td>{value}</td>
            </tr>
            )
        }
    }

    return (
        <Table striped bordered hover variant="dark">
            <thead>
            <tr>
                <th>Type</th>
                <th>Value</th>
            </tr>
            </thead>
            <tbody>
            {items}
            </tbody>
        </Table>

    );
};

export default FlightDetails;
