import {Map, Polyline, Marker, GoogleApiWrapper} from 'google-maps-react';
import planePng from './resources/regular-airplane-24.png';
import markedPlanePng from './resources/marked-airplane-27.png';
import * as React from "react";

const apiKey = 'AIzaSyAjngpsKv9PcK9NqXrHi8VdNi_5VI287CM';


export class FlightMap extends React.Component  {
    /** This method will iterate all of the flight plans and print them into the Map object */
    createMarkers = () => Object.values(this.props.flightsList).map(value => this.getMarker(value))

    /** Get the markers from flights list */
    getMarker = (flight)  => {
        let isClicked = false;
        if (this.props.flightClicked === flight)
            isClicked = true;
        return <Marker key={flight.flight_id} onClick={() => this.props.setFlightClick(flight)}
                        position = {{lat: flight.latitude, lng: flight.longitude}}
                        icon={{
                            url: isClicked ? markedPlanePng : planePng,
                            scaledSize: this.props.google.maps.Size(10, 10),
                            position: {lat: flight.latitude, lng: flight.longitude},
                             }}
                        name={flight.flight_id} />;
    };

    /** Get the polylines from flight segments */
    createPolylines = () => {
        if (this.props.flightClickedPlan === undefined)
            return [];
        return (
            <Polyline
                path={this.getLines(this.props.flightClickedPlan)}
                strokeColor={'#ff2c5c'}
                strokeOpacity={1}
                strokeWeight={4} />)}


    getLines = (flightPlan) => Object.values(flightPlan.segments).map(value => ({lat: value.latitude, lng: value.longitude}))

    getCenter = () => {
        let flight = this.props.flightsList[0];
        if (this.props.flightClicked)
            flight = this.props.flightClicked;
        return {lat: flight.latitude, lng: flight.longitude};
    }

    render() {
        return (
            <Map google={this.props.google} style={containerStyle}
                 containerStyle={containerStyle} zoom={3} center={this.getCenter()}>
                {this.createMarkers()}
                {this.createPolylines()}
            </Map>
        );
    }
}

const containerStyle = {
    position: 'absolute',
    left: 0,
    right: 0,
    top: 0,
    bottom: 0
}

export default GoogleApiWrapper({
    apiKey: apiKey
})(FlightMap)