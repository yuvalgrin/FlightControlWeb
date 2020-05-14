import {Map, Polyline, Marker, GoogleApiWrapper} from 'google-maps-react';
import planePng from './resources/pngwave.png';
import * as React from "react";

const apiKey = 'AIzaSyAjngpsKv9PcK9NqXrHi8VdNi_5VI287CM';

let myLatLng = {lat: -25.363, lng: 131.044};
let flightClicked;
let flightsList;

export class FlightMap extends React.Component  {
    render() {
        return (
            <Map google={this.props.google} style={containerStyle}
                           containerStyle={containerStyle} zoom={14} center={myLatLng}>
                <Marker onClick={onAirplaneClick()}
                        icon={{
                    url: planePng,
                    position: {myLatLng},
                    origin: new this.props.google.maps.Point(0, 0),
                    anchor: new this.props.google.maps.Point(0, 32) }}
                        name={'Airplane'} />
            </Map>
        );
    }
}

/**
 *  This method will iterate all of the flight plans and print them into the Map object
 * @param google
 * @param flight
 * @param isClicked
 * @returns {[]}
 */
const flightIterator = ({google,flight,isClicked}) => {
    let items = [];
    let onClick;

    for (let [key, value] of Object.entries(flightsList)) {
        items.push( getMarkers(null,null,null) )
        items.push( getPolylines() )
    }
    return (items);
};

// Get the markers from flights list
const getMarkers = ({google,flight,isClicked}) => {
    let items = [];
    let onClick;

        items.push(
            <Marker onClick={onAirplaneClick(flight)}
                    icon={{
                        url: isClicked ? planePng : planePng,
                        position: {myLatLng},
                        origin: new google.maps.Point(0, 0),
                        anchor: new google.maps.Point(0, 32) }}
                    name={flight.flight_id} />
        )

    return (items);
};


// Get the polylines from flight segments
const getPolylines = ({flight,isClicked}) => {
    let items = [];
    let onClick;

    for (let [key, value] of Object.entries(flightsList)) {
        let points = [{lat: 25.774, lng: -80.190},
                        {lat: 25.774, lng: -80.190}];
        items.push(
            <Polyline
                path={points}
                strokeColor={isClicked?"#0000FF":"#FFFF00"}
                strokeOpacity={isClicked?1:0.7}
                strokeWeight={isClicked?4:2} />
        )
    }

    return (items);
};

const onAirplaneClick = (e) => {
    flightClicked = e;
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