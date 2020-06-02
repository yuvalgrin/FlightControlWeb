import {Map, Polyline, Marker, GoogleApiWrapper} from 'google-maps-react';
import planePng from './resources/regular-airplane-24.png';
import markedPlanePng from './resources/marked-airplane-27.png';
import './FlightMap.css';
import * as React from "react";

const apiKey = 'AIzaSyAjngpsKv9PcK9NqXrHi8VdNi_5VI287CM';
const defaultLat = 33.790755;
const defaultLon = 37.203170;

export class FlightMap extends React.Component  {
    /** This method will iterate all of the flight plans and print them into the Map object */
    createMarkers = () => Object.values(this.props.flightsList).map(value => this.getMarker(value))

    /** Get the markers from flights list */
    getMarker = (flight)  => {
        let isClicked = false;
        if (this.props.flightClicked && this.props.flightClicked.flight_id === flight.flight_id)
            isClicked = true;
        return <Marker key={flight.flight_id} onClick={() => this.props.setFlightClick(flight)}
                        position = {{lng: flight.longitude, lat: flight.latitude}}
                        icon={{
                            url: isClicked ? markedPlanePng : planePng,
                            position: {lng: flight.longitude, lat: flight.latitude},
                             }}
                        name={flight.flight_id} />;
    };

    /** Get the polylines from flight segments */
    createPolylines = () => {
        if (this.props.flightClicked === [] || this.props.flightClickedPlan === undefined)
            return [];
        return (
            <Polyline
                path={this.getLines(this.props.flightClickedPlan)}
                strokeColor={'#ff2c5c'}
                strokeOpacity={1}
                strokeWeight={4} />)}


    getLines = (flightPlan) => [this.getInitialLocation()].concat(Object.values(flightPlan.segments).map(value =>
        ({lng: value.longitude, lat: value.latitude})))

    getInitialLocation = () => ({lng: this.props.flightClickedPlan.initial_location.longitude,
        lat: this.props.flightClickedPlan.initial_location.latitude})

    /** Center into clicked flight else to default location */
    getCenter = () => {
        if (this.props.flightClicked && this.props.flightClicked.length !== 0) {
            const lat = parseFloat(this.props.flightClicked.latitude);
            const lon = parseFloat(this.props.flightClicked.longitude);
            return {
                lat: lat,
                lng: lon
            }
        }
        else
            return {lat: defaultLat, lng: defaultLon};
    }

    /** Center map only on init */
    componentDidMount() {
        this.props.setShouldCenterMap(true);
    }

    /** Center map only once per click */
    componentDidUpdate(prevProps, prevState) {
        this.props.setShouldCenterMap(false);
    }

    /** Create map and center it */
    getMapWithCenter = () => (
        <Map google={this.props.google} zoom={4} center={this.getCenter()} onClick={() => this.props.onFlightDeselect()}>
            {this.createMarkers()}
            {this.createPolylines()}
        </Map>
    )

    /** Create map without centering it */
    getMapWithoutCenter = () => (
        <Map google={this.props.google} zoom={4} onClick={() => this.props.onFlightDeselect()}>
            {this.createMarkers()}
            {this.createPolylines()}
        </Map>
    )

    /** Create map and center only at first */
    getMap = () => {
        if (this.props.shouldCenterMap) {
            return this.getMapWithCenter();
        }
        return this.getMapWithoutCenter();
    }

    componentDidCatch(error, errorInfo) {
        console.log(error);
    }

    render() {
        return (
            this.getMap()
        );
    }
}

export default GoogleApiWrapper({
    apiKey: apiKey
})(FlightMap)