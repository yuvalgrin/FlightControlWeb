import {Map, InfoWindow, Marker, GoogleApiWrapper} from 'google-maps-react';
import Table from "react-bootstrap/Table";
import * as React from "react";

const apiKey = 'AIzaSyAjngpsKv9PcK9NqXrHi8VdNi_5VI287CM';

export class FlightMap extends React.Component  {

    render() {
        return (
            <Map style={containerStyle}
                           containerStyle={containerStyle} google={this.props.google} zoom={14}>
                <AirplaneIcon/>
            </Map>
        );
    }
}

const AirplaneIcon = ({}) => {
    return (<img width={10} height={10} src={"https://toppng.com/uploads/preview/airplane-icon-11549899707q0qa0ytxst.png"}/>);
}

const style = {
    width: '100%',
    height: '100%'
}

const containerStyle = {
    position: 'relative',
    width: '500px',
    height: '400px'
}

export default GoogleApiWrapper({
    apiKey: (apiKey)
})(FlightMap)