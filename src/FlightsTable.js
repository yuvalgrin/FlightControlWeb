import * as React from 'react';
import Table from 'react-bootstrap/Table';
import buttonPng from './resources/button.png';
import {useDropzone} from 'react-dropzone'
import {deleteReq, postReq} from './utils/RequestUtil';
import './FlightsTable.css';
import fileuploadPng from './resources/file-upload.png';

import {useCallback, useState} from "react";

const urlPrefix = "https://localhost:5001/api/";
const flightPlanApi = "flightplan";
const flightsApi = "flights/";


const FlightsTable = ({
                         flightsList,
                         flightClicked,
                         setFlightClick,
                          setErrorAlert
                     }) => {
    const [isDragOver, setIsDragOver] = useState(false);

    /** Get only local flights */
    const getLocalFlights = () => {
        if (flightsList)
            return flightsList.map(item => {return createRow(item, true)})
    }

    /** Get only local flights */
    const getServersFlights = () => {
        if (flightsList)
            return flightsList.map(item => {return createRow(item, false)})
    }

    /** Create a table row with company name - flight id */
    const createRow = (flight, localFlights) => {
        if ((localFlights && flight.is_external) || (!localFlights && !flight.is_external))
            return;

        let markFlight = false;
        if (flightClicked && flight.flight_id === flightClicked.flight_id){
            markFlight = true;
        }
        return (
            <tr key={flight.flight_id}>
                <td>
                    <button className={markFlight?'mark-flight':'regular-flight'} title='watch flight'
                       onClick={() => setFlightClick(flight)}>
                            {flight.flight_id +'    '+ flight.company_name}
                    </button>

                    <button className={'link-button'} onClick={() =>
                        deleteReq(urlPrefix + flightsApi + flight.flight_id, setErrorAlert)}>
                        <img alt={'delete flight'} src={buttonPng}/>
                    </button>
                </td>
            </tr>)
    }

    /** Actions done when droping a file */
    const onDrop = useCallback((acceptedFiles) => {
        let url = urlPrefix + flightPlanApi;

        acceptedFiles.forEach((file) => {
            const reader = new FileReader()

            reader.onabort = () => setErrorAlert('file reading was aborted')
            reader.onerror = () => setErrorAlert('file reading has failed')
            reader.onload = () => {
                // Send file content to server side as a post request
                const binaryStr = reader.result
                postReq(url, binaryStr, setErrorAlert);
                console.log(binaryStr)
            }
            reader.readAsArrayBuffer(file)
        })
        onDragLeave();
    }, [])


    const onDragEnter = () => {setIsDragOver(true)}
    const onDragLeave = () => {setIsDragOver(false)}

    const {getRootProps, getInputProps} = useDropzone({onDrop,
        onDragEnter,
        onDragLeave,
        noClick: true})

    /** Generates the img and animation for drag-and-drop */
    const getDragOverComp = () => {
        return (
        <img alt={'dragndrop'} className={'drag-drop-img'} src={fileuploadPng}/>
        )
    }

    /** Generate the table component
     * First local flights,
     * Afterwards the remote servers flights */
    const getTableComp = (isDragOver) => {
        return (
            <Table className={isDragOver?'dropzone-disabled':''}>
                <tbody>
                {getLocalFlights()}
                {getServersFlights()}
                </tbody>
            </Table>)
    }

    return (
        <div className={'container'}>
            <div {...getRootProps({className: isDragOver?'dropzone':'dropzone.disabled'})}>
            <input {...getInputProps()} />
                {getTableComp(isDragOver)}
                {isDragOver?getDragOverComp():''}
            </div>
        </div>

    );
};

export default FlightsTable;
