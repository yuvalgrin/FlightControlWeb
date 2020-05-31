import * as React from 'react';
import Table from 'react-bootstrap/Table';
import buttonPng from './resources/button.png';
import {useDropzone} from 'react-dropzone'
import {deleteReq, postReq} from './utils/RequestUtil';
import './FlightsTable.css';
import fileuploadPng from './resources/file-upload.png';

import {useCallback, useState} from "react";

const urlPrefix = window.SERVER_URL + "/api/";
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
            return flightsList.map(item => {return createRow(item)})
    }

    /** Get only local flights */
    const getServersFlights = () => {
        let serverFlights = [];
        if (flightsList)
            serverFlights = flightsList.map(item => {return createExternalRow(item)})

        if (serverFlights.length >= 1 && serverFlights[0] !== undefined)
            return [createTextRow('Remote servers', false)].concat(serverFlights)
    }

    /** X only for local flights */
    const getDeleteButton = (flight) => {
        return <button className={'link-button'} onClick={() =>
            deleteReq(urlPrefix + flightsApi + flight.flight_id, setErrorAlert)}>
            <img alt={'delete flight'} src={buttonPng}/>
            </button>
    }

    /** Create a table row with company name - flight id */
    const createExternalRow = (flight) => {
        if (!flight.is_external)
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
                </td>
            </tr>)
    }


    /** Create a table row with company name - flight id and delete button*/
    const createRow = (flight) => {
        if (flight.is_external)
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

                    {getDeleteButton(flight)}
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
                const binaryStr = reader.result;
                postReq(url, binaryStr, setErrorAlert);
            }
            reader.readAsText(file, 'UTF-8')
        })
        onDragLeave();
    }, [setErrorAlert])

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


    /** Generates row with text only */
    const createTextRow = (text, isNoFlights) => {
        return (
            <tr key={'emptyRow'}>
                <td className={isNoFlights?'no-flights':'remote-flight'}>
                    {text}
                </td>
            </tr>
        )
    }

    /** Generates row with text only */
    const createEmptyTable = () => {
        if (flightsList.length === 0)
            return createTextRow('Drop your flightplan here', true);
    }

    /** Generate the table component
     * First local flights,
     * Afterwards the remote servers flights */
    const getTableComp = (isDragOver) => {
        return (
            <Table className={isDragOver?'dropzone-disabled':''}>
                <tbody>
                {createEmptyTable()}
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
